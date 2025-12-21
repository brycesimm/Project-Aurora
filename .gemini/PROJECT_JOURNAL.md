# Project Aurora - Development Journal

This journal documents key discussions, decisions, and progress throughout the development of Project Aurora.

---

## 2025-11-17: Project Inception

### Initial Consultation

**Developer Background:**
- ~4 years professional experience with .NET applications
- Beginner-level mobile development (some Android learning experience)
- Seeking to build first production mobile application

**Application Concepts Evaluated:**

1. **Positive News Application** ✅ SELECTED
   - Exclusively focuses on upbeat and positive media
   - Addresses user fatigue with fear-mongering and divisive traditional media
   - Could be social media or news app format
   - Identified challenges: content sourcing, defining "positivity," moderation

2. **Dating Application** ❌ REJECTED
   - Focus on transparency and long-term relationships
   - Addresses issues with existing apps (hookup culture, questionable algorithms)
   - Rejected due to: insurmountable network effect problem, high user acquisition costs, complex trust & safety infrastructure, technical complexity

**Decision Rationale:**
The positive news app offers:
- Achievable MVP without network effect dependency
- Iterative validation possibility
- Value from day one, even with single user
- Alignment with .NET expertise
- Clearer path to differentiation

### Naming & Branding Discussion

**Project Codename Selected:** Aurora
- Roman goddess of dawn (Greek: Eos)
- Symbolizes opening gates of heaven for the sun to rise
- Metaphor: Opening gates to brighter perspective each day

**Public Brand Name:** TBD
- "Happybara" considered (capybara wordplay) - charming but potentially too cutesy
- "Eunoia" considered (Greek: beautiful thinking) - elegant but phonetically similar to "annoying"
- Target audience: General (PG-13+)
- Decision deferred for further consideration

### Strategic Planning Framework Established

Created 5-phase planning approach to avoid premature technical decisions:

1. **Vision & Core Definition** - Define problem, users, value proposition, success metrics
2. **Feature Prioritization** - Determine MVP scope, differentiation factors
3. **Technical Foundation Research** - Choose technology stack, assess learning requirements
4. **Development Roadmap** - Create milestones, timelines, risk assessment
5. **Validation Strategy** - Define metrics, testing approach, pivot criteria

**Current Status:** Beginning Phase 1

### Technology Stack Considerations

**Discussed Options:**
- **.NET MAUI** - Leverages existing C# expertise, cross-platform
- **Flutter** - Excellent performance, large community, requires learning Dart
- **React Native** - Massive ecosystem, requires JavaScript/TypeScript

**Decision:** Deferred until Phase 3 (after core features are defined)

### Next Steps

1. Complete Phase 1: Vision & Core Definition
   - Articulate specific problem being solved
   - Define target user with concrete details
   - Establish core value proposition
   - Set success criteria for 6 months and 1 year

2. Document findings in PLANNING.md

---

**Session Duration:** ~45 minutes
**Key Documents Created:** PLANNING.md, .claude/PROJECT_JOURNAL.md
**Developer Sentiment:** Engaged, thoughtful, preferring structured approach over rushing into development

## 2025-11-23: Planning Deep Dive

### Key Updates
- Completed Phase 1 answers inside .codex/PLANNING.md (problem, persona, value proposition, 6/12-month goals).
- Defined Phase 2 MVP scope, Vibe of the Day differentiator, and staged future features.
- Locked .NET MAUI as primary stack with detailed onboarding checklist; outlined Azure-based backend approach.
- Built full Phase 4 roadmap (Milestones A-D) with actionable tasks and success criteria.
- Refined Phase 5 validation strategy to focus on qualitative feedback, lightweight metrics, smaller beta cohort, and pragmatic pivot rules.

### Next Focus
1. Execute Milestone A (MAUI environment setup + shell prototype) and log findings.
2. Start drafting Azure Function schema for Daily Picks once MAUI shell stabilizes.
---

## 2025-12-06: Environment Debugging & Workflow Refinement

### Summary
- Successfully completed and merged **Story A-01.1 (Basic Application Shell)**.
- Refined development workflow:
    - Adopted a story-per-branch strategy.
    - Formalized use of `BACKLOG.md` with Acceptance Criteria.
    - Established a collaborative, step-by-step development pace.
- Resolved a persistent build cache corruption issue in the .NET MAUI environment. The solution required manually deleting the hidden `.vs` folder to force a true clean build.
- Documented the caching issue and its solution in `DEBUGGING_NOTES.md` for future reference.

### Next Focus
1. Resume implementation of **Story A-01.2: Vibe of the Day hero card**.
---

## 2025-12-06 (Continued): Story A-01.2 Completion & XAML Deep Dive

### Summary
- Successfully completed **Story A-01.2 (Vibe of the Day hero card)**.
    - Implemented the card's visual container (`Frame`).
    - Added a `VerticalStackLayout` for internal content arrangement.
    - Included a title `Label`, an `Image` placeholder, and a descriptive text `Label`.
- Deep dive into XAML UI controls and properties:
    - Explained `Frame` and `VerticalStackLayout` concepts and usage.
    - Detailed `Image` properties: `Source`, `Aspect` (`AspectFill`), `HeightRequest`, `HorizontalOptions` (`FillAndExpand`).
    - Clarified `AppThemeBinding` and "adaptive TextColor" for light/dark mode compatibility.

### Next Focus
1. Begin implementation of **Story A-01.3: Daily Picks List**.
---

## 2025-12-07: Story A-01.3 Completion - Daily Picks List

### Summary
This session was dedicated to the implementation of **Story A-01.3: Daily Picks List**.

Key accomplishments include:
1.  **Workflow Refinement:** Established a new feature branch, `story/A-01.3-daily-picks-list`, to ensure proper version control and work isolation.
2.  **Data Modeling:** Created the `DailyPick` data model, defining the structure for news items with `required` properties for `Title` and `Snippet`.
3.  **UI Implementation:** Successfully implemented a data-bound `CollectionView` in the main page's XAML to display a scrollable list of mock news stories, fulfilling all acceptance criteria.
4.  **Code Refinement:** Addressed and resolved C# compiler warnings by adopting the `required` keyword for better code quality and null-safety.
5.  **Documentation & Commit:** Updated `BACKLOG.md` and `PROJECT_JOURNAL.md` to reflect the story's completion and committed the finalized feature to the repository with a detailed message.

The `story/A-01.3-daily-picks-list` branch now contains a complete and verified implementation of the feature.

### Next Focus
1. Begin implementation of **Story A-01.4: Placeholder Reaction Button**.
---

## 2025-12-07 (Continued): Story A-01.4 Completion - Reaction Button & Font Icons

### Summary
This session was dedicated to the implementation of **Story A-01.4: Placeholder Reaction Button**. The process involved a significant amount of debugging and refinement to ensure a robust implementation using font icons, which is a best practice for scalable and theme-able icons.

Key accomplishments include:
1.  **Font Icon Integration:** The Material Design Icons font (`.ttf`) was successfully downloaded, added to the project (`Resources/Fonts`), and registered in `MauiProgram.cs` under the alias "MDI".
2.  **Button Implementation:** A reaction `Button` was added to `MainPage.xaml`, with its icon rendered using a `FontImageSource`.
3.  **Extensive Debugging:**
    *   Resolved an issue where the icon was not appearing by testing with a known-good "home" icon, which revealed the problem was related to color and a specific icon glyph.
    *   Diagnosed and fixed a color contrast issue by analyzing `Colors.xaml` and `Styles.xaml` and then setting an explicit, high-contrast color on the `FontImageSource`.
    *   After determining the initial solid "heart" glyph was problematic, the implementation was successfully updated to use the `heart-outline` glyph (`F02D5`).
4.  **Version Control:** All changes were committed to the `story/A-01.4-reaction-button` feature branch and pushed to the remote repository.

The story is now functionally complete, with the button and a visible `heart-outline` icon appearing correctly and firing a click event as expected.

### Next Focus
1. Merge the `story/A-01.4-reaction-button` branch into `main`.
2. Begin planning for **Milestone B: Backend/API + curation workflow**.
---

## 2025-12-07 (Session 2): Milestone B Planning & Strategy

### Summary
This session focused on the collaborative planning for **Milestone B: Backend/API + Curation Workflow** after confirming the successful completion of all Milestone A work.

Key activities and decisions include:
1.  **Milestone A Completion:** Confirmed that all stories for Milestone A were complete, merged, and that the `main` branch was up-to-date.
2.  **Branching Strategy:** Established a `feature/milestone-b-planning` branch to handle the collaborative definition of the next milestone's scope without impacting the main branch.
3.  **Collaborative Story Definition:** Mutually agreed upon and defined the initial user stories for **Feature B-01: Content Structure & API Definition**. These stories cover the creation of a formal JSON schema for content and a local API endpoint prototype.
4.  **Backend Strategy Clarification:** Addressed and clarified the strategic choice of Azure for the backend. Discussed the cost implications and confirmed that the intended use of Azure Functions (Consumption Plan) and Azure Blob Storage would be either free or extremely low-cost for the MVP, alleviating cost concerns.
5.  **Documentation:** The defined stories for Milestone B were captured in `BACKLOG.md` on the planning branch, pending merge.

This session established a clear, mutually-understood technical foundation and a set of actionable tasks for the commencement of Milestone B development.

### Next Focus
1. Merge the `feature/milestone-b-planning` branch.
2. Create a new development branch for **Story B-01.1: Define the Content Schema**.
---

## 2025-12-07 (Session 3): Story B-01.1 Completion - Data Contract Definition

### Summary
This session was dedicated to the implementation of **Story B-01.1: Define the Content Schema**, establishing the foundational data contract for the entire application.

Key accomplishments include:
1.  **Schema Definition:** A formal `content.schema.json` file was created, defining the structure for the API's response, including the `VibeOfTheDay` and a list of `DailyPicks`.
2.  **Schema Refactoring:** Based on maintainability feedback, the schema was immediately refactored to use a reusable `$defs` section for a canonical `contentItem`. This makes future changes easier and less error-prone.
3.  **Model Synchronization:** The internal C# data model was synchronized with the new schema. This involved renaming `DailyPick.cs` to `ContentItem.cs`, updating its properties to match the schema (`Id`, `ImageUrl`, `ArticleUrl`, etc.), and updating the mock data in the UI to use the new model.
4.  **Workflow Improvement:** Identified a potential for drift between the C# models and the JSON schema. A new story (`B-02.1`) was added to the backlog to create a tool that automatically generates the schema from the C# code, making the models the single source of truth.

The completion of this story provides a robust and well-defined data contract that will guide all future backend and front-end development for Milestone B.

### Next Focus
1.  Begin implementation of **Story B-02.1: Implement Automated JSON Schema Generation**.
2.  Upon completion of B-02.1, return to **Story B-01.2: Local, HTTP-triggered function for mock content**.
---

## 2025-12-07 (Session 4): Swagger/OpenAPI Integration & Schema Export Automation Planning

### Summary
This session primarily tackled Swagger/OpenAPI integration and a persistent `TypeLoadException` caused by `Microsoft.OpenApi` version conflicts, leading to a successful resolution and a refined plan for automated schema export.

Key accomplishments and resolutions include:

1.  **Swagger/OpenAPI Integration in `SchemaBuilder`:**
    *   Successfully added `Swashbuckle.AspNetCore` and `Microsoft.AspNetCore.OpenApi` NuGet packages.
    *   Configured Swashbuckle within `src/SchemaBuilder/Program.cs` to enable API exploration, Swagger generation, and Swagger UI.
    *   Introduced a minimal API endpoint `/schema/contentitem` to expose the `ContentItem` model for schema generation, ensuring `required` properties were correctly initialized.
    *   Removed the default `WeatherForecast` endpoint to streamline the project.

2.  **Resolution of `TypeLoadException` (Dependency Hell):**
    *   Diagnosed a `System.TypeLoadException` relating to `Microsoft.OpenApi, Version=2.3.0.0` as a transitive dependency conflict.
    *   After initial attempts to resolve by adding `Microsoft.AspNetCore.OpenApi` using directive and explicitly forcing `Microsoft.OpenApi` version `2.3.0`, the issue was ultimately resolved by downgrading `Swashbuckle.AspNetCore` to version `6.5.0`. This aligned its transitive `Microsoft.OpenApi` dependency with the `1.6.x` version, which was compatible with the existing `.NET 9` environment and `Microsoft.AspNetCore.OpenApi` 9.0.0, thus eliminating the `TypeLoadException`.
    *   The successful resolution was verified by the Swagger UI loading correctly in the browser.

3.  **Schema Export & Backlog Refinement:**
    *   Successfully fetched the Swagger JSON definition from the running `SchemaBuilder` application using `Invoke-WebRequest`.
    *   Extracted the `ContentItem` schema from the Swagger JSON and wrote it to `content.schema.json`, effectively completing the manual aspect of **Story B-02.1**.
    *   The initial plan for **Story B-02.1: Implement Automated JSON Schema Generation** was refined and expanded into new, more granular stories under **Feature B-02: Automated JSON Schema Generation**:
        *   **Story B-02.2: Implement Console Application for Schema Export.**
        *   **Story B-02.3: Integrate Schema Export into Build Process.**
    *   An erroneous attempt to create the `SchemaExporter` project prematurely was fully reverted, ensuring the project's state remains consistent with the current branch and backlog.

### Next Focus
1.  Implement **Story B-02.3: Integrate Schema Export into Build Process** to ensure `content.schema.json` is always up-to-date.
2.  Begin implementation of **Story B-01.2: Local, HTTP-triggered function for mock content**.
---

## 2025-12-12: Story B-02.3 Completion - Integrate Schema Export into Build Process

### Summary
This session was dedicated to the implementation of **Story B-02.3: Integrate Schema Export into Build Process**. The goal was to automate the generation of `content.schema.json` whenever the `SchemaBuilder` project is built.

Key accomplishments and resolutions include:

1.  **PostBuildEvent Implementation:**
    *   Added a `Target` element with a `PostBuildEvent` to the `SchemaBuilder.csproj` file.
    *   The `PostBuildEvent` was configured to execute the `SchemaBuilder` application using `dotnet $(TargetPath)`. This ensures the `SchemaBuilder` executable is run after a successful build, triggering the schema generation.

2.  **Debugging and Resolution:**
    *   Initial build attempts failed because the `PostBuildEvent` was attempting to execute the `.dll` directly.
    *   Resolved this by prefixing the command with `dotnet`, correctly invoking the .NET runtime to execute the assembly.

3.  **Verification:**
    *   A full solution build was executed, which completed successfully.
    *   The `content.schema.json` file was verified to be updated with the latest schema, confirming the automation is working as expected.

This story significantly improves the developer workflow by ensuring the `content.schema.json` is always synchronized with the C# `ContentItem` model, preventing potential data contract drift between front-end and back-end development.

### Next Focus
1. Begin implementation of **Story B-01.2: Local, HTTP-triggered function for mock content**.
---

## 2025-12-12: Story B-01.2 Completion - Local, HTTP-triggered function for mock content

### Summary
This session focused on the implementation of **Story B-01.2: Local, HTTP-triggered function for mock content**. The objective was to create a local Azure Functions endpoint that serves the `sample.content.json` file, enabling the front-end application to consume mock data via an API.

Key accomplishments and resolutions include:

1.  **Azure Functions Project Creation:** A new Azure Functions project, `Aurora.Api`, was created within the solution and added to `Project-Aurora.sln`.
2.  **Function Implementation:** An HTTP-triggered function named `GetDailyContent` was implemented. This function reads the `sample.content.json` file and returns its content with a `200 OK` status and `application/json` content type. The function was configured for `Anonymous` authorization and to respond only to `GET` requests.
3.  **Pathing Refinement:** The initial pathing logic for `sample.content.json` was refactored. The `Aurora.Api.csproj` was updated to include `sample.content.json` as a content file, ensuring it is copied to the output directory. The `GetDailyContent` function's code was then simplified to read the file from the executing assembly's directory.
4.  **Azure Functions Core Tools Installation & Troubleshooting:**
    *   Encountered an error indicating the absence of Azure Functions Core Tools.
    *   Used `winget` to successfully install `Microsoft.Azure.FunctionsCoreTools`.
    *   Resolved a subsequent issue where `dotnet run` was unable to launch the Core Tools by obtaining the absolute path to `func.exe` and running the host directly. This confirmed the Core Tools were installed and functional.
5.  **Verification:** The function was successfully tested by accessing `http://localhost:7071/api/GetDailyContent` in a web browser, which returned the expected JSON content from `sample.content.json`.

This story significantly advances the backend development by providing a functional local API endpoint for front-end integration and lays the groundwork for future cloud-based API deployment.

### Next Focus
The next step is to integrate the mobile application (Project Aurora) with this newly created local API endpoint.

---

## 2025-12-12 (Session 2): Milestone C Planning

### Summary
With all stories for Milestone B completed, this session was dedicated to planning the next phase of development: **Milestone C: Live Data Integration & Core Features**.

Key activities include:
1.  **Milestone Review:** Confirmed the completion of Milestones A and B, noting that the `BACKLOG.md` was clear of any pending tasks.
2.  **Backlog Population:** Defined and documented the initial set of user stories for **Feature C-01: Data Service Integration**. These stories cover the creation of data models, a service interface (`IContentService`), a concrete service implementation to fetch data from the local API, registration with the DI container, and the final consumption of the service in the UI.
3.  **Documentation:** Updated `BACKLOG.md` on the `planning/backlog-updates` branch with the new stories for Milestone C.

This session establishes a clear, actionable plan for connecting the MAUI front-end to the newly created backend API.

### Next Focus
1.  Merge the `planning/backlog-updates` branch.
2.  Begin implementation of **Story C-01.1: Define Content Data Models and Service Interface**.

---

## 2025-12-12 (Session 3): Story C-01.1 Completion - Data Models & Service Interface

### Summary
This session focused on implementing **Story C-01.1: Define Content Data Models and Service Interface**.

Key accomplishments include:
1.  **Branch Creation:** Started a new feature branch `feature/C-01.1-content-models-interface`.
2.  **Model & Interface Definition:**
    *   Created `ContentFeed.cs` in `src/Aurora.Shared/Models`.
    *   Created `IContentService.cs` in `src/Aurora.Shared/Interfaces`.
3.  **Refactoring & Cleanup:**
    *   Adopted file-scoped namespaces for both new files and updated `ContentItem.cs` to match, ensuring a cleaner, consistent code style across `Aurora.Shared`.
    *   Moved `IContentService` to a dedicated `Interfaces` directory for better project organization.
4.  **Verification:** Successfully built the `Aurora.Shared` project to ensure no syntax errors or namespace conflicts were introduced.

### Next Focus
1.  Merge `feature/C-01.1-content-models-interface` into `main`.
2.  Begin **Story C-01.2: Concrete Content Service Implementation**.

---

## 2025-12-12 (Session 4): Story C-01.2 Completion - Content Service Implementation

### Summary
This session focused on implementing **Story C-01.2: Concrete Content Service Implementation**, including essential refactoring for configuration management and future-proofing.

Key accomplishments include:
1.  **Branch Creation:** Created `feature/C-01.2-content-service-impl`.
2.  **ContentService Implementation:**
    *   Created `src/Aurora/Services/ContentService.cs` implementing `IContentService`.
    *   It now relies on an `HttpClient` whose `BaseAddress` is configured via DI.
3.  **Configuration Management:**
    *   Added `Microsoft.Extensions.Configuration.Json` and `Microsoft.Extensions.Http` NuGet packages to `Aurora.csproj`.
    *   Created and embedded `src/Aurora/appsettings.json` with the API Base URL.
    *   Updated `MauiProgram.cs` to load `appsettings.json` and configure the `MauiAppBuilder` with it.
4.  **HttpClient Configuration (Typed Client Pattern):**
    *   Implemented the Typed Client pattern by registering `IContentService` and `ContentService` with DI in `MauiProgram.cs`.
    *   The `HttpClient`'s `BaseAddress` is set from `appsettings.json` during registration.
    *   Refactored the URL usage in `ContentService.cs` to use the Base URL + relative path pattern for clarity and future-proofing.
5.  **Verification:** Successfully built the `Aurora` project after all changes.

### Next Focus (Deferred)
*Initial plan was to create unit tests for `ContentService`, but this was deferred.*

### Decision to Defer Unit Testing:
During the attempt to create a unit test project (`Aurora.Tests`) for `ContentService`, a target framework incompatibility issue arose due to `Aurora.csproj` being a MAUI application. To properly unit test the service, significant refactoring (moving `ContentService` to a new shared library project) would be required. Given token constraints and the user's preference to prioritize core feature development, the unit testing effort for this story has been *deferred*.

A new milestone/feature will be proposed to address robust testing strategy and architecture for service testability. The current verification of `ContentService`'s functionality will occur during the implementation of **Story C-01.4** (UI integration).

---

## 2025-12-21: Milestone C - Live Data Integration

### Summary
This session successfully completed **Story C-01.4: UI Integration of Live Data**, marking a pivotal transition from mock data to a functional, data-driven application.

Key accomplishments include:
1.  **UI Integration & Refactor:** 
    - Connected `MainPage` to `ContentService` to fetch live data from the local API.
    - Refactored the main UI to use `CollectionView.Header` instead of nested ScrollViews, resolving a critical layout collapse issue and ensuring smooth scrolling performance.
2.  **Android Networking Fixes:**
    - Diagnosed and resolved the "Android vs. Localhost" loopback issue by remapping API calls to `10.0.2.2` specifically for Android devices in `MauiProgram.cs`.
    - Enabled cleartext HTTP traffic in `AndroidManifest.xml` to allow the emulator to communicate with the local development server (non-SSL).
3.  **Data Contract Hardening:**
    - Updated the `ContentItem` model with `[JsonPropertyName]` attributes to handle snake_case JSON keys from the API, preventing silent deserialization failures.
    - Configured the API project to use a fixed port (7071) to match the client configuration reliably.
4.  **Documentation Update:** formally marked **Story C-01.3** as complete in the backlog.

The application now successfully fetches and displays the "Vibe of the Day" and "Daily Picks" from the local Azure Function on both Windows and Android emulators.

### Next Focus
1.  Merge the `feature/C-01.4-ui-integration` branch.
2.  Begin **Milestone D: Robust Testing Framework & Service Testability**, starting with **Story D-01.1: Service Testability Refactor**.