# Aurora Deployment Guide

**Last Updated:** 2025-12-31

This document provides step-by-step procedures for deploying Aurora to Google Play Internal Testing and managing Azure-hosted content.

---

## Android AAB Signing & Release

### Prerequisites

Before building a signed AAB for Google Play, ensure you have:

1. **Android Release Keystore**
   - Location: `C:\Programming\Keystores\aurora-release.keystore`
   - Alias: `aurora`
   - Validity: 10,000 days (~27 years)
   - Algorithm: RSA 2048-bit
   - SHA-256 Fingerprint: `EA:41:5B:D4:AC:73:D0:BE:A0:E5:F5:52:24:27:63:59:69:25:8A:1E:72:76:7D:E4:BC:3A:55:42:0B:DE:B6:D5`

2. **Keystore Password**
   - Stored in: Bitwarden (Secure Note: "Aurora Release Keystore")
   - Backed up to: 3 secure locations (Bitwarden attachment + cloud storage + external USB)

3. **Microsoft OpenJDK 17+**
   - Required for `keytool` (keystore management)
   - Install: `winget install Microsoft.OpenJDK.17`
   - Verify: `keytool -version`

4. **.NET 9 SDK**
   - Required for MAUI builds
   - Verify: `dotnet --version` (should show 9.0.x)

---

### Build Signed AAB

Follow these steps to generate a signed Android App Bundle for Google Play Console upload:

#### 1. Set Environment Variable (Keystore Password)

**PowerShell:**
```powershell
$env:AURORA_KEYSTORE_PASSWORD = "your-password-here"
```

**Important:**
- Replace `your-password-here` with the actual keystore password from Bitwarden
- This password is NEVER committed to Git (stored only in memory for the build session)
- Close PowerShell window when done to clear the environment variable

#### 2. Clean Previous Build Artifacts (Optional but Recommended)

```powershell
Remove-Item -Recurse -Force "C:\Programming\Project-Graveyard\Project-Aurora\src\Aurora\bin"
Remove-Item -Recurse -Force "C:\Programming\Project-Graveyard\Project-Aurora\src\Aurora\obj"
```

This ensures a clean build with no cached references to old package names or configurations.

#### 3. Build Release AAB

```powershell
dotnet publish "C:\Programming\Project-Graveyard\Project-Aurora\src\Aurora\Aurora.csproj" -c Release -f net9.0-android
```

**Expected Output:**
- Build completes with no errors
- AAB created at: `src\Aurora\bin\Release\net9.0-android\publish\com.metanoiasociety.aurora-Signed.aab`
- Typical size: ~30 MB

#### 4. Verify AAB Properties

Check the generated AAB file:

```powershell
# Verify file exists and check size
Get-Item "C:\Programming\Project-Graveyard\Project-Aurora\src\Aurora\bin\Release\net9.0-android\publish\com.metanoiasociety.aurora-Signed.aab"
```

**Expected Properties:**
- File name: `com.metanoiasociety.aurora-Signed.aab`
- Size: ~30 MB (acceptable range: 25-35 MB)
- Package name: `com.metanoiasociety.aurora` (permanently locked after first upload)

---

### Upload to Google Play Console

#### 1. Navigate to Google Play Console

1. Open: https://play.google.com/console
2. Sign in with: `themetanoiasociety@gmail.com`
3. Select app: **"Aurora - Uplifting Media"**

#### 2. Create Internal Testing Release

1. Left sidebar: **Testing** → **Internal testing**
2. Click **"Create new release"** button (or "New release" if releases exist)

#### 3. Upload AAB

1. Click **"Upload"** button in the "App bundles" section
2. Select: `C:\Programming\Project-Graveyard\Project-Aurora\src\Aurora\bin\Release\net9.0-android\publish\com.metanoiasociety.aurora-Signed.aab`
3. Wait for upload validation (Google verifies signature, checks for issues)
4. Review AAB details:
   - Package name: `com.metanoiasociety.aurora`
   - Supported devices: ~12,929 Android devices
   - Target SDK: 35 (Android 15)
   - API levels: 21+ (Android 5.0+)

**Common Upload Errors:**
- **"Version code conflict"**: Increment `<ApplicationVersion>` in `Aurora.csproj` (e.g., `1` → `2`)
- **"Signature mismatch"**: Keystore password incorrect or wrong keystore file
- **"Missing permissions"**: Check `AndroidManifest.xml` for required permissions

#### 4. Configure Release

1. **Release name**: Use format `X (Version)` (e.g., "2 (1.0.1)")
   - Google auto-increments the release number
   - Keep it simple and sequential

2. **Release notes**: Add user-facing changes (optional for internal testing, required for production)
   - Example:
     ```
     Bug fixes and performance improvements.

     Features:
     - Daily Picks: Curated uplifting news stories
     - Vibe of the Day: Morning inspiration
     - Reactions: Express sentiment with emojis
     - Share: Send stories to friends
     ```

3. **Click "Next"** to proceed to review

#### 5. Review Warnings (if any)

**Expected Warnings for Internal Testing:**
- **No deobfuscation file**: Safe to ignore for beta (enables easier debugging)
- **No debug symbols**: Safe to ignore for small-scale testing (useful for crash reports in production)
- **No testers configured**: Add testers in next step

**Blocking Errors:**
- **No testers**: Must add at least one email to Internal Testing list

#### 6. Configure Testers

1. On review screen, click **"Manage testers"** or navigate to **Testers** tab
2. Add tester email addresses (comma-separated)
   - **Important**: Emails must match the Google account signed into Google Play Store on the test device
   - Example: `user@gmail.com, tester2@gmail.com`
3. Click **"Save"**

#### 7. Start Rollout

1. Review all settings on the release summary screen
2. Click **"Start rollout to Internal testing"** (or "Review release" → "Start rollout")
3. Confirm the rollout

**Propagation Time:**
- Release becomes available to testers within ~5 minutes
- Google's automated security scan (Play Protect) may take 10-30 minutes for first-time installs

#### 8. Share Opt-in URL

1. Navigate to **Testing** → **Internal testing**
2. Find **"Testers"** section
3. Copy the **opt-in URL** (format: `https://play.google.com/apps/internaltest/XXXX`)
4. Email the opt-in URL to testers:
   - Subject: "Aurora Beta Testing - Join Internal Testing"
   - Body: Include opt-in link + brief instructions

**Note:** Opt-in URL is private and should NOT be committed to Git or shared publicly. Testers must be added to the email whitelist before they can access the link.

---

## Store Listing Configuration

For internal testing, store listing is **optional** but recommended for a polished tester experience.

### Required Fields

Navigate to **Grow Users** → **Store presence** → **Main store listing**:

1. **App name**: "Aurora - Uplifting Media"
2. **Short description** (80 characters max):
   ```
   Daily dose of uplifting news and inspiring stories to brighten your day
   ```

3. **Full description** (4,000 characters max):
   ```
   Aurora brings you a daily dose of uplifting news in a world saturated with negativity. Start each day with stories that inspire hope, celebrate human progress, and remind you of the good happening around the world.

   FEATURES

   Vibe of the Day
   Begin your morning with an inspiring quote or reflection designed to set a positive tone for your day.

   Daily Picks
   Curated uplifting news stories from credible sources—environmental wins, scientific breakthroughs, acts of kindness, and moments of human triumph. No clickbait, no fear-mongering, just genuinely positive media.

   React & Share
   Express how stories make you feel with reaction emojis, and share uplifting content with friends and family.

   WHY AURORA?

   Traditional news cycles thrive on division and fear. Aurora offers an antidote: a carefully curated feed of stories that prove humanity is making progress, one headline at a time. Whether it's a community coming together, a medical breakthrough, or an environmental victory—Aurora surfaces the news that restores your faith in the world.

   BETA TESTING

   Aurora is currently in early beta testing. We're refining the experience with feedback from real users like you. Expect regular updates, fresh content, and continuous improvements as we work toward our vision of making positive news accessible to everyone.

   Metanoia Society — Transformative media for a better world.
   ```

4. **App icon** (512x512px PNG, max 1 MB):
   - Upload from: `src\Aurora\Resources\AppIcon\appicon.png` (resize to 512x512 if source is 1536x1536)

5. **Feature graphic** (1024x500px, optional but recommended):
   - Can create placeholder or use app branding

6. **Screenshots** (at least 2 required):
   - Phone screenshots from `assets\` directory:
     - `Daily_Picks_Light_Mode.jpg`
     - `Daily_Picks_Dark_Mode.jpg`
     - `Vibe_Light_Mode.jpg`
     - `Vibe_Dark_Mode.jpg`

7. **Click "Save"**

**Note:** Store listing changes require Google review for public releases (Closed/Open Testing, Production) but are displayed immediately for Internal Testing after saving.

---

## Azure Content Deployment

Aurora's content (Vibe of the Day + Daily Picks) is deployed to Azure Blob Storage and served via Azure Functions API.

### Content Management PowerShell Scripts

Located in: `tools/content-management/`

1. **`New-ContentTemplate.ps1`**: Generate JSON templates with placeholder values
2. **`Validate-Content.ps1`**: Syntax validation before deployment
3. **`Deploy-Content.ps1`**: Upload to Azure Blob Storage
4. **`Rollback-Content.ps1`**: Restore previous version if deployment fails

### Deployment Workflow

**Step 1: Generate Template**
```powershell
cd C:\Programming\Project-Graveyard\Project-Aurora\tools\content-management
.\New-ContentTemplate.ps1 -VibeCount 1 -PicksCount 10 -OutputFile .\new-content.json
```

**Step 2: Populate Content**
- Open `new-content.json` in VS Code
- Fill in real stories: titles, snippets, URLs, images, published dates

**Step 3: Validate**
```powershell
.\Validate-Content.ps1 -FilePath .\new-content.json
```
- Must return **zero errors** (warnings acceptable)

**Step 4: Deploy to Production**
```powershell
.\Deploy-Content.ps1 -FilePath .\new-content.json -Environment Production
```
- Uploads to Azure Blob Storage: `aurora-content-beta` container
- Previous version automatically backed up to `tools/content-management/backups/`

**Step 5: Verify**
- Open Aurora on test device (S24 Ultra)
- Pull to refresh (if implemented) or force-stop and reopen app
- Confirm new stories appear

**Rollback (if needed):**
```powershell
.\Rollback-Content.ps1 -Environment Production -BackupTimestamp YYYYMMDD-HHMMSS
```

### Azure Infrastructure

**Resource Group:** `rg-aurora-beta`

**Key Resources:**
- **Storage Account:** `staurorabet` (content blob storage)
- **Function App:** `func-aurora-beta-*` (API endpoints)
- **Table Storage:** Reaction persistence (Uplift counts)

**API Endpoints:**
- GET `/api/content` - Fetch latest content.json
- POST `/api/reactions` - Submit reaction (Uplift button)
- GET `/api/reactions/{storyId}` - Get reaction count

**Monitoring:**
- Azure Portal: https://portal.azure.com
- Resource Group: `rg-aurora-beta`
- Check Function App logs for API errors
- Check Storage Account metrics for blob access

---

## Version Increment Checklist

When preparing a new release, update version numbers in `src/Aurora/Aurora.csproj`:

```xml
<!-- Versions -->
<ApplicationDisplayVersion>1.0.1</ApplicationDisplayVersion>  <!-- User-facing version (semantic versioning) -->
<ApplicationVersion>2</ApplicationVersion>                    <!-- Google Play version code (must increment for each upload) -->
```

**Rules:**
- **ApplicationDisplayVersion**: User-facing (e.g., `1.0.0` → `1.0.1` for bug fixes, `1.1.0` for new features)
- **ApplicationVersion**: Google Play version code (integer, must increment: `1` → `2` → `3`...)

**Important:** Google Play rejects uploads with duplicate `ApplicationVersion` values. Always increment before building a new AAB.

---

## Security Checklist

Before deploying, verify:

- [ ] Keystore password NOT committed to Git (environment variable only)
- [ ] `.gitignore` includes `*.keystore`, `*.jks`, `keystore.properties`
- [ ] Keystore backed up to 3 secure locations (Bitwarden + cloud + USB)
- [ ] Google Play opt-in URL NOT committed to public repositories
- [ ] Azure connection strings NOT committed to Git (environment variables or appsettings.json excluded from source control)

---

## Troubleshooting

### AAB Build Fails

**Error:** "Keystore password is incorrect"
- **Solution:** Verify password in Bitwarden; ensure `$env:AURORA_KEYSTORE_PASSWORD` is set correctly

**Error:** "Could not find keystore file"
- **Solution:** Check keystore exists at `C:\Programming\Keystores\aurora-release.keystore`

**Error:** "Build failed with errors"
- **Solution:** Run `dotnet clean` then retry; check MAUI workload installed: `dotnet workload list`

### Google Play Upload Issues

**Error:** "Version code already exists"
- **Solution:** Increment `<ApplicationVersion>` in `Aurora.csproj`

**Error:** "Signature does not match previous upload"
- **Solution:** CRITICAL - Do NOT regenerate keystore. Verify using correct keystore file at `C:\Programming\Keystores\aurora-release.keystore`

**Error:** "Opt-in link shows 'You don't have access'"
- **Solution:** Add tester's Google account email to Internal Testing list in Console

### Content Deployment Failures

**Error:** "Blob upload failed - connection string invalid"
- **Solution:** Check Azure Storage connection string in `Deploy-Content.ps1` or environment variable

**Error:** "New content doesn't appear in app"
- **Solution:** Verify blob uploaded in Azure Portal; force-stop and reopen app; check API logs for read errors

---

## Future Automation Opportunities

**Deferred to Milestone CT-1** (Content Tooling Enhancements):
- Migrate PowerShell scripts to .NET console apps for cross-platform support
- Implement GitHub Actions CI/CD for automated builds on commits
- Automated version incrementing based on commit messages (conventional commits)
- Slack/Discord notifications for successful deployments

For now, manual deployment ensures control and visibility during beta testing phase.

---

## Related Documentation

- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Technical architecture and cloud infrastructure
- **[DEBUGGING.md](DEBUGGING.md)** - Troubleshooting guides and known issues
- **[BACKLOG.md](../planning/BACKLOG.md)** - User stories and acceptance criteria
- **[PROJECT_JOURNAL.md](../planning/PROJECT_JOURNAL.md)** - Development session history
