@description('Required. Project name to use in naming resources.')
param projectName string

@description('Required. Environment name.')
param env string

@description('Optional. Resource tags. Defaults to resource group tags.')
param tags object = resourceGroup().tags

@description('Required. Key Vault name for storing secrets.')
param keyVaultName string

@description('Optional. Data location for Communication Services. Defaults to Europe.')
param dataLocation string = 'Europe'

@description('Optional. The name of the Communication Services resource.')
param communicationServicesName string = 'cs-${projectName}-${env}'

@description('Optional. The name of the Email Communication Service resource.')
param emailCommunicationServiceName string = 'acs-${projectName}-${env}'

@description('Optional. The domain name for the managed email service.')
param emailDomainName string = 'AzureManagedDomain'

resource emailCommunicationService 'Microsoft.Communication/emailServices@2023-04-01' = {
  name: emailCommunicationServiceName
  location: 'global'
  tags: tags
  properties: {
    dataLocation: dataLocation
  }

  resource emailDomain 'domains' = {
    name: emailDomainName
    location: 'global'
    tags: tags
    properties: {
      domainManagement: 'AzureManaged'
    }
  }
}

resource communicationServices 'Microsoft.Communication/communicationServices@2023-04-01' = {
  name: communicationServicesName
  location: 'global'
  tags: tags
  properties: {
    dataLocation: dataLocation
    linkedDomains: [emailCommunicationService::emailDomain.id]
  }
}

resource _ 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  name: '${keyVaultName}/CommunicationServices--ConnectionString'
  properties: {
    value: communicationServices.listKeys().primaryConnectionString
  }
}

resource keyVaultSecretSenderEmail 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  name: '${keyVaultName}/Email--SenderAddress'
  properties: {
    value: 'DoNotReply@${emailCommunicationService::emailDomain.properties.mailFromSenderDomain}'
  }
}

output connectionStringSecretName string = 'CommunicationServices--ConnectionString'
output senderEmailSecretName string = 'Email--SenderAddress'
