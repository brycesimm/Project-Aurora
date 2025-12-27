// Project Aurora - Compute Module
// Provisions App Service Plan and Function App

@description('Azure region for resources')
param location string

@description('Environment name')
param environmentName string

@description('Unique suffix for globally unique names')
param uniqueSuffix string

@description('Storage connection string')
param storageConnectionString string

@description('Application Insights connection string')
param appInsightsConnectionString string

@description('Blob container name for content')
param contentContainerName string

// ============================================================================
// Variables
// ============================================================================

var appServicePlanName = 'plan-aurora-${environmentName}'
// Shorten function app name to avoid length issues
var functionAppName = 'func-aurora-${environmentName}-${take(uniqueSuffix, 8)}'

// ============================================================================
// Resources
// ============================================================================

// App Service Plan (Consumption Plan - Serverless, Free Tier)
resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
	name: appServicePlanName
	location: location
	sku: {
		name: 'Y1' // Consumption tier (serverless)
		tier: 'Dynamic'
	}
	properties: {
		reserved: true // Required for Linux
	}
	tags: {
		Environment: environmentName
		Project: 'Aurora'
		Note: 'Consumption Plan - pay-per-execution serverless deployment (free tier)'
	}
}

// Function App
resource functionApp 'Microsoft.Web/sites@2023-12-01' = {
	name: functionAppName
	location: location
	kind: 'functionapp,linux'
	properties: {
		serverFarmId: appServicePlan.id
		httpsOnly: true
		siteConfig: {
			linuxFxVersion: 'DOTNET-ISOLATED|9.0' // .NET 9 Isolated process
			appSettings: [
				{
					name: 'AzureWebJobsStorage'
					value: storageConnectionString
				}
				{
					name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
					value: storageConnectionString
				}
				{
					name: 'WEBSITE_CONTENTSHARE'
					value: toLower(functionAppName)
				}
				{
					name: 'FUNCTIONS_EXTENSION_VERSION'
					value: '~4'
				}
				{
					name: 'FUNCTIONS_WORKER_RUNTIME'
					value: 'dotnet-isolated'
				}
				{
					name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
					value: appInsightsConnectionString
				}
				{
					name: 'TableStorageConnectionString'
					value: storageConnectionString
				}
				{
					name: 'BlobStorageConnectionString'
					value: storageConnectionString
				}
				{
					name: 'ContentContainerName'
					value: contentContainerName
				}
			]
			cors: {
				allowedOrigins: [
					'*' // Allow all origins for beta (restrict in production)
				]
				supportCredentials: false
			}
			ftpsState: 'Disabled'
			minTlsVersion: '1.2'
		}
	}
	tags: {
		Environment: environmentName
		Project: 'Aurora'
	}
}

// ============================================================================
// Outputs
// ============================================================================

@description('Function App name')
output functionAppName string = functionApp.name

@description('Function App default hostname')
output functionAppHostName string = functionApp.properties.defaultHostName

@description('Function App base URL')
output apiBaseUrl string = 'https://${functionApp.properties.defaultHostName}'
