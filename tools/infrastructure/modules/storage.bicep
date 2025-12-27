// Project Aurora - Storage Module
// Provisions Storage Account, Blob Container, and Table Storage

@description('Azure region for resources')
param location string

@description('Environment name')
param environmentName string

@description('Unique suffix for globally unique names')
param uniqueSuffix string

// ============================================================================
// Variables
// ============================================================================

// Storage account names must be 3-24 characters, lowercase and numbers only
var storageAccountName = 'staurora${take(uniqueSuffix, 16)}'
var blobContainerName = 'aurora-content'

// ============================================================================
// Resources
// ============================================================================

// Storage Account
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' = {
	name: storageAccountName
	location: location
	sku: {
		name: 'Standard_LRS' // Locally redundant storage
	}
	kind: 'StorageV2'
	properties: {
		accessTier: 'Hot'
		supportsHttpsTrafficOnly: true
		minimumTlsVersion: 'TLS1_2'
		allowBlobPublicAccess: false
	}
	tags: {
		Environment: environmentName
		Project: 'Aurora'
	}
}

// Blob Service
resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2023-05-01' = {
	parent: storageAccount
	name: 'default'
}

// Content Container (for content.json)
resource contentContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-05-01' = {
	parent: blobService
	name: blobContainerName
	properties: {
		publicAccess: 'None' // Private access only
	}
}

// Table Service
resource tableService 'Microsoft.Storage/storageAccounts/tableServices@2023-05-01' = {
	parent: storageAccount
	name: 'default'
}

// Reactions Table
resource reactionsTable 'Microsoft.Storage/storageAccounts/tableServices/tables@2023-05-01' = {
	parent: tableService
	name: 'Reactions'
}

// ============================================================================
// Outputs
// ============================================================================

@description('Storage Account name')
output storageAccountName string = storageAccount.name

@description('Storage Account connection string')
@secure()
output storageConnectionString string = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value};EndpointSuffix=core.windows.net'

@description('Blob container name')
output containerName string = blobContainerName
