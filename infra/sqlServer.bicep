@description('Required. The name of the Azure SQL to create.')
param serverName string

@description('Required. The name of the Azure SQL Database to create.')
param databaseName string

@description('Required. SQL server administrator login.')
param adminLogin string

@description('Required. SQL server administrator Object Id.')
param adminObjectId string

@description('Optional. Resource location. Defaults to resource group location')
param location string = resourceGroup().location

// @description('Optional. Resource tags. Defaults to resource group tags.')
// param tags object = resourceGroup().tags

@description('Optional. The minimal TLS version for the SQL server.')
@allowed([
  '1.0'
  '1.1'
  '1.2'
  '1.3'
  'None'
])
param minimalTlsVersion string = '1.2'

@description('Optional. The maximum size of the SQL Database in GB. Default is 1 GB.')
@minValue(1)
param maxSizeInGB int = 1

var tenantId = subscription().tenantId
var maxSizeInBytes = maxSizeInGB * 1024 * 1024 * 1024

resource server 'Microsoft.Sql/servers@2024-05-01-preview' = {
  identity: {
    type: 'SystemAssigned'
  }
  location: location
  name: serverName
  // tags: tags
  properties: {
    administrators: {
      administratorType: 'ActiveDirectory'
      azureADOnlyAuthentication: true
      login: adminLogin
      principalType: 'User'
      sid: adminObjectId
      tenantId: tenantId
    }
    minimalTlsVersion: minimalTlsVersion
    publicNetworkAccess: 'Enabled'
    restrictOutboundNetworkAccess: 'Disabled'
    version: '12.0'
  }

  resource threatProtection 'advancedThreatProtectionSettings' = {
    name: 'Default'
    properties: {
      state: 'Disabled'
    }
  }

  resource azureAdOnly 'azureADOnlyAuthentications' = {
    name: 'Default'
    properties: {
      azureADOnlyAuthentication: true
    }
  }

  resource firewallRules 'firewallRules' = {
    name: 'AllowAllWindowsAzureIps'
    properties: {
      startIpAddress: '0.0.0.0'
      endIpAddress: '0.0.0.0'
    }
  }

  resource securityAlertPolicies 'securityAlertPolicies' = {
    name: 'Default'
    properties: {
      state: 'Disabled'
      disabledAlerts: [
        ''
      ]
      emailAddresses: [
        ''
      ]
      emailAccountAdmins: false
      retentionDays: 0
    }
  }
}

resource database 'Microsoft.Sql/servers/databases@2024-05-01-preview' = {
  parent: server
  name: databaseName
  location: location
  // tags: tags
  sku: {
    name: 'GP_S_Gen5'
    tier: 'GeneralPurpose'
    family: 'Gen5'
    capacity: 1
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    createMode: 'Default'
    maxSizeBytes: maxSizeInBytes
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: false
    readScale: 'Disabled'
    autoPauseDelay: 15
    minCapacity: json('0.5')
    requestedBackupStorageRedundancy: 'Local'
    isLedgerOn: false
    availabilityZone: 'NoPreference'
  }

  resource threatProtection 'advancedThreatProtectionSettings' = {
    name: 'Default'
    properties: {
      state: 'Disabled'
    }
  }

  resource vulnerabilityAssessments 'vulnerabilityAssessments' = {
    name: 'Default'
    properties: {
      recurringScans: {
        emailSubscriptionAdmins: true
        isEnabled: false
      }
    }
  }

  resource backupShortTermRetentionPolicies 'backupShortTermRetentionPolicies' = {
    name: 'default'
    properties: {
      retentionDays: 7
      diffBackupIntervalInHours: 12
    }
  }

  resource geoBackupPolicies 'geoBackupPolicies' = {
    name: 'Default'
    properties: {
      state: 'Disabled'
    }
  }
}
