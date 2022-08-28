param applicationInsightsInstrumentationKey string
param storageAccountName string
param storageAccountAccessKey string
// param appConfigurationName string
param functionAppName string
param functionAppStagingSlotName string

@description('Value of "APP_CONFIGURATION_LABEL" appsetting for production slot')
param appConfiguration_appConfigLabel_value_production string = 'production'
@description('Value of "APP_CONFIGURATION_LABEL" appsetting for staging slot')
param appConfiguration_appConfigLabel_value_staging string = 'staging'

var BASE_SLOT_APPSETTINGS = {
  // APP_CONFIGURATION_NAME: appConfigurationName
  APPINSIGHTS_INSTRUMENTATIONKEY: applicationInsightsInstrumentationKey
  APPLICATIONINSIGHTS_CONNECTION_STRING: 'InstrumentationKey=${applicationInsightsInstrumentationKey}'
  AzureWebJobsStorage: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccountAccessKey}'
  FUNCTIONS_EXTENSION_VERSION: '~4'
  FUNCTIONS_WORKER_RUNTIME: 'dotnet-isolated'
  WEBSITE_CONTENTSHARE: toLower(storageAccountName)
  WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccountAccessKey}'
}

var PROD_SLOT_APPSETTINGS = {
  APP_CONFIGURATION_LABEL: appConfiguration_appConfigLabel_value_production
}
@description('Set app settings on production slot')
resource functionAppSettings 'Microsoft.Web/sites/config@2021-03-01' = {
  name: '${functionAppName}/appsettings'
  properties: union(BASE_SLOT_APPSETTINGS, PROD_SLOT_APPSETTINGS)
}

/* update staging slot with unique settings */
var STG_SLOT_APPSETTINGS = {
  APP_CONFIGURATION_LABEL: appConfiguration_appConfigLabel_value_staging
}
@description('Set app settings on production slot')
resource stagingFunctionAppSettings 'Microsoft.Web/sites/config@2021-03-01' = {
  name: '${functionAppStagingSlotName}/appsettings'
  properties: union(BASE_SLOT_APPSETTINGS, STG_SLOT_APPSETTINGS)
}
