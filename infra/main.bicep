targetScope = 'subscription'

@allowed([
  'Dev'
  'Test'
  'Prod'
])
@description('Required. Environment name.')
param environment string

@description('Required. Project name to use in naming resources.')
param projectName string

@description('Optional. The name of the resource group to create.')
param rgName string = 'rg-${toLower(projectName)}-${toLower(environment)}'

@description('Optional. The location for all resources.')
param location string = 'swedencentral'

@description('Optional. Resource Group tags.')
param tags object = {}

@description('Required. SQL server administrator login.')
param sqlAdminLogin string

@description('Required. SQL server administrator Object Id.')
param sqlAdminObjectId string

var env = toLower(environment)
var deploymentName = deployment().name

resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-11-01' = {
  name: rgName
  location: location
}

module rg 'resourceGroup.bicep' = {
  scope: resourceGroup
  name: '${deploymentName}-resources'
  params: {
    env: env
    projectName: toLower(projectName)
    tags: tags
    sqlAdminLogin: sqlAdminLogin
    sqlAdminObjectId: sqlAdminObjectId
  }
}
