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

- [ ] **Story V-0.4:** As a developer, I want real uplifting news stories in the application so that beta testers can evaluate actual content quality.
    - **AC 1:** Initial content file contains 1 Vibe of the Day + 5-10 Daily Picks with real, recently published uplifting news articles, valid ArticleUrls, appropriate ImageUrls, and 2-3 sentence snippets.
    - **AC 2:** Content sources are credible (r/UpliftingNews, Good News Network, Positive News, etc.); no broken links, no paywalled content.
    - **AC 3:** Image handling: Use article's Open Graph/featured image URL where available (hotlinking); fallback to placeholder image (`https://via.placeholder.com/800x600/7986CB/FFFFFF?text=Aurora`) if hotlinking not allowed or image unavailable.
    - **AC 4:** Content.json is uploaded to `aurora-content` blob container; GetDailyContent endpoint serves the real content.
    - **AC 5:** Verified on S24 Ultra: all stories display correctly, READ buttons open articles, Uplift reactions work.
    - **Edge Cases:**
        - Broken links after publication are acceptable risk for beta (document as known issue).
        - Image licensing: use Creative Commons or Open Graph images (fair use for preview); document for production review.
        - Content curation is collaborative: both user and assistant find/format articles.

---

### Feature V-02: Content Management Tooling (Phase 2)
*Automation and validation scripts to streamline content curation workflow after Phase 1 verification.*

**Note:** These stories are deferred until after V-0.1 through V-0.4 are complete and tested.

- [ ] **Story V-0.5:** As a content curator, I want automated validation of my content JSON so that I don't accidentally break the app with malformed data.
    - **AC 1:** PowerShell script `Validate-Content.ps1` exists in `tools/content-management/` directory; run with `.\Validate-Content.ps1 -FilePath .\content.json`.
    - **AC 2:** Script validates JSON against `content.schema.json`: checks syntax, validates required fields, validates UpliftCount is non-negative integer, validates URLs are well-formed (http/https).
    - **AC 3:** Clear error messages: Success: "✓ content.json is valid and ready to upload"; Failure examples: "✗ Error on line 15: Missing required field 'title' in DailyPicks[2]" or "✗ Error: 'article_url' must be a valid URL (got: 'not-a-url')".
    - **AC 4:** Optional image URL accessibility check: attempt HEAD request to each ImageUrl; warn (don't fail) if 404 or timeout (e.g., "⚠ Warning: Image URL for 'Coral Reef Story' returned 404").
    - **AC 5:** README.md in `tools/content-management/` documents script usage, dependencies, and result interpretation.
    - **Edge Cases:**
        - JSON schema validation works offline; image URL checks skip with warning if offline.
        - Optional quality validation: warn if snippet >200 chars (too long) or <50 chars (too short).

- [ ] **Story V-0.6:** As a content curator, I want a one-command deployment process so that I can publish content updates quickly and reliably.
    - **AC 1:** PowerShell script `Deploy-Content.ps1` exists in `tools/content-management/`; run with `.\Deploy-Content.ps1 -FilePath .\content.json -Environment Production`.
    - **AC 2:** Script performs pre-deployment validation: automatically runs `Validate-Content.ps1`; aborts if validation fails; requires confirmation if warnings exist (e.g., "Image URL returned 404. Continue? [Y/N]").
    - **AC 3:** Script uploads to Azure Blob Storage using Azure CLI (`az storage blob upload`); overwrites existing file; displays "✓ Content deployed to Production at 2025-12-26 14:32:15".
    - **AC 4:** Supports multiple environments: `-Environment Dev` vs. `-Environment Production`; defaults to Dev for safety.
    - **AC 5:** Rollback capability: before upload, downloads current `content.json` and saves as `content.backup.YYYY-MM-DD-HHMMSS.json`; provides `.\Rollback-Content.ps1` to restore; keeps last 5 backups.
    - **AC 6:** README.md documents full workflow: Edit → Validate → Deploy → Verify → Rollback (if needed).
    - **Edge Cases:**
        - Script checks for `az` command; provides install instructions if missing.
        - Uses `az login` for authentication (user authenticates once, credentials cached).
        - Azure Blob upload is atomic (all or nothing); if upload fails, old content remains active.

- [ ] **Story V-0.7:** As a content curator, I want a template generator for content.json so that I can quickly create properly formatted content files.
    - **AC 1:** PowerShell script `New-ContentTemplate.ps1` exists in `tools/content-management/`; run with `.\New-ContentTemplate.ps1 -VibeCount 1 -PicksCount 7 -OutputFile .\my-content.json`.
    - **AC 2:** Generated template includes all required fields with helpful placeholders: `"id": "REPLACE_WITH_UNIQUE_ID_1"`, `"title": "TODO: Article Title Here"`, `"snippet": "TODO: 2-3 sentence summary"`, `"article_url": "https://example.com/article"`, `"image_url": "https://placeholder.com/800x600"`, `"published_date": "2025-12-26"` (auto-filled with today), `"uplift_count": 0`.
    - **AC 3:** Includes helpful comments or README explaining each field (e.g., `"id": "unique-kebab-case-identifier (e.g., 'coral-reef-restoration-2025')"`).
    - **AC 4:** Supports bulk creation: `-VibeCount 1 -PicksCount 10` creates 11 items; IDs auto-numbered: `vibe-of-the-day-1`, `daily-pick-1`, `daily-pick-2`, etc.
    - **AC 5:** Generated file passes validation syntax check (warnings for placeholder URLs are expected).
    - **Edge Cases:**
        - Use obvious placeholders (not real data) to force replacement.
        - Defer automatic metadata fetching (title, image from URL) to future enhancement.