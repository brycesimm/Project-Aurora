<#
.SYNOPSIS
    Validates content.json against the Aurora content schema.

.DESCRIPTION
    This script validates a content JSON file to ensure it meets the requirements
    for Aurora's daily content feed. It checks JSON syntax, required fields,
    data types, URL formats, and optionally tests image URL accessibility.

.PARAMETER FilePath
    Path to the content.json file to validate.

.PARAMETER CheckImageUrls
    If specified, performs HTTP HEAD requests to verify image URLs are accessible.
    Warnings are generated for inaccessible URLs, but validation still passes.

.EXAMPLE
    .\Validate-Content.ps1 -FilePath .\content.json

.EXAMPLE
    .\Validate-Content.ps1 -FilePath .\content.json -CheckImageUrls

#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$FilePath,

    [Parameter(Mandatory = $false)]
    [switch]$CheckImageUrls
)

$ErrorActionPreference = 'Stop'

# Color constants for output
$ColorSuccess = 'Green'
$ColorError = 'Red'
$ColorWarning = 'Yellow'
$ColorInfo = 'Cyan'

function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor $ColorSuccess
}

function Write-ValidationError {
    param([string]$Message)
    Write-Host "✗ Error: $Message" -ForegroundColor $ColorError
}

function Write-ValidationWarning {
    param([string]$Message)
    Write-Host "⚠ Warning: $Message" -ForegroundColor $ColorWarning
}

function Write-InfoMessage {
    param([string]$Message)
    Write-Host $Message -ForegroundColor $ColorInfo
}

function Test-UrlFormat {
    param([string]$Url, [string]$FieldName, [string]$ItemContext)

    if ([string]::IsNullOrWhiteSpace($Url)) {
        Write-ValidationError "$ItemContext - '$FieldName' is empty or missing"
        return $false
    }

    if (-not ($Url -match '^https?://')) {
        Write-ValidationError "$ItemContext - '$FieldName' must start with http:// or https:// (got: '$Url')"
        return $false
    }

    try {
        $uri = [System.Uri]$Url
        if (-not $uri.IsAbsoluteUri) {
            Write-ValidationError "$ItemContext - '$FieldName' must be an absolute URL (got: '$Url')"
            return $false
        }
    }
    catch {
        Write-ValidationError "$ItemContext - '$FieldName' is not a valid URL (got: '$Url')"
        return $false
    }

    return $true
}

function Test-ContentItem {
    param(
        [PSCustomObject]$Item,
        [string]$Context,
        [int]$Index = -1
    )

    $contextLabel = if ($Index -ge 0) { "$Context[$Index]" } else { $Context }
    $isValid = $true

    # Required fields from schema
    $requiredFields = @('id', 'title', 'snippet', 'image_url', 'article_url', 'uplift_count')

    foreach ($field in $requiredFields) {
        if (-not $Item.PSObject.Properties.Name.Contains($field)) {
            Write-ValidationError "$contextLabel - Missing required field '$field'"
            $isValid = $false
        }
    }

    # If we're missing required fields, no point in further validation
    if (-not $isValid) {
        return $false
    }

    # Validate field types
    if ($Item.id -isnot [string] -or [string]::IsNullOrWhiteSpace($Item.id)) {
        Write-ValidationError "$contextLabel - 'id' must be a non-empty string"
        $isValid = $false
    }

    if ($Item.title -isnot [string] -or [string]::IsNullOrWhiteSpace($Item.title)) {
        Write-ValidationError "$contextLabel - 'title' must be a non-empty string"
        $isValid = $false
    }

    if ($Item.snippet -isnot [string] -or [string]::IsNullOrWhiteSpace($Item.snippet)) {
        Write-ValidationError "$contextLabel - 'snippet' must be a non-empty string"
        $isValid = $false
    }

    # Validate uplift_count
    if ($Item.uplift_count -isnot [int] -and $Item.uplift_count -isnot [long]) {
        Write-ValidationError "$contextLabel - 'uplift_count' must be an integer (got: $($Item.uplift_count))"
        $isValid = $false
    }
    elseif ($Item.uplift_count -lt 0) {
        Write-ValidationError "$contextLabel - 'uplift_count' must be non-negative (got: $($Item.uplift_count))"
        $isValid = $false
    }

    # Validate URLs
    if (-not (Test-UrlFormat -Url $Item.image_url -FieldName 'image_url' -ItemContext $contextLabel)) {
        $isValid = $false
    }

    if (-not (Test-UrlFormat -Url $Item.article_url -FieldName 'article_url' -ItemContext $contextLabel)) {
        $isValid = $false
    }

    # Optional: Quality warnings
    if ($Item.snippet.Length -lt 50) {
        Write-ValidationWarning "$contextLabel - 'snippet' is very short (<50 chars). Consider adding more context."
    }

    if ($Item.snippet.Length -gt 200) {
        Write-ValidationWarning "$contextLabel - 'snippet' is very long (>200 chars). Consider shortening for readability."
    }

    return $isValid
}

function Test-ImageUrlAccessibility {
    param([string]$Url, [string]$Title)

    try {
        $response = Invoke-WebRequest -Uri $Url -Method Head -TimeoutSec 5 -UseBasicParsing -ErrorAction Stop
        if ($response.StatusCode -eq 200) {
            return $true
        }
        else {
            Write-ValidationWarning "Image URL for '$Title' returned status code $($response.StatusCode)"
            return $false
        }
    }
    catch {
        Write-ValidationWarning "Image URL for '$Title' is not accessible: $($_.Exception.Message)"
        return $false
    }
}

# Main validation logic
Write-InfoMessage "Validating content file: $FilePath"
Write-Host ""

# Check file exists
if (-not (Test-Path $FilePath)) {
    Write-ValidationError "File not found: $FilePath"
    exit 1
}

# Read and parse JSON
try {
    $jsonContent = Get-Content -Path $FilePath -Raw
    $content = $jsonContent | ConvertFrom-Json -ErrorAction Stop
}
catch {
    Write-ValidationError "Failed to parse JSON: $($_.Exception.Message)"
    exit 1
}

Write-Success "JSON syntax is valid"

# Validate root structure
$validationPassed = $true

if (-not $content.PSObject.Properties.Name.Contains('VibeOfTheDay')) {
    Write-ValidationError "Missing required property 'VibeOfTheDay' at root level"
    $validationPassed = $false
}

if (-not $content.PSObject.Properties.Name.Contains('DailyPicks')) {
    Write-ValidationError "Missing required property 'DailyPicks' at root level"
    $validationPassed = $false
}

if (-not $validationPassed) {
    Write-Host ""
    Write-ValidationError "Content validation failed - root structure is invalid"
    exit 1
}

# Validate VibeOfTheDay
if (-not (Test-ContentItem -Item $content.VibeOfTheDay -Context 'VibeOfTheDay')) {
    $validationPassed = $false
}

# Validate DailyPicks is an array
if ($content.DailyPicks -isnot [Array]) {
    Write-ValidationError "DailyPicks must be an array"
    $validationPassed = $false
}
else {
    # Validate each daily pick
    for ($i = 0; $i -lt $content.DailyPicks.Count; $i++) {
        if (-not (Test-ContentItem -Item $content.DailyPicks[$i] -Context 'DailyPicks' -Index $i)) {
            $validationPassed = $false
        }
    }

    # Check count recommendation
    if ($content.DailyPicks.Count -lt 5) {
        Write-ValidationWarning "DailyPicks contains only $($content.DailyPicks.Count) items. Recommend 5-10 stories for variety."
    }
    elseif ($content.DailyPicks.Count -gt 10) {
        Write-ValidationWarning "DailyPicks contains $($content.DailyPicks.Count) items. Recommend 5-10 stories to avoid overwhelming users."
    }
}

# Optional image URL accessibility checks
if ($CheckImageUrls -and $validationPassed) {
    Write-Host ""
    Write-InfoMessage "Checking image URL accessibility (this may take a moment)..."

    Test-ImageUrlAccessibility -Url $content.VibeOfTheDay.image_url -Title $content.VibeOfTheDay.title | Out-Null

    foreach ($pick in $content.DailyPicks) {
        Test-ImageUrlAccessibility -Url $pick.image_url -Title $pick.title | Out-Null
    }
}

# Final result
Write-Host ""
if ($validationPassed) {
    Write-Success "content.json is valid and ready to upload"
    Write-InfoMessage "  - 1 Vibe of the Day"
    Write-InfoMessage "  - $($content.DailyPicks.Count) Daily Picks"
    exit 0
}
else {
    Write-ValidationError "Content validation failed - please fix the errors above"
    exit 1
}
