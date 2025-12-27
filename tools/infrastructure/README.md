# Project Aurora - Infrastructure as Code

This directory contains Bicep templates and deployment scripts for provisioning Azure infrastructure.

## Structure

```
infrastructure/
├── main.bicep              # Main orchestrator
├── parameters.json         # Environment-specific parameters
├── deploy.ps1             # PowerShell deployment script
└── modules/
    ├── storage.bicep      # Storage Account, Blob, Table
    ├── monitoring.bicep   # Application Insights
    └── compute.bicep      # App Service Plan, Function App
```

## Quick Start

### Deploy Infrastructure

```powershell
# Deploy to beta environment (default)
.\deploy.ps1

# Deploy to dev environment
.\deploy.ps1 -Environment dev -ResourceGroupName rg-aurora-dev

# Deploy to different region
.\deploy.ps1 -Location westus2
```

### Manual Deployment (Azure CLI)

```bash
# Create resource group
az group create --name rg-aurora-beta --location eastus

# Deploy Bicep template
az deployment group create \
  --name aurora-deployment \
  --resource-group rg-aurora-beta \
  --template-file main.bicep \
  --parameters parameters.json
```

## Resources Provisioned

### Storage Module (`modules/storage.bicep`)
- **Storage Account** (Standard LRS)
  - Blob Container: `aurora-content` (for content.json)
  - Table: `Reactions` (for user reactions)

### Monitoring Module (`modules/monitoring.bicep`)
- **Application Insights** (30-day retention, free tier)

### Compute Module (`modules/compute.bicep`)
- **App Service Plan** (Consumption Y1 - serverless)
- **Function App** (.NET 9 Isolated, Linux)
  - CORS: Enabled for all origins (beta only)
  - HTTPS Only: Enforced
  - TLS: 1.2 minimum

## Environment Configuration

Modify `parameters.json` for different environments:

```json
{
  "parameters": {
    "environmentName": {
      "value": "beta"  // Options: dev, beta, prod
    },
    "location": {
      "value": "eastus"
    }
  }
}
```

## Cost Estimate (Beta Environment)

- **Storage Account:** ~$0.02/GB/month + $0.0004/10K operations
- **Function App:** Free tier (1M executions/month)
- **Application Insights:** Free tier (5GB/month)

**Expected Monthly Cost:** $0 - $5 for typical beta usage

## Outputs

After deployment, the following values are available:

- `storageAccountName`: Name of the storage account
- `functionAppName`: Name of the function app
- `apiBaseUrl`: Full HTTPS URL for API calls
- `appInsightsInstrumentationKey`: For telemetry configuration

## Troubleshooting

### Deployment Fails with "Name not available"
The unique suffix ensures globally unique names, but if you encounter conflicts:
1. Delete the existing resource group: `az group delete --name rg-aurora-beta`
2. Re-run the deployment

### Function App shows "Application Error"
This is normal before code deployment. Deploy the Aurora.Api project to resolve.

## Next Steps After Deployment

1. **Deploy API Code:**
   ```bash
   cd src/Aurora.Api
   func azure functionapp publish <functionAppName>
   ```

2. **Upload Initial Content:**
   ```bash
   az storage blob upload \
     --account-name <storageAccountName> \
     --container-name aurora-content \
     --name content.json \
     --file ../../sample.content.json
   ```

3. **Update MAUI App Configuration:**
   - Edit `src/Aurora/appsettings.json`
   - Set `ApiBaseUrl` to the output `apiBaseUrl`

---

**Last Updated:** 2025-12-27
