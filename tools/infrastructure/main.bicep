// Project Aurora - Main Infrastructure Orchestrator
// This file coordinates all infrastructure modules

targetScope = 'resourceGroup'

@description('Azure region for all resources')
param location string = resourceGroup().location

@description('Environment name (dev, beta, prod)')
@allowed([
	'dev'
	'beta'
	'prod'
])
param environmentName string = 'beta'

@description('Unique suffix for globally unique resource names')
param uniqueSuffix string = uniqueString(resourceGroup().id)

// ============================================================================
// Module: Storage
// ============================================================================

module storage 'modules/storage.bicep' = {
	name: 'storage-deployment'
	params: {
		location: location
		environmentName: environmentName
		uniqueSuffix: uniqueSuffix
	}
}

// ============================================================================
// Module: Monitoring
// ============================================================================

module monitoring 'modules/monitoring.bicep' = {
	name: 'monitoring-deployment'
	params: {
		location: location
		environmentName: environmentName
	}
}

// ============================================================================
// Module: Compute
// ============================================================================

module compute 'modules/compute.bicep' = {
	name: 'compute-deployment'
	params: {
		location: location
		environmentName: environmentName
		uniqueSuffix: uniqueSuffix
		storageConnectionString: storage.outputs.storageConnectionString
		appInsightsConnectionString: monitoring.outputs.connectionString
		contentContainerName: storage.outputs.containerName
	}
	// Dependencies are implicit through parameter passing
}

// ============================================================================
// Outputs
// ============================================================================

@description('Storage Account name')
output storageAccountName string = storage.outputs.storageAccountName

@description('Function App name')
output functionAppName string = compute.outputs.functionAppName

@description('API Base URL')
output apiBaseUrl string = compute.outputs.apiBaseUrl

@description('Application Insights Instrumentation Key')
output appInsightsInstrumentationKey string = monitoring.outputs.instrumentationKey

@description('Deployment Environment')
output environment string = environmentName
