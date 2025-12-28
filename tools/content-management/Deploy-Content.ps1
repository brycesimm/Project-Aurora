<#
.SYNOPSIS
    Deploys content.json to Azure Blob Storage with validation and rollback capability.

.DESCRIPTION
    This script automates the deployment of Aurora's daily content feed to Azure Blob Storage.
    It performs validation, creates backups, and supports rollback to previous versions.

    The script:
    - Validates content using Validate-Content.ps1
    - Downloads and backs up current production content
    - Uploads new content to Azure Blob Storage
    - Maintains last 5 backups for rollback capability

.PARAMETER FilePath
    Path to the content.json file to deploy.

.PARAMETER Environment
    Target environment (Dev or Production). Defaults to Dev for safety.

.PARAMETER Force
    Skip validation warnings confirmation prompt.

.EXAMPLE
    .\Deploy-Content.ps1 -FilePath .\content.json -Environment Dev

.EXAMPLE
    .\Deploy-Content.ps1 -FilePath .\content.json -Environment Production

.EXAMPLE
    .\Deploy-Content.ps1 -FilePath .\content.json -Environment Production -Force

#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$FilePath,

    [Parameter(Mandatory = $false)]
    [ValidateSet('Dev', 'Production')]
    [string]$Environment = 'Dev',

    [Parameter(Mandatory = $false)]
    [switch]$Force
)

$ErrorActionPreference = 'Stop'

# Color constants for output
$ColorSuccess = 'Green'
$ColorError = 'Red'
$ColorWarning = 'Yellow'
$ColorInfo = 'Cyan'

# Environment configuration
$EnvironmentConfig = @{
    'Dev' = @{
        ResourceGroup = 'rg-aurora-beta'
        StorageAccount = 'staurora4tcguzr2zm32w'
        ContainerName = 'aurora-content'
        BlobName = 'content.json'
    }
    'Production' = @{
        ResourceGroup = 'rg-aurora-beta'
        StorageAccount = 'staurora4tcguzr2zm32w'
        ContainerName = 'aurora-content'
        BlobName = 'content.json'
    }
}

function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor $ColorSuccess
}

function Write-DeploymentError {
    param([string]$Message)
    Write-Host "✗ Error: $Message" -ForegroundColor $ColorError
}

function Write-DeploymentWarning {
    param([string]$Message)
    Write-Host "⚠ Warning: $Message" -ForegroundColor $ColorWarning
}

function Write-InfoMessage {
    param([string]$Message)
    Write-Host $Message -ForegroundColor $ColorInfo
}

function Test-AzureCli {
    try {
        $azVersion = az version --output json 2>$null | ConvertFrom-Json
        if ($azVersion) {
            Write-Success "Azure CLI detected (version $($azVersion.'azure-cli'))"
            return $true
        }
    }
    catch {
        Write-DeploymentError "Azure CLI is not installed or not in PATH"
        Write-Host ""
        Write-Host "Please install Azure CLI from: https://aka.ms/installazurecliwindows" -ForegroundColor $ColorInfo
        Write-Host "Or via winget: winget install -e --id Microsoft.AzureCLI" -ForegroundColor $ColorInfo
        return $false
    }
    return $false
}

function Test-AzureLogin {
    try {
        $account = az account show 2>$null | ConvertFrom-Json
        if ($account) {
            Write-Success "Authenticated as: $($account.user.name)"
            return $true
        }
    }
    catch {
        Write-DeploymentWarning "Not logged in to Azure"
        Write-InfoMessage "Running 'az login' to authenticate..."
        az login | Out-Null

        $account = az account show 2>$null | ConvertFrom-Json
        if ($account) {
            Write-Success "Successfully authenticated as: $($account.user.name)"
            return $true
        }
        else {
            Write-DeploymentError "Azure authentication failed"
            return $false
        }
    }
    return $false
}

function Invoke-ContentValidation {
    param([string]$ContentPath)

    Write-InfoMessage "Step 1/4: Validating content..."
    Write-Host ""

    $validationScript = Join-Path $PSScriptRoot 'Validate-Content.ps1'

    if (-not (Test-Path $validationScript)) {
        Write-DeploymentError "Validation script not found: $validationScript"
        return $false
    }

    $validationResult = & $validationScript -FilePath $ContentPath
    $validationExitCode = $LASTEXITCODE

    Write-Host ""

    if ($validationExitCode -eq 0) {
        Write-Success "Content validation passed"
        return $true
    }
    else {
        Write-DeploymentError "Content validation failed - deployment aborted"
        return $false
    }
}

function New-ContentBackup {
    param(
        [string]$ResourceGroup,
        [string]$StorageAccount,
        [string]$ContainerName,
        [string]$BlobName,
        [string]$EnvironmentName
    )

    Write-InfoMessage "Step 2/4: Creating backup of current content..."
    Write-Host ""

    # Create backups directory if it doesn't exist
    $backupDir = Join-Path $PSScriptRoot 'backups'
    if (-not (Test-Path $backupDir)) {
        New-Item -ItemType Directory -Path $backupDir | Out-Null
        Write-InfoMessage "Created backups directory: $backupDir"
    }

    # Generate backup filename with timestamp
    $timestamp = Get-Date -Format 'yyyy-MM-dd-HHmmss'
    $backupFileName = "content.backup.$EnvironmentName.$timestamp.json"
    $backupPath = Join-Path $backupDir $backupFileName

    # Download current blob content
    try {
        $downloadResult = az storage blob download `
            --account-name $StorageAccount `
            --container-name $ContainerName `
            --name $BlobName `
            --file $backupPath `
            --auth-mode key `
            2>&1

        if ($LASTEXITCODE -eq 0) {
            Write-Success "Backup created: $backupFileName"

            # Clean up old backups (keep last 5 per environment)
            $existingBackups = Get-ChildItem -Path $backupDir -Filter "content.backup.$EnvironmentName.*.json" |
                Sort-Object LastWriteTime -Descending

            if ($existingBackups.Count -gt 5) {
                $backupsToDelete = $existingBackups | Select-Object -Skip 5
                foreach ($backup in $backupsToDelete) {
                    Remove-Item $backup.FullName -Force
                    Write-InfoMessage "  Removed old backup: $($backup.Name)"
                }
            }

            return $backupPath
        }
        else {
            Write-DeploymentWarning "Failed to create backup - current blob may not exist (first deployment?)"
            Write-InfoMessage "Proceeding with deployment without backup..."
            return $null
        }
    }
    catch {
        Write-DeploymentWarning "Backup failed: $($_.Exception.Message)"
        Write-InfoMessage "Proceeding with deployment without backup..."
        return $null
    }
}

function Publish-ContentToAzure {
    param(
        [string]$FilePath,
        [string]$ResourceGroup,
        [string]$StorageAccount,
        [string]$ContainerName,
        [string]$BlobName
    )

    Write-InfoMessage "Step 3/4: Uploading content to Azure Blob Storage..."
    Write-Host ""

    try {
        $uploadResult = az storage blob upload `
            --account-name $StorageAccount `
            --container-name $ContainerName `
            --name $BlobName `
            --file $FilePath `
            --overwrite true `
            --auth-mode key `
            2>&1

        if ($LASTEXITCODE -eq 0) {
            return $true
        }
        else {
            Write-DeploymentError "Upload failed: $uploadResult"
            return $false
        }
    }
    catch {
        Write-DeploymentError "Upload exception: $($_.Exception.Message)"
        return $false
    }
}

function Confirm-Deployment {
    param([string]$EnvironmentName)

    if ($Force) {
        return $true
    }

    Write-Host ""
    Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
    Write-Host "  Ready to deploy to: " -NoNewline -ForegroundColor $ColorInfo
    Write-Host $EnvironmentName -ForegroundColor Yellow
    Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
    Write-Host ""
    $response = Read-Host "Continue with deployment? [Y/N]"

    return ($response -eq 'Y' -or $response -eq 'y')
}

# ═══════════════════════════════════════════════════════════════════════════
# Main Deployment Flow
# ═══════════════════════════════════════════════════════════════════════════

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║         Aurora Content Deployment Tool - v1.0                 ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# Validate file exists
if (-not (Test-Path $FilePath)) {
    Write-DeploymentError "Content file not found: $FilePath"
    exit 1
}

$resolvedPath = Resolve-Path $FilePath
Write-InfoMessage "Source file: $resolvedPath"
Write-InfoMessage "Target environment: $Environment"
Write-Host ""

# Get environment configuration
$config = $EnvironmentConfig[$Environment]

# Prerequisite checks
Write-InfoMessage "Checking prerequisites..."
Write-Host ""

if (-not (Test-AzureCli)) {
    exit 1
}

if (-not (Test-AzureLogin)) {
    exit 1
}

Write-Host ""
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host ""

# Step 1: Validate content
if (-not (Invoke-ContentValidation -ContentPath $resolvedPath)) {
    exit 1
}

# Confirm deployment
if (-not (Confirm-Deployment -EnvironmentName $Environment)) {
    Write-Host ""
    Write-DeploymentWarning "Deployment cancelled by user"
    exit 0
}

Write-Host ""
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host ""

# Step 2: Create backup
$backupPath = New-ContentBackup `
    -ResourceGroup $config.ResourceGroup `
    -StorageAccount $config.StorageAccount `
    -ContainerName $config.ContainerName `
    -BlobName $config.BlobName `
    -EnvironmentName $Environment

Write-Host ""
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host ""

# Step 3: Upload to Azure
$uploadSuccess = Publish-ContentToAzure `
    -FilePath $resolvedPath `
    -ResourceGroup $config.ResourceGroup `
    -StorageAccount $config.StorageAccount `
    -ContainerName $config.ContainerName `
    -BlobName $config.BlobName

if (-not $uploadSuccess) {
    Write-Host ""
    Write-DeploymentError "Deployment failed - content upload unsuccessful"

    if ($backupPath) {
        Write-InfoMessage "Previous content backup available at: $backupPath"
        Write-InfoMessage "Use Rollback-Content.ps1 to restore if needed"
    }

    exit 1
}

Write-Success "Content successfully uploaded to blob storage"
Write-Host ""
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host ""

# Step 4: Deployment summary
$deploymentTime = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
Write-InfoMessage "Step 4/4: Deployment complete"
Write-Host ""
Write-Success "Content deployed to $Environment at $deploymentTime"
Write-InfoMessage "  Storage Account: $($config.StorageAccount)"
Write-InfoMessage "  Container: $($config.ContainerName)"
Write-InfoMessage "  Blob: $($config.BlobName)"

if ($backupPath) {
    Write-Host ""
    Write-InfoMessage "Backup saved: $(Split-Path $backupPath -Leaf)"
    Write-InfoMessage "To rollback: .\Rollback-Content.ps1 -Environment $Environment"
}

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║                  Deployment Successful!                        ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""

exit 0
