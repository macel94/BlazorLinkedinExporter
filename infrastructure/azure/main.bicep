param location string = resourceGroup().location

@description('Resource name prefix')
param resourceNamePrefix string
var envResourceNamePrefix = toLower(resourceNamePrefix)

@description('Deployment name (used as parent ID for child deployments)')
param deploymentNameId string = '0000000000'

@description('Name of the staging deployment slot')
var functionAppStagingSlot = 'staging'

resource azStorageAccount 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: '${envResourceNamePrefix}storage'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}
var azStorageAccountPrimaryAccessKey = listKeys(azStorageAccount.id, azStorageAccount.apiVersion).keys[0].value

resource azAppInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${envResourceNamePrefix}-ai'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}
var azAppInsightsInstrumentationKey = azAppInsights.properties.InstrumentationKey

resource azHostingPlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: '${envResourceNamePrefix}-asp'
  location: location
  kind: 'linux'
  sku: {
    name: 'B1'
  }
}

resource azFunctionApp 'Microsoft.Web/sites@2021-03-01' = {
  name: '${envResourceNamePrefix}-app'
  kind: 'functionapp'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    httpsOnly: true
    serverFarmId: azHostingPlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|6.0'
    }
  }
}

resource azFunctionSlotStaging 'Microsoft.Web/sites/slots@2022-03-01' = {
  name: '${azFunctionApp.name}/${functionAppStagingSlot}'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    enabled: true
    httpsOnly: true
  }
}

resource azAppConfiguration 'Microsoft.Web/sites/config@2021-03-01' = {
  name: 'slotConfigNames'
  parent: azFunctionApp
  properties: {
    appSettingNames: [
      'APP_CONFIGURATION_LABEL'
    ]
  }
}

module appService_appSettings 'appservice-appsettings-config.bicep' = {
  name: '${deploymentNameId}-appservice-config'
  params: {
    appConfigurationName: azAppConfiguration.name
    appConfiguration_appConfigLabel_value_production: 'production'
    appConfiguration_appConfigLabel_value_staging: 'staging'
    applicationInsightsInstrumentationKey: azAppInsightsInstrumentationKey
    storageAccountName: azStorageAccount.name
    storageAccountAccessKey: azStorageAccountPrimaryAccessKey
    functionAppName: azFunctionApp.name
    functionAppStagingSlotName: azFunctionSlotStaging.name
  }
}

output appConfigName string = azAppConfiguration.name
output appInsightsInstrumentionKey string = azAppInsightsInstrumentationKey
output functionAppName string = azFunctionApp.name
output functionAppSlotName string = functionAppStagingSlot
