<#
.SYNOPSIS
    Rolls back Azure Blob Storage content to a previous backup.

.DESCRIPTION
    This script restores content.json from a backup file created by Deploy-Content.ps1.
    It lists available backups and allows selection of which version to restore.

.PARAMETER Environment
    Target environment (Dev or Production) to rollback.

.PARAMETER BackupFile
    Optional: Specific backup file to restore. If not provided, shows interactive list.

.EXAMPLE
    .\Rollback-Content.ps1 -Environment Production

.EXAMPLE
    .\Rollback-Content.ps1 -Environment Dev -BackupFile .\backups\content.backup.Dev.2025-12-28-143215.json

#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('Dev', 'Production')]
    [string]$Environment,

    [Parameter(Mandatory = $false)]
    [string]$BackupFile
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

function Write-RollbackError {
    param([string]$Message)
    Write-Host "✗ Error: $Message" -ForegroundColor $ColorError
}

function Write-RollbackWarning {
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
            return $true
        }
    }
    catch {
        Write-RollbackError "Azure CLI is not installed or not in PATH"
        Write-Host ""
        Write-Host "Please install Azure CLI from: https://aka.ms/installazurecliwindows" -ForegroundColor $ColorInfo
        return $false
    }
    return $false
}

function Test-AzureLogin {
    try {
        $account = az account show 2>$null | ConvertFrom-Json
        if ($account) {
            return $true
        }
    }
    catch {
        Write-RollbackWarning "Not logged in to Azure"
        Write-InfoMessage "Running 'az login' to authenticate..."
        az login | Out-Null

        $account = az account show 2>$null | ConvertFrom-Json
        if ($account) {
            return $true
        }
        else {
            Write-RollbackError "Azure authentication failed"
            return $false
        }
    }
    return $false
}

function Get-BackupFile {
    param([string]$EnvironmentName)

    $backupDir = Join-Path $PSScriptRoot 'backups'

    if (-not (Test-Path $backupDir)) {
        Write-RollbackError "Backups directory not found: $backupDir"
        return $null
    }

    # Find all backups for this environment
    $backups = Get-ChildItem -Path $backupDir -Filter "content.backup.$EnvironmentName.*.json" |
        Sort-Object LastWriteTime -Descending

    if ($backups.Count -eq 0) {
        Write-RollbackError "No backups found for $EnvironmentName environment"
        return $null
    }

    Write-InfoMessage "Available backups for $EnvironmentName environment:"
    Write-Host ""

    for ($i = 0; $i -lt $backups.Count; $i++) {
        $backup = $backups[$i]
        $timestamp = $backup.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss')
        $size = "{0:N2} KB" -f ($backup.Length / 1KB)

        Write-Host "  [$($i + 1)] $($backup.Name)" -ForegroundColor Cyan
        Write-Host "      Created: $timestamp | Size: $size" -ForegroundColor DarkGray
    }

    Write-Host ""
    Write-Host "  [0] Cancel rollback" -ForegroundColor Yellow
    Write-Host ""

    $selection = Read-Host "Select backup to restore [0-$($backups.Count)]"

    try {
        $index = [int]$selection
        if ($index -eq 0) {
            return $null
        }
        elseif ($index -ge 1 -and $index -le $backups.Count) {
            return $backups[$index - 1].FullName
        }
        else {
            Write-RollbackError "Invalid selection"
            return $null
        }
    }
    catch {
        Write-RollbackError "Invalid input"
        return $null
    }
}

function Restore-ContentFromBackup {
    param(
        [string]$BackupFilePath,
        [string]$StorageAccount,
        [string]$ContainerName,
        [string]$BlobName
    )

    Write-InfoMessage "Uploading backup to Azure Blob Storage..."
    Write-Host ""

    try {
        $uploadResult = az storage blob upload `
            --account-name $StorageAccount `
            --container-name $ContainerName `
            --name $BlobName `
            --file $BackupFilePath `
            --overwrite true `
            --auth-mode key `
            2>&1

        if ($LASTEXITCODE -eq 0) {
            return $true
        }
        else {
            Write-RollbackError "Upload failed: $uploadResult"
            return $false
        }
    }
    catch {
        Write-RollbackError "Upload exception: $($_.Exception.Message)"
        return $false
    }
}

# ═══════════════════════════════════════════════════════════════════════════
# Main Rollback Flow
# ═══════════════════════════════════════════════════════════════════════════

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Yellow
Write-Host "║         Aurora Content Rollback Tool - v1.0                   ║" -ForegroundColor Yellow
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Yellow
Write-Host ""

# Get environment configuration
$config = $EnvironmentConfig[$Environment]

Write-InfoMessage "Target environment: $Environment"
Write-InfoMessage "Storage Account: $($config.StorageAccount)"
Write-Host ""

# Prerequisite checks
if (-not (Test-AzureCli)) {
    exit 1
}

if (-not (Test-AzureLogin)) {
    exit 1
}

Write-Host ""
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host ""

# Determine which backup to restore
$backupPath = $null

if ($BackupFile) {
    # User provided specific backup file
    if (-not (Test-Path $BackupFile)) {
        Write-RollbackError "Backup file not found: $BackupFile"
        exit 1
    }
    $backupPath = Resolve-Path $BackupFile
}
else {
    # Interactive selection
    $backupPath = Get-BackupFile -EnvironmentName $Environment

    if (-not $backupPath) {
        Write-Host ""
        Write-RollbackWarning "Rollback cancelled"
        exit 0
    }
}

Write-Host ""
Write-Success "Selected backup: $(Split-Path $backupPath -Leaf)"
Write-Host ""

# Confirm rollback
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host ""
Write-RollbackWarning "This will replace the current $Environment content with the selected backup."
Write-Host ""
$response = Read-Host "Continue with rollback? [Y/N]"

if ($response -ne 'Y' -and $response -ne 'y') {
    Write-Host ""
    Write-RollbackWarning "Rollback cancelled by user"
    exit 0
}

Write-Host ""
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host ""

# Perform rollback
$restoreSuccess = Restore-ContentFromBackup `
    -BackupFilePath $backupPath `
    -StorageAccount $config.StorageAccount `
    -ContainerName $config.ContainerName `
    -BlobName $config.BlobName

if (-not $restoreSuccess) {
    Write-Host ""
    Write-RollbackError "Rollback failed - content restore unsuccessful"
    exit 1
}

# Success summary
$rollbackTime = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'

Write-Host ""
Write-Success "Content successfully restored from backup"
Write-Host ""
Write-InfoMessage "Rollback Summary:"
Write-InfoMessage "  Environment: $Environment"
Write-InfoMessage "  Backup restored: $(Split-Path $backupPath -Leaf)"
Write-InfoMessage "  Rollback time: $rollbackTime"
Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║                  Rollback Successful!                          ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""

exit 0
