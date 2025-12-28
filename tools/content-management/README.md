# Aurora Content Management Tools

This directory contains PowerShell automation scripts for managing Aurora's daily content feed. These tools ensure content quality, streamline deployment, and reduce the risk of breaking changes during beta testing.

---

## 📋 Scripts

### Validate-Content.ps1

Validates a `content.json` file against Aurora's schema requirements before deployment.

**Purpose:**
- Prevents deployment of malformed JSON that would break the application
- Ensures all required fields are present and correctly typed
- Validates URL formats for articles and images
- Optionally checks image URL accessibility

**Usage:**
```powershell
.\Validate-Content.ps1 -FilePath <path-to-content.json> [-CheckImageUrls]
```

**Parameters:**
- `-FilePath` (required): Path to the content JSON file to validate
- `-CheckImageUrls` (optional): Performs HTTP HEAD requests to verify image URLs are accessible. Warnings are generated for inaccessible URLs, but validation still passes.

**Examples:**
```powershell
# Basic validation
.\Validate-Content.ps1 -FilePath ..\..\sample.content.json

# Validation with image URL accessibility checks
.\Validate-Content.ps1 -FilePath .\my-content.json -CheckImageUrls
```

---

## ✅ Validation Rules

### Root Structure
- Must contain `VibeOfTheDay` property (single content item)
- Must contain `DailyPicks` property (array of content items)

### Content Item Schema
Each content item (Vibe of the Day and Daily Picks) must have:

| Field          | Type    | Validation Rules                                              |
|----------------|---------|---------------------------------------------------------------|
| `id`           | string  | Required, non-empty, unique identifier (kebab-case recommended) |
| `title`        | string  | Required, non-empty                                           |
| `snippet`      | string  | Required, non-empty, 2-3 sentences recommended (50-200 chars) |
| `image_url`    | string  | Required, valid HTTP/HTTPS URL                                |
| `article_url`  | string  | Required, valid HTTP/HTTPS URL                                |
| `uplift_count` | integer | Required, non-negative integer (typically 0 for new content)  |

### Quality Recommendations
- **Snippet length:** 50-200 characters for optimal readability
- **Daily Picks count:** 5-10 stories (warnings for <5 or >10)
- **Image URLs:** Should be accessible (verified with `-CheckImageUrls`)

---

## 📊 Result Interpretation

### Success Output
```
✓ JSON syntax is valid
✓ content.json is valid and ready to upload
  - 1 Vibe of the Day
  - 7 Daily Picks
```

**Exit Code:** `0`

This indicates the file is ready for deployment.

---

### Error Output
```
✓ JSON syntax is valid
✗ Error: DailyPicks[2] - Missing required field 'title'
✗ Error: DailyPicks[4] - 'article_url' must start with http:// or https:// (got: 'not-a-url')
✗ Error: VibeOfTheDay - 'uplift_count' must be non-negative (got: -5)

✗ Error: Content validation failed - please fix the errors above
```

**Exit Code:** `1`

Errors indicate critical issues that **must be fixed** before deployment. The application will fail to load content with these errors.

**Common Errors:**
- **Missing required field:** Add the missing property to the content item
- **Invalid URL format:** Ensure URLs start with `http://` or `https://`
- **Negative uplift_count:** Change to `0` or a positive integer
- **Wrong data type:** Ensure `uplift_count` is an integer, strings are quoted, etc.

---

### Warning Output
```
✓ JSON syntax is valid
⚠ Warning: DailyPicks[1] - 'snippet' is very short (<50 chars). Consider adding more context.
⚠ Warning: Image URL for 'Coral Reef Restoration' is not accessible: The remote server returned an error: (404) Not Found.
⚠ Warning: DailyPicks contains only 3 items. Recommend 5-10 stories for variety.

✓ content.json is valid and ready to upload
  - 1 Vibe of the Day
  - 3 Daily Picks
```

**Exit Code:** `0`

Warnings indicate **non-critical issues** that won't break the application but may affect quality or user experience. You can proceed with deployment, but consider addressing warnings before publishing.

**Common Warnings:**
- **Short snippet:** Add more context (recommended 2-3 sentences)
- **Long snippet:** Shorten for mobile readability
- **Inaccessible image URL:** Image may be broken or require authentication; consider using a different source
- **Too few/many Daily Picks:** Adjust count for optimal user experience (5-10 recommended)

---

## 🔧 Dependencies

### Required
- **PowerShell 7.0+** (Windows built-in, or install via `winget install Microsoft.PowerShell`)
- **Internet connection** (for `-CheckImageUrls` option only)

### No External Packages
This script uses only PowerShell built-in cmdlets:
- `Get-Content` - Read JSON file
- `ConvertFrom-Json` - Parse JSON
- `Invoke-WebRequest` - Test image URLs (optional)
- `Write-Host` - Colored output

---

## 🚀 Workflow Integration

### Recommended Content Update Workflow

1. **Edit** content file (e.g., `content.json`)
2. **Validate** with this script:
   ```powershell
   .\Validate-Content.ps1 -FilePath .\content.json -CheckImageUrls
   ```
3. **Fix** any errors or warnings
4. **Deploy** to Azure Blob Storage (using `Deploy-Content.ps1` - coming soon)
5. **Verify** in the Aurora app on a test device

---

## 📝 Content Curation Tips

### Finding Uplifting News Sources
- **r/UpliftingNews** (Reddit) - Community-curated positive news
- **Good News Network** (goodnewsnetwork.org) - Daily uplifting stories
- **Positive News** (positive.news) - Constructive journalism
- **The Optimist Daily** - Solutions-focused reporting
- **BBC Future** - Science and innovation breakthroughs

### Snippet Writing Best Practices
- **2-3 sentences:** Brief but informative
- **Lead with impact:** Start with the most uplifting aspect
- **Avoid jargon:** Use accessible language (PG-13+ audience)
- **Include specifics:** Numbers, names, outcomes make stories tangible
- **End with hope:** Leave the reader feeling inspired

### Image Selection
- **Use Open Graph images:** Most news sites provide `og:image` meta tags
- **Prefer landscape orientation:** 16:9 or 4:3 aspect ratios
- **High resolution:** 800x600 minimum (will be downscaled for mobile)
- **Relevant visuals:** Match the story subject
- **Fallback to placeholder:** Use `https://via.placeholder.com/800x600/7986CB/FFFFFF?text=Aurora` if no suitable image

### ID Naming Convention
Use kebab-case identifiers that are:
- **Descriptive:** `green-turtles-conservation-2025` vs `story-1`
- **Unique:** Include year/month to avoid collisions
- **URL-safe:** Only lowercase letters, numbers, and hyphens

---

## 🐛 Troubleshooting

### "File not found" error
**Cause:** Path is incorrect or file doesn't exist
**Solution:** Use absolute path or verify relative path from script directory

### "Failed to parse JSON" error
**Cause:** Syntax error in JSON (missing comma, extra quote, etc.)
**Solution:** Use a JSON validator (jsonlint.com) or VS Code's JSON validation

### Image URL check hangs
**Cause:** Server is slow or unresponsive
**Solution:** Script has 5-second timeout; check internet connection or skip `-CheckImageUrls`

### "Execution policy" error
**Cause:** PowerShell execution policy restricts script execution
**Solution:** Run `Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned`

---

## 📚 Related Documentation

- **Content Schema:** `../../content.schema.json`
- **Sample Content:** `../../sample.content.json`
- **Architecture:** `../../docs/technical/ARCHITECTURE.md`
- **Backlog:** `../../docs/planning/BACKLOG.md` (Story V-0.5)

---

**Last Updated:** 2025-12-28
**Maintainer:** Project Aurora Team
**License:** Internal tooling for Project Aurora
