# Project Aurora - Backlog

This document tracks the features, user stories, and tasks for Project Aurora. It is organized by milestones defined in `PLANNING.md`.

---

## Milestone A: MAUI Onboarding + Shell with Mock Data

### Feature A-01: Application Shell & Mock Feed
*This feature covers the creation of the basic UI shell and the display of mock data to represent the core home screen experience.*

- [x] **Story A-01.1:** As a user, I want a basic application shell so that I can see the app's title and main content area.
    - **AC 1:** The application window/page displays "Project Aurora" as the title.
    - **AC 2:** The shell provides a designated, styled content area for the main page's content.
- [x] **Story A-01.2:** As a user, I want to see a "Vibe of the Day" hero card on the main page to view the featured story.
    - **AC 1:** A visually distinct "hero" card is displayed at the top of the main page.
    - **AC 2:** The card contains a mock title and a short paragraph of mock text.
    - **AC 3:** A placeholder for an image is included within the card.
- [x] **Story A-01.3:** As a user, I want to see a list of mock "Daily Picks" so I can scroll through uplifting stories.
    - **AC 1:** A scrollable list is displayed below the "Vibe of the Day" card.
    - **AC 2:** The list contains 5-10 mock story items.
    - **AC 3:** Each item in the list displays a mock title and a short text snippet.
- [x] **Story A-01.4:** As a user, I want to see a placeholder reaction button on each story to understand how I will interact with content.
    - **AC 1:** A button with a label (e.g., "Uplift") is visible on the Vibe card and on each Daily Pick item.
    - **AC 2:** The button's click action is not required to be functional for this story.

---

## Milestone B: Backend/API + Curation Workflow
*This milestone covers the definition of our data structure and the creation of a local API endpoint to serve mock content, forming the foundation for our backend.*

### Feature B-01: Content Structure & API Definition
- [x] **Story B-01.1:** As a developer, I need a clearly defined JSON schema for the app's content so that the front-end and back-end have a shared understanding of the data model.
    - **AC 1:** [x] A `content.schema.json` file is created to formally define the structure, including fields like `id`, `title`, `snippet`, `sourceUrl`, `imageUrl`, and `publicationDate`.
    - **AC 2:** [x] The schema distinguishes between the "Vibe of the Day" and standard "Daily Picks," allowing for unique fields on the Vibe card.
    - **AC 3:** [x] A `sample.content.json` file is created that validates against the schema, containing one "Vibe" and at least five "Picks."
- [x] **Story B-01.2:** As a developer, I need a local, HTTP-triggered function to serve the mock content, allowing for front-end development without a live cloud environment.
    - **AC 1:** A new, runnable .NET Azure Functions project is added to the solution.
    - **AC 2:** An HTTP GET endpoint named `GetDailyContent` is created within the new project.
    - **AC 3:** When called, the endpoint reads `sample.content.json` and returns its contents with a `200 OK` status and correct `Content-Type` header (`application/json`).

---

### Feature B-02: Automated JSON Schema Generation
*This feature covers the automation of JSON schema generation from C# models, improving developer tooling and ensuring data contract consistency.*

- [x] **Story B-02.1:** As a developer, I need to define the app's content schema so that the front-end and back-end have a shared understanding of the data model.
    - **AC 1:** A `content.schema.json` file is created to formally define the structure.
    - **AC 2:** The schema distinguishes between the "Vibe of the Day" and standard "Daily Picks."
    - **AC 3:** A `sample.content.json` file is created that validates against the schema.
- [x] **Story B-02.2:** As a developer, I want the `SchemaBuilder` project to directly generate the `content.schema.json` file from the C# models, ensuring a single source of truth for the data contract.
    - **AC 1:** [x] The `SchemaBuilder` project is a console application and does not run a web server.
    - **AC 2:** [x] When executed, `SchemaBuilder` generates the OpenAPI schema for the `ContentItem` model in-memory.
    - **AC 3:** [x] `SchemaBuilder` saves the generated schema to the `content.schema.json` file in the project root.
    - **AC 4:** [x] The generated schema is a valid JSON schema for the `ContentItem` model.
- [x] **Story B-02.3:** As a developer, I want to integrate schema export into the build process so that `content.schema.json` is always up-to-date.
    - **AC 1:** Building the `SchemaBuilder` project (or the entire solution) automatically triggers the schema generation.
    - **AC 2:** `content.schema.json` is updated with the latest schema during the build process.
    - **AC 3:** The build process completes without errors or unexpected user interaction.

---

## Milestone C: Live Data Integration & Core Features
*This milestone focuses on connecting the MAUI application to the backend API, implementing core features like data loading, error handling, and sharing.*

### Feature C-01: Data Service Integration
*This feature covers establishing the communication layer between the MAUI application and the backend API, enabling the retrieval of live content.*

- [x] **Story C-01.1:** As a developer, I need to define the data models for the content feed and create an interface for the content service, so that I can establish a clear contract for fetching data.
    - **AC 1:** A `ContentFeed.cs` model is created in `Aurora.Shared` to represent the entire data payload, containing a `VibeOfTheDay` (`ContentItem`) and a list of `DailyPicks` (`List<ContentItem>`).
    - **AC 2:** An `IContentService` interface is created in `Aurora.Shared` with a method `GetDailyContentAsync()` that returns a `Task<ContentFeed>`.
- [x] **Story C-01.2:** As a developer, I need a concrete implementation of the content service that fetches data from the local Azure Function so that the MAUI app can retrieve mock content.
    - **AC 1:** A `ContentService` class is created in the `Aurora` project that implements `IContentService`.
    - **AC 2:** The `ContentService` uses `HttpClient` to call the `GetDailyContent` endpoint of the local Azure Function (`http://localhost:7071/api/GetDailyContent`).
    - **AC 3:** The service correctly deserializes the JSON response into a `ContentFeed` object.
- [x] **Story C-01.3:** As a developer, I need to register the content service with the MAUI application's dependency injection container so that it can be easily consumed by other parts of the application.
    - **AC 1:** The `ContentService` is registered as a singleton service for `IContentService` in `MauiProgram.cs`.
    - **AC 2:** The `HttpClient` required by the `ContentService` is also registered with the DI container.
- [x] **Story C-01.4:** As a user, I want to see the actual content fetched from the local API displayed in the app so that I can verify the data integration.
    - **AC 1:** The `MainPage` (or its ViewModel) consumes the `IContentService`.
    - **AC 2:** On page load, the app calls `GetDailyContentAsync()` to retrieve the content.
    - **AC 3:** The "Vibe of the Day" card and the "Daily Picks" list are populated with data from the service, replacing the old mock data.
    - **AC 4:** The app handles potential `null` or empty content gracefully (e.g., by displaying a "No content available" message).

---

## Milestone D: Robust Testing Framework & Service Testability
*This milestone focuses on establishing a comprehensive testing strategy, including architectural refactoring to enable isolated unit testing of services and shared logic.*

### Feature D-01: Service Testability Refactor
- [x] **Story D-01.1:** As a developer, I need to refactor core service logic into a dedicated class library so that it can be independently unit tested without MAUI framework dependencies.
    - **AC 1:** [x] A new `.NET 9.0` class library project (`Aurora.Client.Core`) is created.
    - **AC 2:** [x] All shared service logic, including `ContentService`, is moved into this new project.
    - **AC 3:** [x] `Aurora.csproj` (MAUI app) is updated to reference the new core project.
    - **AC 4:** [x] `Aurora.Client.Core.csproj` references `Aurora.Shared.csproj`.
    - **AC 5:** [x] `Aurora.Client.Core.Tests` project is created and configured to reference `Aurora.Client.Core.csproj`.
- [x] **Story D-01.2:** As a developer, I need to implement unit tests for `ContentService` so that its data retrieval and deserialization logic is verified.
    - **AC 1:** [x] Tests use `Moq` to create a mock `HttpMessageHandler` for `HttpClient`.
    - **AC 2:** [x] Tests provide a controlled JSON string matching the `ContentFeed` schema.
    - **AC 3:** [x] Tests assert that `GetDailyContentAsync` correctly deserializes the mock JSON into a `ContentFeed` object, validating key properties.
    - **AC 4:** [x] All tests pass successfully.

### Feature D-02: Automated Quality Assurance
- [x] **Story D-02.1:** As a developer, I want a CI pipeline to automatically build and test my code so that I don't merge broken features.
    - **AC 1:** A GitHub Actions workflow file (e.g., `.github/workflows/dotnet.yml`) is created.
    - **AC 2:** The workflow triggers on `push` to `main` and `pull_request` events.
    - **AC 3:** The workflow sets up .NET 9, builds the solution, and runs all unit tests.
    - **AC 4:** The workflow completes successfully on the current codebase.

### Feature D-03: Code Style Standardization
- [x] **Story D-03.1:** As a developer, I want consistent code style enforced automatically so that the codebase remains uniform without manual policing.
    - **AC 1:** A `Directory.Build.props` file is created at the solution root to enable native `.NET Analyzers` (AnalysisLevel: latest-all).
    - **AC 2:** A root `.editorconfig` file defines basic conventions (tabs, file-scoped namespaces).
    - **AC 3:** The solution builds successfully with **zero warnings**, ensuring a pristine codebase.

---

## Milestone E: Core Interactions & Polish
*This milestone transforms the application from a read-only prototype into an interactive experience, completing the core "Uplift" loop and ensuring visual consistency.*

### Feature E-01: User Reactions
*This feature enables the anonymous "Uplift" reaction system, requiring backend state persistence.*

- [x] **Story E-01.1:** As a developer, I need to update the data model to track reaction counts so that the UI can display how many people have been uplifted.
    - **AC 1:** `ContentItem.cs` includes a new integer property `UpliftCount`.
    - **AC 2:** `content.schema.json` is updated via `SchemaBuilder` to include `uplift_count`.
    - **AC 3:** `sample.content.json` is updated with mock values for testing.
- [x] **Story E-01.2:** As a developer, I need a backend storage service to persist reaction counts so that data survives app restarts.
    - **AC 1:** `Aurora.Api` includes the `Azure.Data.Tables` package.
    - **AC 2:** A `TableStorageService` is implemented to Handle `Get` and `Upsert` operations for reaction entities.
    - **AC 3:** The service is configured to use Azurite (local emulator) for development.
- [x] **Story E-01.3:** As a developer, I need an API endpoint to handle reactions so that the client can submit user feedback.
    - **AC 1:** A new HTTP POST endpoint `/api/articles/{id}/react` is created.
    - **AC 2:** The endpoint increments the count in Table Storage and returns the new total.
    - **AC 3:** The endpoint is anonymous (no user login required).
- [x] **Story E-01.4:** As a user, I want to click the "Uplift" button to register my positive sentiment and see the count increase.
    - **AC 1:** The "Uplift" button in `MainPage` is wired to the `ContentService`.
    - **AC 2:** Clicking the button immediately increments the count visually (optimistic update).
    - **AC 3:** The app sends the request to the API in the background.
    - **AC 4:** If the API fails, the count is rolled back or an error is logged (silent failure preferred for UX).

### Feature E-02: Native Sharing
*This feature leverages the device's native capabilities to share content externally.*

- [x] **Story E-02.1:** As a user, I want to share a story with friends so that I can spread positivity.
    - **AC 1:** A "Share" icon button is added to the "Vibe of the Day" card and each "Daily Pick".
    - **AC 2:** Clicking the button opens the native OS share sheet (iOS/Android).
    - **AC 3:** The shared content includes the Story Title and the Article URL.

### Feature E-03: Visual Design System
*This feature covers the definition and implementation of a cohesive visual language.*

- [x] **Story E-03.1: Define Design System & Palette**
    - **AC 1:** `docs/DESIGN_SYSTEM.md` is populated with a finalized color palette (Primary, Background, Surface) for Light and Dark modes.
    - **AC 2:** Typography scale (Headings, Body, Caption) is defined and documented.
    - **AC 3:** Corner radius and spacing standards are defined.
- [x] **Story E-03.2: Implement Color & Theme Resources**
    - **AC 1:** `Colors.xaml` is updated with the new palette, replacing default MAUI colors.
    - **AC 2:** Semantic resource keys (e.g., `PrimaryBrush`, `SurfaceBrush`) are created to abstract raw hex values.
    - **AC 3:** `Styles.xaml` is updated to apply these resources globally to common controls.
- [x] **Story E-03.3: Apply Design System to Main Page**
    - **AC 1:** The "Vibe of the Day" card uses the new Surface color, corner radius, and shadow elevation.
    - **AC 2:** Typography styles are applied to all Labels (Hero Title vs. Body Text).
    - **AC 3:** Buttons (Uplift, Share) use the new primary/secondary styling.
    - **AC 4:** Spacing (Margins/Padding) is audited to match the defined grid (e.g., 8pt multiples).
- [x] **Story E-03.4: Visual Refinements (Mobile Verification)**
    - **AC 1:** Title font sizes and weights are adjusted for better legibility on physical devices.
    - **AC 2:** Paragraph line heights and spacing are tuned for comfortable reading on small screens.
    - **AC 3:** Button icons and text labels are aligned and spaced correctly to avoid cramping.

---

## Milestone V-0: Beta Readiness
*This milestone transforms Aurora from a local development prototype to a cloud-connected application with real content, enabling meaningful beta testing that validates the core value proposition.*

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

### Feature V-01: Cloud Infrastructure & Real Content (Phase 1)
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

### Feature V-02: Content Management Tooling (Phase 2)
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

---

## Milestone V-2: Beta Testing Round 1 (Early Alpha Quality)

**Objective:** Establish beta testing infrastructure and execute the first round of validation (self-testing + 1-4 trusted testers) to identify improvements and validate Aurora's core value proposition.

**Target Completion:** 2 weeks from start

**Success Criteria:**
- ✅ Google Forms created and integrated into app for structured feedback
- ✅ Google Play Internal Testing track operational with professional distribution
- ✅ 7-14 day self-validation completed with structured findings documented
- ✅ 1-4 external testers recruited and onboarded (optional, based on availability)
- ✅ Improvement backlog documented with prioritization (Critical/High/Medium/Low)
- ✅ Go/No-Go decision made: Continue refining Aurora OR Pivot substantially

**Platform Scope:** Android-only (iOS explicitly deferred pending Mac/device acquisition)

**Testing Philosophy:** This is early-stage beta testing (alpha quality) focused on discovering critical gaps and validating core value proposition with controlled audience before broader distribution.

**Terminology Note:** We use "beta" for consistency with Azure infrastructure naming (`rg-aurora-beta`), but this milestone represents alpha-quality testing with a small, trusted cohort.

---

### Feature V-2.1: Feedback Collection Infrastructure
*This feature establishes structured surveys to capture quantitative and qualitative insights without survey fatigue.*

- [x] **Story V-2.1.1:** As a project owner, I want a baseline survey that captures tester context and expectations so that I can establish a pre-use benchmark for measuring Aurora's impact.
    - **AC 1:** ✅ Google Form created with 14 questions (expanded from 8) covering: contact info (method + details), app usage patterns, social media context, sentiment analysis, positivity definition, and value proposition validation
    - **AC 2:** ✅ Most questions configured as optional (3 required: contact method, contact details, value proposition rating)
    - **AC 3:** ✅ Questions with simple answers include follow-up text box: "Want to elaborate? (Optional)" for detailed feedback
    - **AC 4:** ✅ Form configured to save responses to Google Sheets automatically with timestamp
    - **AC 5:** ✅ Shareable link generated (`https://forms.gle/eRq3qY1EveJbP6Q87`) and documented in `docs/beta/SURVEY_LINKS.md`
    - **AC 6:** ✅ Form tested with 2 test responses, verified data appears correctly in Google Sheets with all columns populated
    - **AC 7:** 🔄 Deferred to Story V-2.2.1 (Beta Tester Guide not yet created)
    - **Completed:** 2025-12-29
    - **Notes:** Survey expanded beyond original spec to include Google Play Store email collection (1 required question) and enhanced app usage analysis (Q4-Q6, Q14). Email-only contact simplifies tester onboarding (email = whitelist identifier = communication channel). Updated 2025-12-31 to remove Discord/Reddit options for streamlined workflow.
    - **Edge Cases:**
        - Tester skips optional questions: Allowed; only value proposition question (#6 in proposed list) is required
        - Political sensitivity (topics to avoid): Question phrased neutrally: "Are there topics you'd prefer Aurora avoid, even if generally positive? (e.g., political figures, religious themes, specific social issues)"
        - Google account required: Configure form to NOT require sign-in (Settings → Responses → "Limit to 1 response" unchecked)
    - **Survey Questions (Final):**
        1. "On average, how many hours per day do you spend on social media or news apps?" (Scale: <1, 1-2, 2-4, 4-6, 6+) + Optional text: "Want to elaborate?"
        2. "How does social media/news consumption typically make you feel?" (Multiple choice: Informed, Anxious, Entertained, Angry, Hopeful, Drained, Other) + Optional text
        3. "How often do you avoid news or social media due to negative content?" (Scale: Never → Always) + Optional text
        4. "What types of stories do you consider 'positive'? Select all that apply:" (Checkboxes: Scientific breakthroughs, Environmental progress, Human kindness, Animal welfare, Social justice, Economic good news, Health/medical advances, Arts & culture, Other)
        5. "Are there topics you'd prefer Aurora avoid, even if generally positive?" (Open text, optional)
        6. **"How valuable would an app focused exclusively on uplifting news be to you?"** (Scale: Not valuable → Extremely valuable) **[REQUIRED]**
        7. "What app do you typically open first when you pick up your phone?" (Open text, optional)
        8. "How important is it that a news app has a mobile-optimized reading experience?" (Scale: Not important → Extremely important) + Optional text

- [x] **Story V-2.1.2:** As a project owner, I want a weekly feedback survey to track sentiment, usage patterns, and friction points during beta testing so that I can identify improvements iteratively.
    - **AC 1:** ✅ Google Form created with 12 questions (expanded to include standalone "Want to elaborate?" questions) covering: usage frequency, first-app-opened behavior, articles read, content resonance, sharing behavior, mood impact, UX friction (good/bad moments), bugs, and continuation intent
    - **AC 2:** ✅ All questions optional except "How many days this week did you open Aurora?" (allows tracking zero-usage weeks)
    - **AC 3:** ✅ Simple-answer questions include "Want to elaborate?" text boxes for detailed feedback (Q7 for mood, Q12 for continuation intent)
    - **AC 4:** ✅ Form configured to save responses to **separate Google Sheets** from baseline survey (new spreadsheet created)
    - **AC 5:** ✅ Shareable link generated and documented in `docs/beta/SURVEY_LINKS.md`
    - **AC 6:** ✅ Form tested with 2 test submissions, verified data separation from baseline responses
    - **AC 7:** 🔄 Deferred to Story V-2.2.1 (Beta Tester Guide not yet created)
    - **Completed:** 2025-12-29
    - **Notes:** Survey expanded from 10 to 12 questions by making "Want to elaborate?" standalone questions rather than conditional follow-ups. Changed Q6 to "Select all that apply" for better mood tracking. Question wording adjusted to include "Project Aurora" for clarity. Q11 changed from "next week" to general "would you continue" for broader applicability.
    - **Edge Cases:**
        - Tester skips a week: Weekly form is optional; no enforcement or reminders (low-pressure alpha environment)
        - Tester had zero usage that week: Question #1 allows "0 days" response; remaining questions can be skipped
        - Response volume too low for analysis: If <3 responses after 2 weeks, pivot to 1-on-1 interviews instead (document in findings)
    - **Survey Questions (Final):**
        1. **"How many days this week did you open Aurora?"** (Scale: 0, 1, 2-3, 4-5, 6-7) **[REQUIRED]**
        2. "Was Aurora the first app you opened when picking up your phone?" (Multiple choice: Never, Rarely, Sometimes, Often, Always) + Optional text
        3. "How many articles did you read in full this week?" (Scale: 0, 1-2, 3-5, 6-10, 10+) + Optional text
        4. "Which story resonated with you most this week?" (Open text, optional)
        5. "Did you share any stories from Aurora? If not, why?" (Open text, optional)
        6. "How did using Aurora make you feel this week?" (Multiple choice: More hopeful, Less anxious, Informed, Neutral, Disappointed, Other) + Optional text
        7. "What felt **good** when using Aurora this week?" (Open text, optional)
        8. "What felt **bad** or frustrating?" (Open text, optional)
        9. "Did you encounter any bugs or crashes?" (Yes/No + description if yes)
        10. "Would you continue using Aurora next week?" (Yes / Maybe / No) + "Why?" text box

- [x] **Story V-2.1.3:** As a beta tester, I want easy access to the feedback form from within Aurora so that I don't have to search for the link in emails or documentation.
    - **AC 1:** ✅ "Share Feedback" button added at bottom of Daily Picks list (CollectionView.Footer) for natural "end of content" placement
    - **AC 2:** ✅ Button uses "Morning Mist" design system: CommentAccent background, 2.0dp purple border, rounded pill shape (24dp corner radius), Nunito ExtraBold font
    - **AC 3:** ✅ Button icon: Material Design Icons `file-document-edit-outline` (U+F0DC9) with label "Share Feedback"
    - **AC 4:** ✅ Confirmation dialog before opening ("Would you like to provide feedback on your experience with Aurora?" → "Yes, Open Survey" / "Not Right Now")
    - **AC 5:** ✅ Tapping "Yes" opens weekly survey in Chrome Custom Tabs (BrowserLaunchMode.SystemPreferred)
    - **AC 6:** ✅ Configuration via nested `BetaSettings` object in appsettings.json with `IsBetaTesting` flag and `WeeklyFeedbackFormUrl`
    - **AC 7:** ✅ Button visibility controlled by `BetaSettings:IsBetaTesting` flag (production-ready: set false to hide)
    - **AC 8:** ✅ Graceful error handling with MainThread-safe alerts for invalid URLs or browser launch failures
    - **AC 9:** ✅ Threading fix: Button re-enable wrapped in `MainThread.BeginInvokeOnMainThread()` to prevent crash
    - **AC 10:** ✅ Zero-warning build maintained (Debug and Release configurations)
    - **AC 11:** ✅ Verified on Android emulator and S24 Ultra physical device (Wi-Fi + cellular)
    - **Completed:** 2025-12-29
    - **Enhancements Implemented:**
        - Beta mode toggle for clean production transition (no code changes required)
        - Polite confirmation dialog prevents accidental taps
        - `file-document-edit-outline` icon for semantic accuracy (editing/providing feedback)
        - Nested `BetaSettings` object enables future expansion (expiration dates, baseline survey URL, debug flags)
        - Custom Aurora logo integrated (2048x2048px with BaseSize="1536,1536" for optimal scaling)

---

### Feature V-2.2: Beta Tester Onboarding Documentation
*This feature provides clear, concise instructions for beta testers so they understand expectations and can successfully install/use Aurora.*

- [x] **Story V-2.2.1:** As a beta tester, I want a clear onboarding guide so that I understand Aurora's purpose, how to install it, and how to provide valuable feedback.
    - **AC 1:** `docs/beta/BETA_TESTER_GUIDE.md` created with sections: Welcome & Purpose, Installation, How to Provide Feedback, What We're Testing, Known Limitations
    - **AC 2:** Guide is 1-2 pages (300-500 words) in **conversational, warm tone** matching "Morning Mist" design philosophy (friendly, approachable, not clinical)
    - **AC 3:** All links clearly marked as placeholders until dependencies complete:
        - `[BASELINE SURVEY LINK]` (Story V-2.1.1)
        - `[WEEKLY SURVEY LINK]` (Story V-2.1.2)
        - `[GOOGLE PLAY INSTALL LINK]` (Story V-2.3.2)
    - **AC 4:** Guide includes time commitment transparency: "~5-10 min/day for 1-2 weeks, plus one 5-min survey weekly"
    - **AC 5:** Known Limitations section explicitly states: "Android-only for now; iOS planned for future," "Manual content curation (may lag on weekends)," "Minimal features intentionally (testing core concept)"
    - **AC 6:** Contact information section: Provide email (themetanoiasociety@gmail.com) for urgent bugs + reminder that forms are preferred for structured feedback
    - **AC 7:** Guide reviewed for clarity (no jargon, no assumptions about technical knowledge)
    - **AC 8:** PDF export created for easy email distribution: `docs/beta/BETA_TESTER_GUIDE.pdf`
    - **Edge Cases:**
        - Tester loses the guide: Email includes PDF attachment + link to GitHub docs (`https://github.com/.../BETA_TESTER_GUIDE.md`)
        - Links change: Guide lives in Git; can be updated and redistributed quickly
        - Non-technical tester confused by installation: Guide uses screenshots (add in V-2.3.2 after first successful install)
    - **Guide Structure (Final):**
        - **Section 1: Welcome & Purpose** (2-3 sentences: refuge from negative news, you're helping validate if this concept resonates)
        - **Section 2: Installation** (Step 1: Baseline survey, Step 2: Google Play opt-in link, Step 3: Open app and explore)
        - **Section 3: How to Provide Feedback** (Weekly survey every Sunday, In-app "Share Feedback" button, Email for urgent bugs)
        - **Section 4: What We're Testing** (Content quality, user experience, value proposition: "Does Aurora make you feel more hopeful?")
        - **Section 5: Known Limitations** (Android-only, manual curation, minimal features, early beta quality)
    - **Completed:** 2025-12-30
    - **Notes:** Guide significantly expanded beyond original 300-500 word spec (~1,400 words final) to address non-tech-savvy tester needs. Key additions: "What is Aurora?" value proposition section, "✅ You'll Know Aurora is Working If..." expected behavior guide, comprehensive device info instructions with manufacturer variations, beginner-friendly bug template with copy-paste format + filled example, article accessibility limitations (paywall disclosure), installation confirmation with Aurora logo (80x80px), response time expectations (24-48hr), flexible weekly survey timing, usage reassurance ("any feedback is good feedback"). PDF generated via Pandoc→HTML→browser Print-to-PDF with custom CSS (Nunito font, Morning Mist palette #7986CB, emojis throughout). Comprehensive review from non-tech-savvy perspective identified and resolved 11 critical gaps. Contact simplified to email-only (themetanoiasociety@gmail.com); Discord references removed per user preference. Survey links active (baseline/weekly forms from V-2.1.1/V-2.1.2); Google Play link referenced as "sent via email" pending V-2.3.2. All 8 ACs met; guide ready for email distribution.

- [x] **Story V-2.2.2:** As a developer, I want documented steps for generating signed AABs and distributing via Google Play so that I can repeat the process reliably for future beta rounds.
    - **NOTE:** This story **MOVED AFTER V-2.3.2** to capture lessons learned from first deployment.
    - **AC 1:** ✅ `docs/technical/DEPLOYMENT.md` created with step-by-step signing and upload instructions based on actual experience from Story V-2.3.2
    - **AC 2:** ✅ Keystore creation and **backup procedure** documented with explicit security warnings (3 secure locations: Bitwarden + cloud storage + external USB)
    - **AC 3:** ✅ AAB generation documented: PowerShell environment variable setup + `dotnet publish -c Release -f net9.0-android` command
    - **AC 4:** ✅ Google Play Internal Testing release process documented: Console navigation, AAB upload, release notes format, tester configuration, rollout steps
    - **AC 5:** ✅ Store listing configuration documented: app name, descriptions, icon upload, screenshots
    - **AC 6:** ✅ Process verified with Story V-2.3.2 successful deployment (com.metanoiasociety.aurora-Signed.aab uploaded, release "1 (1.0.0)" rolled out)
    - **Edge Cases:**
        - **Keystore lost:** Cannot update the app without losing all installs. Mitigation: Document backup to 3 locations (local, cloud, password manager) in bold/red text
        - AAB rejected by Google: Console provides reason; likely signing config issue. Document common errors: version code conflict, missing permissions, improper signing
        - Tester can't install: Check device compatibility (minimum Android version). Guide should list requirements: "Android 8.0+ (API 26)"
    - **Technical Considerations:**
        - Keystore password must be stored securely (never in Git)
        - Automation deferred: Manual process acceptable for beta (revisit GitHub Actions in Beta Round 2+ if needed)
        - AAB vs. APK: AAB is Google's preferred format (smaller download size, dynamic delivery benefits)
        - Document version code increment requirement: `<ApplicationVersion>1.0.1</ApplicationVersion>` in `Aurora.csproj` for each upload

---

### Feature V-2.3: Google Play Internal Testing Setup
*This feature establishes professional distribution infrastructure for reliable installs and automatic updates.*

- [x] **Story V-2.3.1:** As a developer, I want a Google Play Console account so that I can distribute Aurora via Internal Testing track without requiring testers to sideload APKs.
    - **NOTE:** **Execute this story EARLY in milestone** (Day 1-2) to mitigate 24-48 hour account approval wait time.
    - **AC 1:** ✅ Visit `play.google.com/console/signup` and create Google Play Developer account (Google account: themetanoiasociety@gmail.com)
    - **AC 2:** ✅ Pay $25 one-time registration fee (credit card required, non-refundable)
    - **AC 3:** ✅ Complete developer profile: Developer name "Metanoia Society" (PERMANENT), developer type (Individual), contact information
    - **AC 4:** ✅ Accept Google Play Developer Distribution Agreement (read terms, especially content policy)
    - **AC 5:** ✅ Account status "Active" - approval granted 2025-12-31 (phone verification completed same day)
    - **AC 6:** ✅ Create new application entry: **App Title:** "Aurora - Uplifting Media" (package name assigned on first AAB upload in V-2.3.2)
    - **AC 7:** ✅ Application created in draft state (Internal Testing only, not published)
    - **Progress Notes:**
        - ✅ Developer name research completed: "Metanoia Society" selected after trademark/domain availability analysis
        - ✅ Application submitted with detailed developer background (curating/aggregating positive news content)
        - ✅ Identity verification completed (app install to device)
        - ✅ Phone verification completed (2025-12-31)
        - ✅ Account approved (2025-12-31, ~24 hours after submission)
    - **Edge Cases:**
        - Account approval delayed beyond 48 hours: Contact Google Play support via Console help center (usually resolves within 72 hours)
        - Payment issues: Verify billing address matches credit card; use card that accepts international transactions (Google Ireland)
        - Name collision: If "Aurora" package name taken, use `com.projectaurora.positivenews` or append unique identifier
        - **Name change concern:** Package name (`com.projectaurora.app`) is **permanent** and locked after first publish; App Title ("Aurora - Positive News") can be changed anytime without penalty
    - **Technical Considerations:**
        - One-time cost: $25 USD (lifetime developer account, no renewal fees)
        - Required info: Legal name (individual or business), physical address, phone number, email
        - Account unlocks: Internal Testing (100 testers), Closed Testing (unlimited), Open Testing (unlimited), Production releases
        - Package name recommendation: Use `com.projectaurora.app` to match existing Azure infrastructure naming (`rg-aurora-beta`, `func-aurora-beta-*`)
    - **Time Estimate:** 30-60 minutes (form filling + payment) + 24-48 hours wait (approval)
    - **Decision Points:**
        - **Package Name:** Does `com.projectaurora.app` work, or prefer more generic like `com.positivity.newsapp` to future-proof against rebranding?
        - **Developer Name (Shown on Play Store - PERMANENT):** This appears as "by [Developer Name]" on the Play Store and **cannot be changed** after account creation. Options to consider:
            - **"Project Aurora"** - Clean, established brand identity
            - **"Lilly Light Studios"** - Honors Lilly ("my light in this world"), professional studio naming
            - **"Morning Light Studios"** - Combines Aurora (dawn/morning) with Lilly's light symbolism
            - **"Aurora Light Labs"** - Merges both Aurora and light themes professionally
            - **"Lilly Labs"** - Simple, memorable, honors Lilly directly
            - **Personal name** (e.g., "Bryce Simmerman") - Standard indie developer approach, keeps personal branding
        - **Note:** App can include "In loving memory of Lilly" dedication in About section regardless of developer name chosen
        - **Recommendation:** Create **separate Google account** for Play Console (not personal Gmail) to enable brand identity and isolation (see PLANNING.md "Deferred Business & Legal Items" for full rationale)

- [x] **Story V-2.3.2:** As a developer, I want Aurora deployed to Google Play Internal Testing so that beta testers receive professional installs and automatic updates.
    - **AC 1:** ✅ Generate signed Release AAB - Completed 2025-12-31
        - **Keystore generated:** `C:\Programming\Keystores\aurora-release.keystore`
            - Alias: `aurora`, Validity: 10,000 days (~27 years), Algorithm: RSA 2048-bit
            - SHA-256 fingerprint: `EA:41:5B:D4:AC:73:D0:BE:A0:E5:F5:52:24:27:63:59:69:25:8A:1E:72:76:7D:E4:BC:3A:55:42:0B:DE:B6:D5`
            - Distinguished Name: `CN=Metanoia Society, OU=Unknown, O=Metanoia Society, L=Unknown, ST=Unknown, C=US`
        - **Backup strategy:** Keystore password stored in Bitwarden; keystore file backed up to 3 secure locations (Bitwarden attachment + cloud storage + external USB)
        - **Package name decision:** Changed from `com.projectaurora.app` to **`com.metanoiasociety.aurora`** (organization-scoped namespace for future Metanoia Society apps, aligns with owned domains: metanoiasociety.com/net/org)
        - **Aurora.csproj configuration:** Release signing added with environment variable for password (`$(AURORA_KEYSTORE_PASSWORD)`), AAB format enforced
        - **Build command:** `$env:AURORA_KEYSTORE_PASSWORD = "***"; dotnet publish -c Release -f net9.0-android`
        - **Output:** `com.metanoiasociety.aurora-Signed.aab` (30 MB) - 12,929 supported Android devices, API 21+, Target SDK 35
    - **AC 2:** ✅ Upload AAB to Google Play Console (validates signature, locks package name permanently)
    - **AC 3:** ✅ Add release notes: "Initial beta release for internal testing. Features: Daily Picks, Vibe of the Day, reactions, sharing"
    - **AC 4:** ✅ Release "1 (1.0.0)" created and rolled out to Internal Testing track
    - **AC 5:** ✅ Testers configured (developer email added)
    - **AC 6:** ✅ Opt-in URL generated (retrievable from Google Play Console → Internal Testing)
    - **AC 7:** ✅ Self-verification: Installed on S24 Ultra, app launches with correct name "Aurora" and proper icon
    - **AC 8:** ✅ Store listing configured: App name "Aurora - Uplifting Media", short/full descriptions, app icon (512x512px), feature graphic, screenshots uploaded
    - **AC 9:** ✅ Beta Tester Guide updated with opt-in page expectations (placeholder icon/package name normal for internal testing)
    - **Edge Cases:**
        - **Upload rejected by Console:** Common reasons: improper signing (keystore mismatch), version code conflict (already uploaded this version), missing required permissions in AndroidManifest.xml. Console provides specific error; resolve and re-upload.
        - Opt-in URL doesn't activate: Verify email added to Internal Testing list; propagation takes ~5 minutes. If still broken after 15 min, remove and re-add email.
        - App crashes on install: Check Android Studio Logcat for stack trace. Likely causes: signing config mismatch (Debug vs Release), missing dependencies in Release build, ProGuard obfuscation issues (disable for beta).
        - First install delayed: Google's automated security scan (Play Protect) can take 10-30 minutes before install link activates; show "Pending publication" during this time.
    - **Technical Considerations:**
        - **Version Code:** Ensure `<ApplicationVersion>1.0.0</ApplicationVersion>` in `Aurora.csproj` is set correctly; increment to `1.0.1` for second upload (Google rejects duplicate versions)
        - **Permissions:** Verify `AndroidManifest.xml` includes `<uses-permission android:name="android.permission.INTERNET" />` (already present from V-0)
        - **Signing:** MAUI auto-generates debug keystore for local builds; Release requires explicit keystore configuration in project properties or CLI
        - **AAB benefits:** Smaller download size than APK (Google's dynamic delivery), required for apps >150MB, supports Play Feature Delivery
    - **Time Estimate:** 1-2 hours (includes first-time AAB generation learning curve + Console navigation)

---

### Feature V-2.4: Self-Validation & External Tester Execution
*This feature validates Aurora's core value proposition through personal use and small cohort feedback before broader distribution.*

- [ ] **Story V-2.4.1:** As a project owner, I want to use Aurora daily for 1-2 weeks so that I can validate the value proposition from a user's perspective and identify friction points before finalizing external tester recruitment.
    - **AC 1:** Install Aurora on S24 Ultra via Google Play Internal Testing opt-in link (professional install, not sideloaded APK)
    - **AC 2:** Complete baseline survey (Story V-2.1.1) using own responses
    - **AC 3:** Use Aurora for **7-14 consecutive days** (minimum 7 days required, extend to 14 if availability permits or insights are unclear after Week 1)
    - **AC 4:** Track usage via **end-of-week recap** (not daily logging) with the following data points:
        - [ ] "How many days this week did I open Aurora?" (count)
        - [ ] "How many times was Aurora the first app I opened?" (count out of total phone pickups - estimate acceptable)
        - [ ] "How many articles did I read in full?" (count)
        - [ ] "Which stories resonated most?" (list titles + 1-sentence why)
        - [ ] "Did I share any stories?" (Yes/No + platform if yes: Discord, Reddit, etc.)
        - [ ] "UX moments that felt good" (specific examples: "Chrome Custom Tabs loaded smoothly," "Uplift animation was satisfying")
        - [ ] "UX moments that felt bad/frustrating" (specific examples: "Button too small on S24," "Snippet text too long")
    - **AC 5:** Submit weekly feedback survey (Story V-2.1.2) at end of Week 1 using own responses
    - **AC 6:** If testing extends to Week 2, submit second weekly survey at end of Week 2
    - **AC 7:** Document findings in `docs/beta/BETA_ROUND_1_SELF_VALIDATION.md` using provided template (see below)
    - **AC 8:** **Success Criteria Met:** "I used Aurora more than half of the tested days (≥4 days in Week 1, or ≥8 days over 2 weeks)" AND "Majority of weekly survey responses indicate positive sentiment (More hopeful, Informed, or similar)"
    - **Edge Cases:**
        - Miss logging for a day: End-of-week recap format allows estimation; doesn't invalidate test if ≥5 days have data
        - Zero usage some days: Valid data point; document reason in recap ("Too busy," "Forgot," "Didn't feel like it" - all useful insights)
        - No stories resonate: **Critical finding**; indicates content curation needs major pivot (document in findings as blocker)
        - Extend testing beyond 14 days: Allowed if decision is unclear; document rationale in findings
    - **Tracking Template (docs/beta/BETA_ROUND_1_SELF_VALIDATION.md):**
        ```markdown
        # Beta Round 1 - Self-Validation Log

        ## Testing Period
        - **Start Date:** [YYYY-MM-DD]
        - **End Date:** [YYYY-MM-DD]
        - **Total Days:** [7-14]

        ## Week 1 Recap: [Date Range]

        ### Usage Summary
        - **Days opened:** [X/7]
        - **First app opened:** [X times]
        - **Articles read:** [X total]
        - **Stories shared:** [Yes/No - Platform if yes]

        ### Resonant Stories
        1. **[Story Title]** - Why: [emotional impact, credibility, shareability]
        2. ...

        ### Good UX Moments
        - [Specific example: "READ button opened Chrome Custom Tab smoothly; back button returned to Aurora correctly"]
        - ...

        ### Bad UX Moments / Friction Points
        - [Specific example: "Uplift button too small on S24 Ultra; required precision tap"]
        - ...

        ### Weekly Survey Response
        [Copy-paste your survey answers here for reference]

        ## Week 2 Recap: [Date Range] (if applicable)
        [Same structure as Week 1]

        ## Overall Impression
        [2-3 paragraphs answering:]
        - Did Aurora make me feel more hopeful about humanity?
        - Would I continue using it after this test?
        - Would I recommend it to a friend? Why or why not?

        ## Success Criteria Met?
        - [ ] Used app ≥50% of tested days (≥4/7 or ≥8/14)
        - [ ] Majority positive sentiment in weekly survey(s)
        ```
    - **Time Estimate:** 7-14 days (actual testing) + 1 hour (documentation)

- [ ] **Story V-2.4.2:** As a content curator, I want to update Aurora's content at least once during self-validation so that I can measure curation effort and ensure fresh stories for ongoing testing.
    - **AC 1:** Identify 1 Vibe of the Day + 10 Daily Picks from real uplifting news sources (credible outlets, published within last 7-14 days)
    - **AC 2:** Create new `content.json` using `New-ContentTemplate.ps1` as starting point: `.\New-ContentTemplate.ps1 -VibeCount 1 -PicksCount 10 -OutputFile .\beta-round-1-update.json`
    - **AC 3:** Populate template with real stories: titles, 2-3 sentence snippets, article URLs, image URLs (hotlinked or placeholder), published dates
    - **AC 4:** Validate using `Validate-Content.ps1`: `.\Validate-Content.ps1 -FilePath .\beta-round-1-update.json` (must return zero errors, warnings acceptable)
    - **AC 5:** Deploy to Azure Blob Storage: `.\Deploy-Content.ps1 -FilePath .\beta-round-1-update.json -Environment Production`
    - **AC 6:** Verify deployment: Open Aurora on S24 Ultra, pull to refresh (if implemented) or force-stop and reopen app; confirm new stories appear
    - **AC 7:** **Track total curation time:** Start timer at "begin searching for stories," stop at "deployment verified." Document time in format: "X minutes for 11 stories (1 Vibe + 10 Picks)"
    - **AC 8:** **Success Threshold:** Total curation time **≤45 minutes** (per PLANNING.md sustainability criteria). If exceeds 45 min, document why and identify bottlenecks.
    - **AC 9:** Document curation experience in `docs/beta/BETA_ROUND_1_SELF_VALIDATION.md` under new section: "Content Curation Experience"
    - **Edge Cases:**
        - **Curation exceeds 45 min:** Not a failure; document bottlenecks (finding credible sources took 20 min, writing snippets took 15 min, image search took 15 min). Identifies automation opportunities for future.
        - Deployment fails (blob upload error): Rollback using `Rollback-Content.ps1`, investigate error in Azure Portal (connection string, permissions), retry
        - New content doesn't appear in app: Verify blob upload succeeded in Azure Portal; clear app cache or reinstall; check API logs for blob read errors
        - Stories go stale (published >2 weeks ago): Acceptable for beta testing; prioritize credibility and uplift quality over recency
    - **Content Curation Template (add to BETA_ROUND_1_SELF_VALIDATION.md):**
        ```markdown
        ## Content Curation Experience

        ### Update #1: [Date]
        - **Total Time:** [X minutes]
        - **Breakdown (estimate):**
            - Story discovery/research: [X min]
            - Snippet writing: [X min]
            - Image sourcing: [X min]
            - Deployment/verification: [X min]

        ### Bottlenecks Identified
        - [e.g., "Finding credible sources for environmental news took 15 min; need RSS feed automation"]
        - ...

        ### Success Threshold Met?
        - [ ] Total time ≤45 minutes (sustainable for daily curation)
        - [ ] If exceeded, are bottlenecks addressable with tooling? (Yes/No + ideas)
        ```
    - **Time Estimate:** Variable (target ≤45 min, could be 30-60 min for first update)

- [ ] **Story V-2.4.3:** As a project owner, I want to synthesize self-validation findings and recruit 1-4 external testers so that I can validate Aurora's value proposition beyond personal bias and gather diverse perspectives.
    - **AC 1:** Create `docs/beta/BETA_ROUND_1_FINDINGS.md` with the following sections:
        - [ ] **Executive Summary:** 2-3 sentences answering: "Did Aurora deliver on its value proposition for me? Should we proceed with external testers?"
        - [ ] **Quantitative Data:** First-app-opened rate (X%), days used (X/7 or X/14), articles read (total), stories shared (count), curation time (X minutes)
        - [ ] **Qualitative Insights:** Stories that resonated (themes), stories that didn't (themes), emotional impact, habit formation potential
        - [ ] **Critical Bugs:** Issues that block external testing (must-fix before Round 1 expansion) - e.g., app crashes, content fails to load
        - [ ] **UX Friction Points:** Annoyances that degrade experience (should-fix before Round 2) - e.g., button sizing, text readability
        - [ ] **Feature Requests:** Nice-to-haves discovered during testing (could-fix post-beta) - e.g., dark mode improvements, search functionality
    - **AC 2:** Update `BACKLOG.md` with new stories for identified improvements, categorized as:
        - [ ] **Blocker (Beta Round 1 - Fix Before External Testers):** Critical bugs that prevent testing (e.g., app crashes on launch)
        - [ ] **High Priority (Beta Round 2):** Significant UX issues that degrade value prop (e.g., "Uplift button too small on large screens")
        - [ ] **Medium Priority (Post-Beta):** Enhancements for broader release (e.g., "Add pull-to-refresh for content updates")
        - [ ] **Low Priority (Future Backlog):** Nice-to-haves (e.g., "User accounts for cross-device sync")
    - **AC 3:** **Recruit 1-4 External Testers** (if self-validation was positive):
        - [ ] Identify 1-4 trusted contacts with Android devices (friends, family, online community members)
        - [ ] Send recruitment email with: Beta Tester Guide PDF, Baseline Survey link, Google Play opt-in URL, personal note explaining Aurora's purpose
        - [ ] Add tester emails to Google Play Console Internal Testing track
        - [ ] Verify testers successfully install app and complete baseline survey
        - [ ] Set expectation: "Use for 1 week, submit weekly survey on Sunday"
    - **AC 4:** Make **Go/No-Go Decision** documented in findings:
        - [ ] **Go:** Continue refining Aurora; implement blockers/high-priority fixes; proceed to Beta Round 2 with larger cohort
        - [ ] **No-Go:** Substantial pivot required; Aurora doesn't deliver value proposition as designed; decide whether to restart concept or salvage
    - **AC 5:** Document decision rationale in findings: "Why Go?" or "Why No-Go?" (specific data points and insights that drove decision)
    - **Go Decision Criteria (Thresholds):**
        - ✅ Opened Aurora first ≥50% of tested days (shows habit formation potential)
        - ✅ At least 3 stories resonated emotionally over testing period (content quality validated)
        - ✅ Weekly survey shows "More hopeful," "Informed," or similar positive sentiment (value prop validated)
        - ✅ Curation time ≤45 min/update OR bottlenecks identified with clear automation path (sustainability validated)
        - ✅ No critical bugs encountered (app stability validated)
    - **No-Go Decision Triggers (Pivot Signals):**
        - ❌ Never or rarely opened Aurora first (<30% of days) - no habit formation
        - ❌ No stories resonated; felt neutral or negative about content - content quality failure
        - ❌ Curation time consistently >60 min with no clear automation path - unsustainable
        - ❌ Critical bugs made app frequently unusable - stability failure
    - **Edge Cases:**
        - **Mixed results:** Some criteria pass, some fail. Document as "Conditional Go" with specific blockers that must be addressed before Round 2. Example: "Content resonated, but curation time was 55 min; automate RSS parsing before Round 2."
        - **No strong signal:** Neutral data (neither great nor terrible). Recommendation: Recruit 1-2 external testers for additional perspectives; extend testing to Week 2 if only tested 7 days.
        - **Cannot recruit external testers:** Acceptable to proceed with self-only if self-validation was strongly positive; document as limitation ("Round 1 limited to self-testing due to tester availability; will expand in Round 2")
        - **All testers decline or ghost:** Valid finding; may indicate low perceived value. Document in No-Go rationale: "Could not recruit testers; suggests concept lacks initial appeal."
    - **External Tester Recruitment Email Template:**
        ```
        Subject: Help me test Aurora - a refuge from negative news

        Hi [Name],

        I'm working on a mobile app called Aurora that curates uplifting news stories - a refuge from the fear-mongering and negativity that dominates most media. I'd love your help testing it for 1-2 weeks.

        **What I'm asking:**
        - Install Aurora on your Android phone (via Google Play link)
        - Use it for 1 week (~5 min/day)
        - Fill out a quick weekly survey (5 min)

        **What you get:**
        - Early access to something genuinely positive
        - A say in shaping the app before broader release
        - Daily dose of hope for humanity 😊

        Interested? Here's what to do:
        1. Fill out this baseline survey: [LINK]
        2. Install Aurora: [GOOGLE PLAY OPT-IN LINK]
        3. Open the app and explore!

        Full guide attached. Let me know if you have any questions!

        Thanks,
        [Your Name]
        ```
    - **Time Estimate:** 3-4 hours (write findings, update backlog, recruit testers)