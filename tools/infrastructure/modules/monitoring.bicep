// Project Aurora - Monitoring Module
// Provisions Application Insights for telemetry and diagnostics

@description('Azure region for resources')
param location string

@description('Environment name')
param environmentName string

// ============================================================================
// Variables
// ============================================================================

var appInsightsName = 'appi-aurora-${environmentName}'

// ============================================================================
// Resources
// ============================================================================

// Application Insights
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
	name: appInsightsName
	location: location
	kind: 'web'
	properties: {
		Application_Type: 'web'
		Request_Source: 'rest'
		RetentionInDays: 30 // Free tier: 30 days
		publicNetworkAccessForIngestion: 'Enabled'
		publicNetworkAccessForQuery: 'Enabled'
	}
	tags: {
		Environment: environmentName
		Project: 'Aurora'
	}
}

// ============================================================================
// Outputs
// ============================================================================

@description('Application Insights name')
output appInsightsName string = appInsights.name

@description('Application Insights connection string')
output connectionString string = appInsights.properties.ConnectionString

@description('Application Insights instrumentation key')
output instrumentationKey string = appInsights.properties.InstrumentationKey
