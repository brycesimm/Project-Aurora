# Milestone V-0: Beta Readiness

**Status:** ✅ Completed
**Completion Date:** 2025-12-28

*This milestone transforms Aurora from a local development prototype to a cloud-connected application with real content, enabling meaningful beta testing that validates the core value proposition.*

---

**Objective:** Enable external beta testing by deploying infrastructure to Azure, integrating real content, and establishing reliable content management workflows.

**Success Criteria:**
- Testers can read real uplifting news articles from any network
- Application works independently of local development machine
- Reactions persist in cloud storage visible to all users
- Content can be updated without rebuilding/redistributing the app

**Structure:** This milestone is divided into two phases:
- **Phase 1 (Immediate):** Core infrastructure and real content integration (V-0.1 through V-0.4)
- **Phase 2 (Post-Verification):** Content management tooling after Phase 1 is tested (V-0.5 through V-0.7)

---

## Feature V-01: Cloud Infrastructure & Real Content (Phase 1)
*Core infrastructure deployment and real content integration to achieve a testable cloud-connected application.*

- [x] **Story V-0.1:** As a developer, I want the Aurora API and storage deployed to Azure so that beta testers can access the application from any network.
    - **AC 1:** [x] Azure Functions app is created and configured (Consumption Plan, .NET 9, Application Insights enabled, CORS configured for mobile app).
    - **AC 2:** [x] Azure Table Storage is deployed with "Reactions" table and connection string configured in Function App settings.
    - **AC 3:** [x] GetDailyContent endpoint is accessible from public internet at `https://aurora-api-[uniqueid].azurewebsites.net/api/GetDailyContent` and returns valid JSON.
    - **AC 4:** [x] ReactToContent endpoint persists data to cloud Table Storage; subsequent GET requests reflect updated counts.
    - **AC 5:** [x] MAUI app is reconfigured with production Azure Function URL; verified working on Android emulator and S24 Ultra over cellular.
    - **Edge Cases:**
        - Cold start delays (2-5 seconds) are acceptable for beta.
        - Use `appsettings.Development.json` vs. `appsettings.json` for local vs. cloud environment switching.
        - ReactionStorageService creates initial entity with count=1 if no existing reaction exists (already implemented).

- [x] **Story V-0.2:** As a user, I want to read the full article when I click "READ" so that I can consume the uplifting news content.
    - **AC 1:** READ button opens article in device's default browser using `Browser.OpenAsync(contentItem.ArticleUrl)` for both Vibe of the Day and Daily Picks.
    - **AC 2:** Invalid/missing URLs are handled gracefully: button is disabled if ArticleUrl is null/empty; malformed URLs show toast "Unable to open article" and log error.
    - **AC 3:** Button provides visual feedback (loading indicator or immediate disable) to prevent double-tap race conditions.
    - **AC 4:** Verified working on both Android emulator and S24 Ultra physical device.
    - **Edge Cases:**
        - Validate URL is well-formed (starts with http:// or https://) before calling Browser.OpenAsync().
        - Offline errors are handled by browser app (not Aurora's responsibility).
        - Defer "article read" analytics tracking to post-beta.
    - **Completed:** 2025-12-27 (Chrome Custom Tabs implementation, Azurite auto-start, real content deployed)

- [x] **Story V-0.3:** As a developer, I want the API to serve content from Azure Blob Storage so that content can be updated without redeploying the function app.
    - **AC 1:** [x] Azure Blob Storage container `aurora-content` is created (private access) with initial `content.json` file uploaded.
    - **AC 2:** [x] GetDailyContent function reads from Blob Storage using `BlobServiceClient`; connection string configured via App Settings (not hardcoded).
    - **AC 3:** [x] Content updates reflect immediately: upload new `content.json` via Azure CLI, call endpoint, receive new content (no caching).
    - **AC 4:** [x] Local development works with Azurite blob emulator (development connection string uses `UseDevelopmentStorage=true`).
    - **AC 5:** [x] Error handling: HTTP 404 if blob doesn't exist ("Content not yet published"), HTTP 500 if connection string is invalid ("Storage configuration error"); all errors logged to Application Insights.
    - **Edge Cases:**
        - [x] Implement retry logic (Polly policy, 3 retries with exponential backoff) for blob download failures.
        - [x] Defer caching optimization to post-beta (blob reads are ~$0.0004 per 10K operations).
        - [x] Malformed JSON causes deserialization failure; return HTTP 500 (mitigated by V-0.5 validation tooling in Phase 2).
    - **Completed:** 2025-12-28 (Blob storage migration, Azurite tooling, dynamic content updates)

- [x] **Story V-0.4:** As a developer, I want real uplifting news stories in the application so that beta testers can evaluate actual content quality.
    - **AC 1:** [x] Initial content file contains 1 Vibe of the Day + 5-10 Daily Picks with real, recently published uplifting news articles, valid ArticleUrls, appropriate ImageUrls, and 2-3 sentence snippets.
    - **AC 2:** [x] Content sources are credible (r/UpliftingNews, Good News Network, Positive News, etc.); no broken links, no paywalled content.
    - **AC 3:** [x] Image handling: Use article's Open Graph/featured image URL where available (hotlinking); fallback to placeholder image (`https://via.placeholder.com/800x600/7986CB/FFFFFF?text=Aurora`) if hotlinking not allowed or image unavailable.
    - **AC 4:** [x] Content.json is uploaded to `aurora-content` blob container; GetDailyContent endpoint serves the real content.
    - **AC 5:** [x] Verified on S24 Ultra: all stories display correctly, READ buttons open articles, Uplift reactions work.
    - **Edge Cases:**
        - Broken links after publication are acceptable risk for beta (document as known issue).
        - Image licensing: use Creative Commons or Open Graph images (fair use for preview); document for production review.
        - Content curation is collaborative: both user and assistant find/format articles.

---

## Feature V-02: Content Management Tooling (Phase 2)
*Automation and validation scripts to streamline content curation workflow after Phase 1 verification.*

**Note:** These stories are deferred until after V-0.1 through V-0.4 are complete and tested.

- [x] **Story V-0.5:** As a content curator, I want automated validation of my content JSON so that I don't accidentally break the app with malformed data.
    - **AC 1:** PowerShell script `Validate-Content.ps1` exists in `tools/content-management/` directory; run with `.\Validate-Content.ps1 -FilePath .\content.json`.
    - **AC 2:** Script validates JSON against `content.schema.json`: checks syntax, validates required fields, validates UpliftCount is non-negative integer, validates URLs are well-formed (http/https).
    - **AC 3:** Clear error messages: Success: "✓ content.json is valid and ready to upload"; Failure examples: "✗ Error on line 15: Missing required field 'title' in DailyPicks[2]" or "✗ Error: 'article_url' must be a valid URL (got: 'not-a-url')".
    - **AC 4:** Optional image URL accessibility check: attempt HEAD request to each ImageUrl; warn (don't fail) if 404 or timeout (e.g., "⚠ Warning: Image URL for 'Coral Reef Story' returned 404").
    - **AC 5:** README.md in `tools/content-management/` documents script usage, dependencies, and result interpretation.
    - **Edge Cases:**
        - JSON schema validation works offline; image URL checks skip with warning if offline.
        - Optional quality validation: warn if snippet >200 chars (too long) or <50 chars (too short).
    - **Completed:** 2025-12-28 (PowerShell validation script with comprehensive error detection and quality warnings)

- [x] **Story V-0.6:** As a content curator, I want a one-command deployment process so that I can publish content updates quickly and reliably.
    - **AC 1:** [x] PowerShell script `Deploy-Content.ps1` exists in `tools/content-management/`; run with `.\Deploy-Content.ps1 -FilePath .\content.json -Environment Production`.
    - **AC 2:** [x] Script performs pre-deployment validation: automatically runs `Validate-Content.ps1`; aborts if validation fails; requires confirmation prompt before deployment.
    - **AC 3:** [x] Script uploads to Azure Blob Storage using Azure CLI (`az storage blob upload`); overwrites existing file; displays "✓ Content deployed to Dev at 2025-12-28 17:40:24".
    - **AC 4:** [x] Supports multiple environments: `-Environment Dev` vs. `-Environment Production`; defaults to Dev for safety.
    - **AC 5:** [x] Rollback capability: before upload, downloads current `content.json` and saves as `content.backup.<Environment>.YYYY-MM-DD-HHMMSS.json`; provides `.\Rollback-Content.ps1` to restore; keeps last 5 backups per environment.
    - **AC 6:** [x] README.md documents full workflow: Edit → Validate → Deploy → Verify → Rollback (if needed).
    - **Edge Cases:**
        - [x] Script checks for `az` command; provides install instructions if missing.
        - [x] Uses `az login` for authentication (user authenticates once, credentials cached); uses key-based auth for blob operations.
        - [x] Azure Blob upload is atomic (all or nothing); if upload fails, old content remains active.
    - **Completed:** 2025-12-28 (PowerShell deployment automation with backup/rollback, tested successfully on Dev environment)

- [x] **Story V-0.7:** As a content curator, I want a template generator for content.json so that I can quickly create properly formatted content files.
    - **AC 1:** [x] PowerShell script `New-ContentTemplate.ps1` exists in `tools/content-management/`; run with `.\New-ContentTemplate.ps1 -VibeCount 1 -PicksCount 7 -OutputFile .\my-content.json`.
    - **AC 2:** [x] Generated template includes all required fields with helpful placeholders: `"id": "REPLACE_WITH_UNIQUE_ID_1"`, `"title": "TODO: Article Title Here"`, `"snippet": "TODO: 2-3 sentence summary"`, `"article_url": "https://example.com/article"`, `"image_url": "https://placeholder.com/800x600"`, `"published_date": "2025-12-28"` (auto-filled with today), `"uplift_count": 0`.
    - **AC 3:** [x] Includes helpful README explaining each field and next steps guidance in script output.
    - **AC 4:** [x] Supports bulk creation: `-VibeCount 1 -PicksCount 10` creates 11 items; IDs auto-numbered: `REPLACE_WITH_UNIQUE_ID_1`, `REPLACE_WITH_UNIQUE_ID_2`, etc.
    - **AC 5:** [x] Generated file passes validation syntax check (confirmed with Validate-Content.ps1).
    - **Edge Cases:**
        - [x] Uses obvious placeholders (`REPLACE_WITH_UNIQUE_ID_N`, `TODO:`) to force replacement.
        - [x] Deferred automatic metadata fetching (title, image from URL) to future enhancement.
    - **Completed:** 2025-12-28 (PowerShell template generator with comprehensive README.md documentation)
