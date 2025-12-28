<#
.SYNOPSIS
    Generates a content.json template for Aurora's daily content feed.

.DESCRIPTION
    This script creates a properly formatted content.json template file with placeholder
    values, making it easy to start curating new content without manual JSON editing.

    The generated template includes all required fields with helpful placeholders:
    - Auto-generated unique IDs (vibe-of-the-day-N, daily-pick-N)
    - Today's date auto-filled for published_date
    - Clear TODO markers for fields requiring manual input
    - Valid JSON structure ready for editing

.PARAMETER VibeCount
    Number of "Vibe of the Day" entries to generate. Defaults to 1.

.PARAMETER PicksCount
    Number of "Daily Picks" entries to generate. Defaults to 7.

.PARAMETER OutputFile
    Path to the output template file. Defaults to 'content-template.json' in current directory.

.EXAMPLE
    .\New-ContentTemplate.ps1

    Generates a template with 1 Vibe and 7 Daily Picks to content-template.json

.EXAMPLE
    .\New-ContentTemplate.ps1 -PicksCount 10

    Generates a template with 1 Vibe and 10 Daily Picks

.EXAMPLE
    .\New-ContentTemplate.ps1 -VibeCount 1 -PicksCount 5 -OutputFile .\my-content.json

    Generates a template with custom output filename

#>

[CmdletBinding()]
param(
	[Parameter(Mandatory = $false)]
	[ValidateRange(1, 5)]
	[int]$VibeCount = 1,

	[Parameter(Mandatory = $false)]
	[ValidateRange(1, 20)]
	[int]$PicksCount = 7,

	[Parameter(Mandatory = $false)]
	[string]$OutputFile = 'content-template.json'
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

function Write-TemplateError {
	param([string]$Message)
	Write-Host "✗ Error: $Message" -ForegroundColor $ColorError
}

function Write-TemplateWarning {
	param([string]$Message)
	Write-Host "⚠ Warning: $Message" -ForegroundColor $ColorWarning
}

function Write-InfoMessage {
	param([string]$Message)
	Write-Host $Message -ForegroundColor $ColorInfo
}

function New-ContentItem {
	param(
		[string]$IdPrefix,
		[int]$Index
	)

	$today = Get-Date -Format 'yyyy-MM-dd'
	$id = "$IdPrefix-$Index"

	return [PSCustomObject]@{
		id            = "REPLACE_WITH_UNIQUE_ID_$Index"
		title         = "TODO: Article Title Here"
		snippet       = "TODO: Write a 2-3 sentence summary that explains the uplifting news in a clear, engaging way. Focus on the positive impact and specific outcomes."
		image_url     = "https://via.placeholder.com/800x600/7986CB/FFFFFF?text=Aurora+Placeholder"
		article_url   = "https://example.com/article-$Index"
		published_date = $today
		uplift_count  = 0
	}
}

# ═══════════════════════════════════════════════════════════════════════════
# Main Template Generation
# ═══════════════════════════════════════════════════════════════════════════

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║       Aurora Content Template Generator - v1.0                 ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

Write-InfoMessage "Configuration:"
Write-InfoMessage "  Vibe of the Day entries: $VibeCount"
Write-InfoMessage "  Daily Picks entries: $PicksCount"
Write-InfoMessage "  Output file: $OutputFile"
Write-Host ""

# Check if output file already exists
if (Test-Path $OutputFile) {
	Write-TemplateWarning "File already exists: $OutputFile"
	$response = Read-Host "Overwrite existing file? [Y/N]"

	if ($response -ne 'Y' -and $response -ne 'y') {
		Write-Host ""
		Write-InfoMessage "Template generation cancelled by user"
		exit 0
	}
	Write-Host ""
}

Write-InfoMessage "Generating template structure..."
Write-Host ""

# Generate Vibe of the Day
$vibeOfTheDay = if ($VibeCount -eq 1) {
	New-ContentItem -IdPrefix "vibe-of-the-day" -Index 1
}
else {
	Write-TemplateWarning "Multiple Vibes requested, but only 1 is supported in schema. Using first entry only."
	New-ContentItem -IdPrefix "vibe-of-the-day" -Index 1
}

# Generate Daily Picks
$dailyPicks = @()
for ($i = 1; $i -le $PicksCount; $i++) {
	$dailyPicks += New-ContentItem -IdPrefix "daily-pick" -Index $i
}

# Create final structure
$content = [PSCustomObject]@{
	VibeOfTheDay = $vibeOfTheDay
	DailyPicks   = $dailyPicks
}

# Convert to JSON with pretty formatting
try {
	$jsonContent = $content | ConvertTo-Json -Depth 10

	# Write to file
	Set-Content -Path $OutputFile -Value $jsonContent -Encoding UTF8

	Write-Success "Template file created successfully"
	Write-Host ""
}
catch {
	Write-TemplateError "Failed to write template file: $($_.Exception.Message)"
	exit 1
}

# Display summary
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host ""
Write-InfoMessage "Template Summary:"
Write-InfoMessage "  ✓ 1 Vibe of the Day (featured story)"
Write-InfoMessage "  ✓ $PicksCount Daily Picks"
Write-InfoMessage "  ✓ Auto-filled today's date: $(Get-Date -Format 'yyyy-MM-dd')"
Write-InfoMessage "  ✓ Placeholder values ready for editing"
Write-Host ""
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host ""

# Provide next steps guidance
Write-InfoMessage "Next Steps:"
Write-Host ""
Write-Host "  1. Edit the template file:" -ForegroundColor White
Write-Host "     code $OutputFile" -ForegroundColor Gray
Write-Host ""
Write-Host "  2. Replace all placeholder values:" -ForegroundColor White
Write-Host "     - Update 'id' with unique kebab-case identifiers" -ForegroundColor Gray
Write-Host "     - Replace 'TODO: Article Title Here' with actual titles" -ForegroundColor Gray
Write-Host "     - Write engaging 2-3 sentence snippets" -ForegroundColor Gray
Write-Host "     - Set real article URLs from credible sources" -ForegroundColor Gray
Write-Host "     - Update image URLs (or keep placeholder)" -ForegroundColor Gray
Write-Host ""
Write-Host "  3. Validate the content:" -ForegroundColor White
Write-Host "     .\Validate-Content.ps1 -FilePath $OutputFile -CheckImageUrls" -ForegroundColor Gray
Write-Host ""
Write-Host "  4. Deploy to Dev environment:" -ForegroundColor White
Write-Host "     .\Deploy-Content.ps1 -FilePath $OutputFile -Environment Dev" -ForegroundColor Gray
Write-Host ""
Write-Host "  5. After testing, deploy to Production:" -ForegroundColor White
Write-Host "     .\Deploy-Content.ps1 -FilePath $OutputFile -Environment Production" -ForegroundColor Gray
Write-Host ""

Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║              Template Generation Successful!                   ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""

# Suggest content curation resources
Write-InfoMessage "Content Curation Resources:"
Write-InfoMessage "  • r/UpliftingNews - https://reddit.com/r/UpliftingNews"
Write-InfoMessage "  • Good News Network - https://goodnewsnetwork.org"
Write-InfoMessage "  • Positive News - https://positive.news"
Write-InfoMessage "  • The Optimist Daily - https://optimistdaily.com"
Write-Host ""

exit 0
