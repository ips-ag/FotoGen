targetScope = 'resourceGroup'

@description('Required. Environment name.')
param env string

@description('Required. Project name to use in naming resources.')
param projectName string

@description('Optional. Resource location. Defaults to resource group location')
param location string = resourceGroup().location

@description('Optional. Resource tags. Defaults to resource group tags.')
param tags object = resourceGroup().tags

@description('Optional. Log Analytics workspace resource name.')
param logAnalyticsName string = 'law-${projectName}-${env}'

@description('Optional. Application Insights resource name.')
param appInsightsName string = 'ai-${projectName}-${env}'

@description('Optional. The name of the App Service Plan to create.')
param appServicePlanName string = 'asp-${projectName}-${env}'

@description('Optional. The SKU for the App Service Plan.')
@allowed([
  'B1'
  'P1'
])
param appServicePlanSku string = 'B1'

@description('Optional. The name of the UI App Service to create.')
param uiWebAppName string = 'app-${projectName}-ui-${env}'

@description('Optional. The name of the API App Service to create.')
param apiWebAppName string = 'app-${projectName}-api-${env}'

@description('Optional. The name of the Storage Account to create.')
param storageAccountName string = 'sto${projectName}${env}'

@description('Optional. The name of the Key Vault to create.')
param keyVaultName string = 'kv-${projectName}-${env}'

@description('Optional. Indicates number fo days to retain deleted items (containers, blobs, snapshosts, versions). Default value is 7')
param daysSoftDelete int = 7

@description('Optional. Enable Key Vault purge protection. Default is false.')
param enableKeyVaultPurgeProtection bool = false

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: logAnalyticsName
  location: location
  tags: tags
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    workspaceCapping: {
      dailyQuotaGb: 5
    }
    retentionInDays: 30
    features: {
      enableLogAccessUsingOnlyResourcePermissions: true
    }
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  kind: 'web'
  location: location
  tags: tags
  properties: {
    Application_Type: 'web'
    IngestionMode: 'LogAnalytics'
    SamplingPercentage: 50
    WorkspaceResourceId: logAnalytics.id
    Flow_Type: 'Bluefield'
    Request_Source: 'rest'
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2024-11-01' = {
  name: keyVaultName
  location: location
  tags: tags
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enableRbacAuthorization: true
    enableSoftDelete: true
    enablePurgeProtection: enableKeyVaultPurgeProtection ? true : null
    softDeleteRetentionInDays: 10
    publicNetworkAccess: 'Enabled'
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
    }
  }
}

resource _ 'Microsoft.KeyVault/vaults/secrets@2024-11-01' = {
  parent: keyVault
  name: 'ApplicationInsights--ConnectionString'
  properties: {
    value: appInsights.properties.ConnectionString
  }
}

module storageAccount 'storageAccount.bicep' = {
  name: storageAccountName
  params: {
    storageAccountName: storageAccountName
    location: location
    tags: tags
    daysSoftDelete: daysSoftDelete
  }
}

module communicationServices 'communicationServices.bicep' = {
  name: 'communicationServices'
  params: {
    projectName: projectName
    env: env
    tags: tags
    keyVaultName: keyVault.name
  }
}

resource appServicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: appServicePlanName
  location: location
  tags: tags
  sku: {
    name: appServicePlanSku
    capacity: 1
  }
  properties: {
    reserved: true
  }
  kind: 'linux'
}

module uiWebApp 'webApp.bicep' = {
  name: uiWebAppName
  params: {
    name: uiWebAppName
    location: location
    tags: tags
    appServicePlanId: appServicePlan.id
    clientAffinityEnabled: false
    httpsOnly: true
    kind: 'app,linux'
  }
}

module apiWebApp 'webApp.bicep' = {
  name: apiWebAppName
  dependsOn: [uiWebApp]
  params: {
    name: apiWebAppName
    location: location
    tags: tags
    appServicePlanId: appServicePlan.id
    clientAffinityEnabled: false
    httpsOnly: true
    kind: 'app,linux'
    useManagedIdentity: true
  }
}

resource keyVaultSecretsUserRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(keyVault.id, apiWebAppName, '4633458b-17de-408a-b874-0445c86b69e6')
  scope: keyVault
  properties: {
    roleDefinitionId: subscriptionResourceId(
      'Microsoft.Authorization/roleDefinitions',
      '4633458b-17de-408a-b874-0445c86b69e6'
    ) // Key Vault Secrets User
    principalId: apiWebApp.outputs.principalId
    principalType: 'ServicePrincipal'
  }
}

resource apiWebAppConfig 'Microsoft.Web/sites/config@2024-04-01' = {
  name: '${apiWebAppName}/web'
  dependsOn: [apiWebApp, keyVaultSecretsUserRoleAssignment]
  properties: {
    linuxFxVersion: 'DOTNETCORE|9.0'
    cors: {
      allowedOrigins: [uiWebApp.outputs.endpoint]
    }
    appSettings: [
      {
        name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
        value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=ApplicationInsights--ConnectionString)'
      }
      {
        name: 'EMAIL__CONNECTIONSTRING'
        value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=${communicationServices.outputs.connectionStringSecretName})'
      }
      {
        name: 'EMAIL__SENDEREMAIL'
        value: '@Microsoft.KeyVault(VaultName=${keyVaultName};SecretName=${communicationServices.outputs.senderEmailSecretName})'
      }
    ]
  }
}

// Outputs
output keyVaultName string = keyVault.name
output keyVaultId string = keyVault.id
output communicationServicesConnectionStringSecretName string = communicationServices.outputs.connectionStringSecretName
output communicationServicesSenderEmailSecretName string = communicationServices.outputs.senderEmailSecretName
