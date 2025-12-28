# Aurora Content Management Tools

This directory contains PowerShell automation scripts for managing Aurora's daily content feed. These tools ensure content quality, streamline deployment, and reduce the risk of breaking changes during beta testing.

---

## 📋 Scripts

### 1. Validate-Content.ps1

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

### 2. Deploy-Content.ps1

Deploys validated content to Azure Blob Storage with automatic backup and rollback capability.

**Purpose:**
- Automates content deployment to Azure with validation pre-checks
- Creates automatic backups before each deployment
- Supports multiple environments (Dev/Production)
- Prevents accidental deployments with confirmation prompts

**Usage:**
```powershell
.\Deploy-Content.ps1 -FilePath <path-to-content.json> [-Environment <Dev|Production>] [-Force]
```

**Parameters:**
- `-FilePath` (required): Path to the content JSON file to deploy
- `-Environment` (optional): Target environment (`Dev` or `Production`). **Defaults to Dev for safety.**
- `-Force` (optional): Skip confirmation prompt (use for automation/CI)

**Examples:**
```powershell
# Deploy to Dev environment (default, safest)
.\Deploy-Content.ps1 -FilePath .\content.json

# Deploy to Production (requires confirmation)
.\Deploy-Content.ps1 -FilePath .\content.json -Environment Production

# Deploy to Production without prompt (CI/automation)
.\Deploy-Content.ps1 -FilePath .\content.json -Environment Production -Force
```

**Prerequisites:**
- **Azure CLI** installed (`winget install Microsoft.AzureCLI`)
- **Azure authentication:** Run `az login` once (credentials cached)
- **Blob Storage permissions:** Read/Write access to `aurora-content` container

**What the script does:**
1. **Validates content** using `Validate-Content.ps1` (aborts if validation fails)
2. **Downloads current blob** and saves as `content.backup.<Environment>.<timestamp>.json`
3. **Uploads new content** to Azure Blob Storage (overwrites existing)
4. **Maintains last 5 backups** per environment (auto-deletes older backups)

---

### 3. Rollback-Content.ps1

Restores a previous content backup to Azure Blob Storage.

**Purpose:**
- Quickly revert to a previous version if deployment causes issues
- Browse and select from available backups interactively
- Restore specific backup file directly

**Usage:**
```powershell
.\Rollback-Content.ps1 -Environment <Dev|Production> [-BackupFile <path>]
```

**Parameters:**
- `-Environment` (required): Target environment to rollback
- `-BackupFile` (optional): Specific backup file to restore. If omitted, shows interactive list.

**Examples:**
```powershell
# Interactive rollback (shows list of available backups)
.\Rollback-Content.ps1 -Environment Production

# Restore specific backup directly
.\Rollback-Content.ps1 -Environment Production -BackupFile .\backups\content.backup.Production.2025-12-28-143215.json
```

**Backup Location:**
All backups are stored in `tools/content-management/backups/` directory with naming pattern:
```
content.backup.<Environment>.<YYYY-MM-DD-HHMMSS>.json
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

## 🚀 Complete Content Management Workflow

### 1. Curate New Content

**Edit the content file** (recommend working on a copy of `sample.content.json`):
```powershell
# Copy template
cp ..\..\sample.content.json .\my-content.json

# Edit with your preferred editor
code .\my-content.json
```

**Follow the content guidelines:**
- 1 Vibe of the Day (featured story)
- 5-10 Daily Picks (additional stories)
- Valid HTTP/HTTPS URLs for articles and images
- 2-3 sentence snippets (50-200 characters recommended)
- Unique kebab-case IDs

---

### 2. Validate Content

**Run validation with image checks:**
```powershell
.\Validate-Content.ps1 -FilePath .\my-content.json -CheckImageUrls
```

**Interpret results:**
- ✅ **No errors?** → Proceed to deployment
- ⚠️ **Warnings only?** → Review and optionally fix (content is deployable)
- ❌ **Errors present?** → Fix all errors before deploying

---

### 3. Deploy to Azure

**Deploy to Dev first (recommended):**
```powershell
.\Deploy-Content.ps1 -FilePath .\my-content.json -Environment Dev
```

**Verify on test device:**
- Open Aurora app on emulator or physical device
- Check that content loads correctly
- Test all READ buttons open articles
- Verify images display properly

**Deploy to Production:**
```powershell
.\Deploy-Content.ps1 -FilePath .\my-content.json -Environment Production
```

**What happens during deployment:**
```
╔════════════════════════════════════════════════════════════╗
║         Aurora Content Deployment Tool - v1.0              ║
╚════════════════════════════════════════════════════════════╝

Checking prerequisites...
✓ Azure CLI detected (version 2.x.x)
✓ Authenticated as: user@example.com

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Step 1/4: Validating content...
✓ JSON syntax is valid
✓ Content validation passed

Ready to deploy to: Production
Continue with deployment? [Y/N]: Y

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Step 2/4: Creating backup of current content...
✓ Backup created: content.backup.Production.2025-12-28-143215.json

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Step 3/4: Uploading content to Azure Blob Storage...
✓ Content successfully uploaded to blob storage

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Step 4/4: Deployment complete

✓ Content deployed to Production at 2025-12-28 14:32:15
  Storage Account: staurora4tcguzr2zm32w
  Container: aurora-content
  Blob: content.json

Backup saved: content.backup.Production.2025-12-28-143215.json
To rollback: .\Rollback-Content.ps1 -Environment Production

╔════════════════════════════════════════════════════════════╗
║                  Deployment Successful!                    ║
╚════════════════════════════════════════════════════════════╝
```

---

### 4. Verify in Production

**Immediate verification:**
- Open Aurora app (force close and reopen to clear cache)
- Confirm new content appears
- Test article links and reactions
- Monitor Application Insights for errors (optional)

**If issues are detected:**
→ Proceed to rollback (Section 5)

---

### 5. Rollback (Emergency Procedure)

**If deployed content has critical issues:**

```powershell
# Interactive rollback (shows list of backups)
.\Rollback-Content.ps1 -Environment Production
```

**Interactive selection example:**
```
Available backups for Production environment:

  [1] content.backup.Production.2025-12-28-143215.json
      Created: 2025-12-28 14:32:15 | Size: 7.70 KB

  [2] content.backup.Production.2025-12-27-091030.json
      Created: 2025-12-27 09:10:30 | Size: 6.85 KB

  [3] content.backup.Production.2025-12-26-164500.json
      Created: 2025-12-26 16:45:00 | Size: 7.12 KB

  [0] Cancel rollback

Select backup to restore [0-3]: 1

✓ Selected backup: content.backup.Production.2025-12-28-143215.json

⚠ Warning: This will replace the current Production content with the selected backup.

Continue with rollback? [Y/N]: Y

✓ Content successfully restored from backup

Rollback Summary:
  Environment: Production
  Backup restored: content.backup.Production.2025-12-28-143215.json
  Rollback time: 2025-12-28 14:45:22

╔════════════════════════════════════════════════════════════╗
║                  Rollback Successful!                      ║
╚════════════════════════════════════════════════════════════╝
```

**Direct rollback (specify backup file):**
```powershell
.\Rollback-Content.ps1 -Environment Production -BackupFile .\backups\content.backup.Production.2025-12-28-143215.json
```

---

### Quick Reference Workflow

```
Edit → Validate → Deploy (Dev) → Test → Deploy (Prod) → Verify
                                                      ↓
                                                  Issues?
                                                      ↓
                                                  Rollback
```

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
- **Backlog:** `../../docs/planning/BACKLOG.md` (Stories V-0.5, V-0.6)
- **Azure Portal:** [Azure Functions](https://portal.azure.com) (for monitoring and troubleshooting)

---

## 🔐 Security Notes

**Credential Management:**
- Azure CLI credentials are stored securely by `az login`
- Never commit `.azure/` directory or credentials to source control
- Use Azure role-based access control (RBAC) for team environments

**Backup Security:**
- Backups contain published content only (no sensitive data)
- Stored locally in `backups/` directory (excluded from git via `.gitignore`)
- Maintain at least 2 recent backups before deleting old files

---

**Last Updated:** 2025-12-28
**Maintainer:** Project Aurora Team
**License:** Internal tooling for Project Aurora
