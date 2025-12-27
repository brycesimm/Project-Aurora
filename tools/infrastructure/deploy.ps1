# Project Aurora - Infrastructure Deployment Script
# This script deploys the Azure infrastructure using Bicep templates

param(
	[Parameter(Mandatory=$false)]
	[ValidateSet('dev', 'beta', 'prod')]
	[string]$Environment = 'beta',

	[Parameter(Mandatory=$false)]
	[string]$Location = 'eastus',

	[Parameter(Mandatory=$false)]
	[string]$ResourceGroupName = "rg-aurora-$Environment"
)

# Script configuration
$ErrorActionPreference = 'Stop'
$InformationPreference = 'Continue'

Write-Information "=========================================="
Write-Information "Project Aurora - Infrastructure Deployment"
Write-Information "=========================================="
Write-Information "Environment:      $Environment"
Write-Information "Location:         $Location"
Write-Information "Resource Group:   $ResourceGroupName"
Write-Information ""

# Verify Azure CLI is installed
Write-Information "Verifying Azure CLI installation..."
try {
	$azVersion = az version --output json | ConvertFrom-Json
	Write-Information "✓ Azure CLI version: $($azVersion.'azure-cli')"
} catch {
	Write-Error "Azure CLI is not installed. Please install from: https://aka.ms/install-az-cli"
	exit 1
}

# Verify user is logged in
Write-Information "Verifying Azure authentication..."
$account = az account show --output json 2>$null | ConvertFrom-Json
if (-not $account) {
	Write-Warning "Not logged in to Azure. Running 'az login'..."
	az login
	$account = az account show --output json | ConvertFrom-Json
}
Write-Information "✓ Logged in as: $($account.user.name)"
Write-Information "✓ Subscription: $($account.name)"
Write-Information ""

# Create Resource Group if it doesn't exist
Write-Information "Checking resource group..."
$rgExists = az group exists --name $ResourceGroupName
if ($rgExists -eq 'false') {
	Write-Information "Creating resource group '$ResourceGroupName'..."
	az group create --name $ResourceGroupName --location $Location --output none
	Write-Information "✓ Resource group created"
} else {
	Write-Information "✓ Resource group exists"
}
Write-Information ""

# Deploy Bicep template
Write-Information "Deploying infrastructure (this may take 2-3 minutes)..."
$deploymentName = "aurora-deployment-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

try {
	$deployment = az deployment group create `
		--name $deploymentName `
		--resource-group $ResourceGroupName `
		--template-file "$(Split-Path $PSScriptRoot)\infrastructure\main.bicep" `
		--parameters environmentName=$Environment location=$Location `
		--output json | ConvertFrom-Json

	Write-Information ""
	Write-Information "=========================================="
	Write-Information "Deployment Complete!"
	Write-Information "=========================================="
	Write-Information "Storage Account:    $($deployment.properties.outputs.storageAccountName.value)"
	Write-Information "Function App:       $($deployment.properties.outputs.functionAppName.value)"
	Write-Information "API Base URL:       $($deployment.properties.outputs.apiBaseUrl.value)"
	Write-Information "Environment:        $($deployment.properties.outputs.environment.value)"
	Write-Information "=========================================="
	Write-Information ""
	Write-Information "✓ Infrastructure deployment successful!"
	Write-Information ""
	Write-Information "Next Steps:"
	Write-Information "  1. Deploy the Aurora.Api function code"
	Write-Information "  2. Upload content.json to blob storage"
	Write-Information "  3. Update MAUI app configuration with API URL"

} catch {
	Write-Error "Deployment failed: $_"
	exit 1
}
