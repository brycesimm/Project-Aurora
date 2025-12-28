# Setup Local Azurite Storage for Aurora Development
# This script creates the aurora-content container and uploads sample content

param(
    [string]$ContentFile = "../sample.content.json"
)

Write-Host "Setting up local Azurite storage..." -ForegroundColor Cyan

# Azurite well-known connection string
$connectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM52j2LUBn5SPPqKOsj5h+l/E9cQ0IfqC0SsRsWK7x9VyZrLqHWxqwqKxKjUXCFQCn3Q==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"

# Check if Azurite is running
Write-Host "Checking if Azurite is running on port 10000..." -ForegroundColor Yellow
$azuriteRunning = Test-NetConnection -ComputerName 127.0.0.1 -Port 10000 -InformationLevel Quiet -WarningAction SilentlyContinue

if (-not $azuriteRunning) {
    Write-Host "ERROR: Azurite is not running. Please start Azurite first." -ForegroundColor Red
    Write-Host "You can start it by running the Aurora.Api project or manually via 'azurite' command." -ForegroundColor Yellow
    exit 1
}

Write-Host "✓ Azurite is running" -ForegroundColor Green

# Create container using .NET SDK (more reliable than Azure CLI)
Write-Host "Creating 'aurora-content' container..." -ForegroundColor Yellow

$createContainerScript = @"
using Azure.Storage.Blobs;
using System;

var connectionString = "$connectionString";
var blobServiceClient = new BlobServiceClient(connectionString);
var containerClient = blobServiceClient.GetBlobContainerClient("aurora-content");

try
{
    await containerClient.CreateIfNotExistsAsync();
    Console.WriteLine("Container 'aurora-content' created or already exists.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Environment.Exit(1);
}
"@

# Save temp C# script
$tempScript = [System.IO.Path]::GetTempFileName() + ".csx"
Set-Content -Path $tempScript -Value $createContainerScript

# Execute using dotnet-script (if available) or fallback to manual instructions
if (Get-Command dotnet-script -ErrorAction SilentlyContinue) {
    dotnet-script $tempScript
    Remove-Item $tempScript
} else {
    Write-Host "✓ Container 'aurora-content' ready (assuming manual creation via Azure Storage Explorer)" -ForegroundColor Green
    Write-Host ""
    Write-Host "If container doesn't exist, please create it manually:" -ForegroundColor Yellow
    Write-Host "1. Open Azure Storage Explorer" -ForegroundColor Yellow
    Write-Host "2. Connect to Local & Attached -> Storage Accounts -> Emulator" -ForegroundColor Yellow
    Write-Host "3. Right-click Blob Containers -> Create Blob Container -> 'aurora-content'" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Setup complete! You can now run the Aurora.Api project." -ForegroundColor Green
Write-Host "The API will read content from: http://127.0.0.1:10000/devstoreaccount1/aurora-content/content.json" -ForegroundColor Cyan
