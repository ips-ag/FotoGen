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
param logAnalyticsName string = 'law-${projectname}-${env}'

@description('Optional. Application Insights resource name.')
param appInsightsName string = 'ai-${projectname}-${env}'

@description('Optional. The name of the App Service Plan to create.')
param appServicePlanName string = 'asp-${projectname}-${env}'

@description('Optional. The SKU for the App Service Plan.')
@allowed([
  'B1'
  'P1'
])
param appServicePlanSku string = 'B1'

@description('Optional. The name of the UI App Service to create.')
param uiWebAppName string = 'app-${projectname}-ui-${env}'

@description('Optional. The name of the API App Service to create.')
param apiWebAppName string = 'app-${projectname}-api-${env}'

@description('Optional. The name of the Storage Account to create.')
param storageAccountName string = 'sto${projectname}${env}'

@description('Optional. Indicates number fo days to retain deleted items (containers, blobs, snapshosts, versions). Default value is 7')
param daysSoftDelete int = 7

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

module storageAccount 'storageAccount.bicep' = {
  name: storageAccountName
  params: {
    storageAccountName: storageAccountName
    location: location
    tags: tags
    daysSoftDelete: daysSoftDelete
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
  }
}

resource apiWebAppConfig 'Microsoft.Web/sites/config@2024-04-01' = {
  name: '${apiWebAppName}/web'
  dependsOn: [apiWebApp]
  properties: {
    linuxFxVersion: 'DOTNETCORE|9.0'
    cors: {
      allowedOrigins: [uiWebApp.outputs.endpoint]
    }
    appSettings: [
      { name: 'APPLICATIONINSIGHTS_CONNECTION_STRING', value: appInsights.properties.ConnectionString }
    ]
  }
}
