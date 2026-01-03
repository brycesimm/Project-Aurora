# Project Aurora - Planning Framework

**Project Codename:** Aurora
**Mission:** A positive news application that provides refuge from negativity, fear-mongering, and divisive media
**Target Audience:** General audience (PG-13+)

---

## Milestone Naming Convention

Aurora uses a **phase-based naming scheme** for milestones:

- **A-E:** Phase 4 Development Roadmap milestones (initial MVP build)
  - Example: Milestone A (MAUI onboarding), Milestone B (Backend/API)
- **V-*:** Phase 5 Validation Strategy milestones (beta testing & user validation)
  - "V" prefix indicates Validation phase work
  - Example: V-0 (Beta Readiness), V-2 (Beta Testing Round 1)
  - Note: V-1 exists but was deferred, so V-2 came first chronologically
- **CT-*:** Content Tools milestones (content management infrastructure)
  - "CT" prefix indicates Content Tooling work
  - Example: CT-1 (PowerShell to .NET migration)

**Important:** Milestone numbers do NOT indicate execution order. Use the **Execution Priority** field to determine which milestone to work on next:
- 🔴 **NEXT UP** - Currently prioritized for immediate work
- 🟡 **QUEUED** - Planned for execution after higher-priority milestones
- 🟢 **QUEUED** - Lower priority, awaiting completion of earlier work
- ⚪ **DEFERRED** - Future enhancement, not currently scheduled

Completed milestones retain their original names for historical consistency. Future phases may introduce new prefixes as needed.

---

## Planning Phases

### Phase 1: Vision & Core Definition
**Status:** Completed (as of 2025-11-23)

#### Questions to Answer:
- [x] What specific problem are we solving...
  - Modern news/social feeds are engineered to maximize outrage, fear, and false narratives, exploiting dopamine-seeking users and causing avoidable emotional harm.
- [x] Who is our target user... (Demographics, behaviors, pain points)
  - PG-13+ audience, primarily US-based, who feel inundated by negative feeds and want a safe, family-friendly stream of stories that inspire productivity, positivity, and empathy.
- [x] What is the core value proposition in one sentence...
  - Aurora delivers curated inspiration and positivity so users can counterbalance fearmongering media and protect their mental health.
- [x] What does success look like in 6 months...
  - Working proof of concept, Android-tested mobile build, major conceptual hurdles mapped, and early user-sentiment interviews underway (no store launch yet).
- [x] What does success look like in 1 year...
  - App released to at least one store, value proposition demonstrated via polished functionality/design, and beginnings of marketing plus real user support workflows.

#### Findings:
- **Problem Statement:** Aurora serves as an oasis of credible optimism in a media desert dominated by sensational fear and anger.
- **Target Persona:** Time-constrained adults (13+) who crave hopeful stories to share with family/friends and want relief from doom-scrolling habits.
- **Value Proposition:** Curated feed that increases hope for humanity, contrasting negativity with trustworthy, uplifting narratives to sustain mental wellbeing.
- **6-Month Outcome:** Proof-of-concept app validates content model, exposes key hurdles, and provides qualitative sentiment data to steer MVP scope.
- **12-Month Outcome:** Public launch with clarified intent, marketing experiments, and support processes that prove Aurora’s promise in the real world.

---

### Phase 2: Feature Prioritization
**Status:** Completed (as of 2025-11-23)

#### Questions to Answer:
- [x] What is the absolute minimum feature set that delivers value...
  - Daily curated list (5-10 uplifting stories), succinct home feed, anonymous "uplifted me" reaction, native share sheet, and time-permitting push notification for the daily drop.
- [x] What can we ruthlessly cut for the MVP...
  - User accounts, comments, personalization, advanced categorization, analytics dashboards; reactions remain anonymous to avoid auth complexity.
- [x] What's the "wow" factor that differentiates us...
  - A highlighted "Vibe of the Day" card: one standout story (article, video, etc.) surfaced with premium placement, signaling Aurora's editorial voice and giving users a share-worthy focal point.
- [x] What features belong in Phase 2, 3, etc....
  - **Phase 2 extensions:** reliable push notifications, basic scheduling for daily digests.
  - **Phase 3+:** user accounts, personalization, category filters, user-submitted positivity queue, partnerships/content ingestion tooling, analytics dashboards, moderation workflows.

#### Decisions:
- Phase 2 MVP maximizes value-per-effort: double down on curation + delivery workflow, skip social/account features until engagement is validated.
- Push notifications are "nice-to-have" gated on available weekends; sharing is "must-have" because it amplifies reach without backend overhead.

### Phase 3: Technical Foundation Research
**Status:** Completed (as of 2025-11-23)

#### Questions to Answer:
- [x] .NET MAUI vs Flutter: Evaluation criteria
  - Learning curve (C# reuse vs Dart adoption), community/resources, native performance, tooling maturity, ease of integrating push notifications and sharing.
- [x] .NET MAUI vs Flutter: Final decision and rationale
  - Primary choice: .NET MAUI (leverages C# expertise, shared codebase, direct Android/iOS path); Flutter retained as contingency if MAUI tooling blocks progress.
- [x] What do we need to learn vs what we already know...
  - Need: MAUI UI paradigms (XAML/MVU), platform deployment workflows, store submission steps, mobile UX conventions.
  - Know: C#, async/REST patterns, general .NET tooling, Azure basics.
- [x] Backend requirements based on chosen features
  - Lightweight API for curated stories (initially static JSON or serverless API), anonymous reaction tally endpoint, push notification trigger job.
- [x] Infrastructure and hosting considerations
  - Start with managed cloud (Azure App Service or serverless functions) to minimize ops; plan for background job scheduler + blob storage/CDN if hosting media assets.
- [x] Third-party services needed (if any)
  - Firebase (FCM) for pushes, App Center/Analytics for telemetry, optional RSS helpers or newsletters for sourcing.

#### Decisions:
- Commit to .NET MAUI for MVP; reassess Flutter only if MAUI tooling or performance becomes a blocker.
- Favor managed services (Azure Functions/App Service + cloud storage) to reduce maintenance overhead and keep focus on curation/content.
- Track the following MAUI onboarding checklist to reach productive velocity:
  1. Install VS 2022 MAUI workload, .NET 8 SDK, Android toolchain; verify `dotnet new maui`.
  2. Complete Microsoft's starter tutorial, practice XAML hot reload + data binding.
  3. Build samples for Shell navigation, CollectionView/ListView, layouts, styles, DI.
  4. Deploy to Android emulator/device; note iOS requirements for future.
  5. Spike Aurora shell with mock data (home feed, Vibe card, reaction button).
  6. Integrate native share sheet, Firebase push notifications, and a simple backend endpoint.
  7. Test on multiple device sizes, check accessibility, log any store-submission blockers.

### Phase 4: Development Roadmap
**Status:** In Progress

#### Questions to Answer:
- [x] Break MVP into implementable milestones
  1. MAUI onboarding spike (Weeks 1-4): complete checklist, produce shell app with mock data.
  2. Content pipeline prototype (Weeks 5-8): define curation workflow, build backend/API returning daily picks + Vibe card.
  3. Feature integration (Weeks 9-14): connect MAUI app to backend, add reactions, sharing, optional push notifications.
  4. Polish & validation prep (Weeks 15-20): UX refinement, accessibility pass, gather beta feedback.
- [x] Estimate learning curve and development time
  - ~20 weeks of part-time effort aligns with six-month proof-of-concept target while allowing buffer for mobile tooling hurdles.
- [x] Identify risks and unknowns
  - Content sourcing bandwidth, MAUI platform quirks, push setup, backend hosting cost, limited personal time.
- [x] Define sprint/iteration structure
  - Bi-weekly mini-sprints with explicit goals; Sunday review to adjust scope and log findings.
- [x] Set realistic timeline expectations
  - POC ready by Month 5, leaving Month 6 for sentiment interviews and Phase 2 backlog shaping.

#### Roadmap:
- **Milestone A (Completed as of 2025-12-07): MAUI onboarding + shell with mock data**
  - All stories (A-01.1 to A-01.4) related to the initial application shell, mock data display, and UI controls have been implemented, verified, and merged into the `main` branch. This milestone established the foundational UI of the MAUI application.
- **Milestone B (Completed as of 2025-12-12): Backend/API + curation workflow**
  - All stories for Milestone B have been completed, including content schema definition (`content.schema.json`), local Azure Functions API prototype (`GetDailyContent` endpoint), and automated schema generation via `SchemaBuilder` console application integrated into the build process.
- **Milestone C (Completed as of 2025-12-21): Live data integration + reactions/sharing/push**
  1. Add typed data services in the MAUI app (e.g., `IDailyFeedService`) using `HttpClient` + Polly retry policy to fetch the Azure Function endpoint.
  2. Implement loading/error states in the home screen; fall back to cached JSON if offline.
  3. Build anonymous reaction endpoint (`POST /react`) storing aggregate counts in Azure Table Storage; wire the MAUI button to call it and display totals (read-only).
  4. Integrate native sharing via `Share.Default.RequestAsync`, formatting a message with Vibe title + link.
  5. Configure Firebase Cloud Messaging for Android: create Firebase project, upload google-services.json, register device tokens, and create a server-side push trigger (can reuse Azure Function with HTTP POST) to send the daily notification.
  6. Smoke-test the full loop end-to-end: update JSON, trigger notification, confirm app displays new data and reactions persist.
- **Milestone D (Completed as of 2025-12-23): Robust Testing Framework & Service Testability**
  1. **(Completed)** Service Testability Refactor: Created `Aurora.Client.Core` class library to isolate business logic from MAUI framework dependencies.
  2. **(Completed)** Unit Tests: Implemented comprehensive tests for `ContentService` using `Moq` and `HttpMessageHandler` mocking.
  3. **(Completed)** GitHub Actions CI pipeline for automated build and test verification.
  4. **(Completed)** Code Style Standardization: Roslyn Analyzers and `.editorconfig` enforcing zero-warning builds.
- **Milestone E (Completed as of 2025-12-25): Core Interactions & Polish**
  1. **(Completed)** Data Model Evolution: Update schema and models to support reaction counts (`UpliftCount`).
  2. **(Completed)** Backend Persistence: Implement Azure Table Storage service for tracking reactions.
  3. **(Completed)** API Endpoint: Create serverless endpoint to handle reaction submissions.
  4. **(Completed)** UI Integration: Connect "Uplift" button to backend and display live counts.
  5. **(Completed)** Native Sharing: Implement share sheet integration.
  6. **(Completed)** Visual Polish: Standardize styling and ensure accessibility compliance.
      - **(Completed)** Define Design System & Palette (Morning Mist).
      - **(Completed)** Implement Color & Theme Resources.
      - **(Completed)** Apply Design System to Main Page.
      - **(Completed)** Adjust typography and spacing for physical devices (Story E-03.4).

- **Milestone V-0 (Completed as of 2025-12-28): Beta Readiness**
  *Prerequisite to Phase 5 validation: transforms Aurora from local prototype to cloud-connected application with real content.*
  - **Phase 1 (Completed as of 2025-12-27):** Core infrastructure and real content integration
    1. **(Completed)** Deploy Aurora API and storage to Azure (Functions, Table Storage, Blob Storage).
    2. **(Completed)** Implement article reader (browser launch via READ button with Chrome Custom Tabs).
    3. **(Completed)** Migrate content delivery from static JSON to Azure Blob Storage with Polly retry policy.
    4. **(Completed)** Curate and integrate real uplifting news stories (1 Vibe + 10 Daily Picks from credible sources).
  - **Phase 2 (Completed as of 2025-12-28):** Content management tooling
    5. **(Completed)** PowerShell validation script (`Validate-Content.ps1`) for JSON schema compliance with URL validation and quality warnings.
    6. **(Completed)** PowerShell deployment script (`Deploy-Content.ps1`) with rollback capability and backup management.
    7. **(Completed)** PowerShell template generator (`New-ContentTemplate.ps1`) for content.json scaffolding.

- **Milestone V-2 (Completed as of 2026-01-03): Beta Testing Round 1 (Early Alpha Quality)**
  *Establish beta testing infrastructure and execute first validation round (self-testing + 1-4 trusted testers) to identify improvements and validate core value proposition.*
  - **Objective:** Enable structured feedback collection, deploy to Google Play Internal Testing, and validate Aurora's impact through personal use before broader distribution.
  - **Target Timeline:** 2 weeks
  - **Platform Scope:** Android-only (iOS deferred pending Mac/device acquisition)
  - **Terminology Note:** Using "beta" for consistency with Azure infrastructure naming (`rg-aurora-beta`), but this represents alpha-quality testing with small, trusted cohort.
  - **Scope (10 stories across 4 features):**
    - **Feature V-2.1:** Feedback Collection Infrastructure (3 stories)
      1. ✅ Create baseline survey (Google Form) with 14 questions (expanded) covering contact info, social media context, app usage patterns, positivity definition, and value proposition validation
      2. ✅ Create weekly feedback survey (Google Form) with 12 questions (expanded) tracking usage, sentiment, friction, and continuation intent
      3. ✅ Integrate "Share Feedback" button in app (opens weekly survey in browser via Chrome Custom Tabs)
    - **Feature V-2.2:** Beta Tester Onboarding Documentation (2 stories)
      4. ✅ Write Beta Tester Guide (warm, conversational tone; 300-500 words; PDF export for email distribution)
      5. ✅ Document AAB signing and Google Play distribution process (captures lessons learned from first deployment)
    - **Feature V-2.3:** Google Play Internal Testing Setup (2 stories)
      6. ✅ Create Google Play Console Developer account ($25 one-time fee; execute early to mitigate 24-48hr approval wait)
      7. ✅ Deploy Aurora to Internal Testing track (AAB upload, opt-in URL generation, self-verification on S24 Ultra)
    - **Feature V-2.4:** Self-Validation & External Tester Execution (3 stories)
      8. ✅ Execute 7-14 day self-validation period (end-of-week recap tracking, weekly survey submission, document findings)
      9. ✅ Curate and deploy at least one full content update (1 Vibe + 10 Picks; track total curation time; ≤45 min target)
      10. ✅ Synthesize findings, recruit 1-4 external testers (if self-validation positive), make Go/No-Go decision
  - **Success Criteria:**
    - ✅ Google Forms operational and integrated into app
    - ✅ Google Play Internal Testing track deployed
    - ✅ Self-validation completed with documented findings
    - ✅ Improvement backlog prioritized (Blocker/High/Medium/Low)
    - ✅ Go/No-Go decision documented with rationale
  - **Go Decision Thresholds:**
    - Opened Aurora first ≥50% of tested days (habit formation)
    - At least 3 stories resonated emotionally (content quality)
    - Weekly survey shows positive sentiment (value prop validated)
    - Curation time ≤45 min OR clear automation path (sustainability)
    - No critical bugs (app stability)

- **Milestone V-4 (Planned): Content Scalability - Dynamic Feed Architecture**
  *Migrate from static JSON files to database-backed API with pagination to resolve content curation bottleneck.*
  - **Execution Priority:** 🔴 **NEXT UP** (Execution Order: 1st)
  - **Objective:** Enable unlimited story storage, "Load More" pagination, and decouple content updates from app deployments.
  - **Rationale:** **Curator sustainability is the critical blocker.** Manual curation of 11 fresh stories daily is unsustainable (45 min/session, 7x/week). Current static JSON supports exactly 11 stories; users consume all content in 2 sessions with nothing new. Without resolving this bottleneck, content quality and consistency will degrade regardless of UX polish. Database-backed API allows curator to add 20-30 stories 2-3x/week instead of 11 stories 7x/week.
  - **Scope:**
    - Database migration (Azure Table Storage or Cosmos DB for story persistence)
    - API pagination endpoints (`/api/content?skip=0&take=10`)
    - Client "Load More" button (infinite scroll deferred)
    - Content migration tooling for existing archived stories
  - **Long-Term Vision:** RSS aggregation + manual approval queue (automated discovery, human curation quality gate)
  - **Reference:** See `docs/planning/backlog/planned/POST-BETA-IMPROVEMENTS.md` for detailed scalability analysis.
  - **Estimated Effort:** 1-2 weeks

- **Milestone CT-1 (Planned): Content Management Tooling Refactor**
  *Migrate PowerShell-based content management scripts to .NET console applications for better development experience, testability, and solution integration.*
  - **Execution Priority:** 🟡 **QUEUED** (Execution Order: 2nd, after V-4)
  - **Objective:** Eliminate PowerShell dependency for content management workflows; provide strongly-typed, unit-tested tooling that integrates with the Aurora solution and CI/CD pipelines. Image URL validation becomes default behavior.
  - **Rationale:** After V-4 database migration, improved tooling with image validation, schema checking, and deployment automation will be essential for managing larger content volumes efficiently.
  - **Estimated Effort:** 2-3 weeks
  - **Reference:** See full milestone definition below for detailed scope

- **Milestone V-3 (Planned): Beta UX Improvements**
  *Address critical UX friction points identified in Beta Round 1 self-validation before expanding external testing.*
  - **Execution Priority:** 🟢 **QUEUED** (Execution Order: 3rd, after CT-1)
  - **Objective:** Implement 3 High-Priority improvements (pull-to-refresh, Uplift state persistence, bottom navigation bar) to prevent negative first impressions and establish foundation for future features.
  - **Rationale:** Beta Round 1 self-validation identified UX gaps ("missing refresh," "Uplift doesn't track state," "single-page feels incomplete") that degrade user experience. However, external tester feedback is still minimal (1 tester just started). UX improvements have higher ROI after curator sustainability and tooling are resolved.
  - **Scope:** Pull-to-refresh functionality, local Uplift state tracking, bottom navigation with placeholder tabs (Home/Discover/Profile).
  - **Reference:** See `docs/planning/backlog/planned/POST-BETA-IMPROVEMENTS.md` items #1-3 for detailed requirements.
  - **Estimated Effort:** 9-12 hours (2-3 development sessions)

- **Milestone V-1 (Deferred): Integration Testing Infrastructure**
  *Establish comprehensive HTTP-layer test coverage for Azure Functions API to complement existing unit tests.*
  - **Objective:** Verify end-to-end request/response behavior, middleware pipeline, and runtime interactions that unit tests cannot cover (HTTP trigger binding, CORS headers, actual storage operations, Polly retry execution).
  - **Priority:** Deferred until post-beta (V-2 completion); not blocking Phase 5 validation.
  - **Scope:**
    1. Create `Aurora.Api.IntegrationTests` project with Azure Functions in-process hosting.
    2. Implement integration tests for `GetDailyContent` endpoint (success, 404, 500, retry scenarios).
    3. Implement integration tests for `ReactToContent` endpoint (increment, creation, failure, concurrency).
    4. Configure CI pipeline to run integration tests with Azurite service container.
    5. Document integration testing architecture and best practices (`docs/testing/INTEGRATION_TESTING.md`).
  - **Success Criteria:** ~8-10 integration tests passing in CI, clear documentation for adding future endpoint tests, maintained zero-warning build discipline.

- **Milestone CT-1 (Planned): Content Management Tooling Refactor**
  *Migrate PowerShell-based content management scripts to .NET console applications for better development experience, testability, and solution integration.*
  - **Execution Priority:** 🟡 **QUEUED** (Execution Order: 2nd, after V-4)
  - **Objective:** Eliminate PowerShell dependency for content management workflows; provide strongly-typed, unit-tested tooling that integrates with the Aurora solution and CI/CD pipelines.
  - **Current State:** PowerShell scripts (Validate-Content.ps1, Deploy-Content.ps1, Rollback-Content.ps1, New-ContentTemplate.ps1) functional but lack IDE support, unit testing, cross-platform consistency, and image URL validation.
  - **Proposed Architecture:**
    - **Aurora.ContentTools** (.NET 9 console application) - Unified CLI for all content management commands
    - **Aurora.ContentTools.Tests** (xUnit test project) - Unit tests for validation, deployment, template generation logic
    - **Distribution:** .NET global tool (`dotnet tool install -g Aurora.ContentTools`)
  - **Scope:**
    1. Create `Aurora.ContentTools` console application with CommandLineParser for CLI interface.
    2. Migrate validation logic from PowerShell to `ValidateCommand.cs` (reuse schema from `Aurora.Shared`).
    3. **Add image URL validation** (HTTP HEAD requests with timeout, 404/403/timeout detection, default on with `--skip-image-check` flag).
    4. Migrate deployment logic from PowerShell to `DeployCommand.cs` (share Azure SDK with `Aurora.Api`).
    5. Migrate rollback logic to `RollbackCommand.cs` with interactive backup selection.
    6. Migrate template generation logic to `TemplateCommand.cs`.
    7. Create comprehensive unit tests for all commands (validation rules, deployment workflow, error handling, image URL checking).
    8. Configure tool as .NET global tool with NuGet packaging.
    9. Update documentation with migration guide and deprecation notices for PowerShell scripts.
  - **Benefits:**
    - Full Visual Studio IntelliSense, debugging, and refactoring support
    - Unit testing with xUnit (TDD-enabled workflow)
    - Cross-platform consistency (eliminates PowerShell Core vs. Windows PowerShell differences)
    - Solution integration (shared dependencies, consistent error handling)
    - Faster startup times (compiled binaries vs. script interpretation)
    - CI/CD integration (easier GitHub Actions integration)
  - **Success Criteria:**
    - All 4 content management commands migrated and functional
    - 20+ unit tests covering validation, deployment, and template generation logic
    - Zero-warning build maintained
    - Installation via `dotnet tool install` documented and verified
    - PowerShell scripts deprecated with migration guide in README.md
  - **Estimated Effort:** 2-3 weeks

### Phase 5: Validation Strategy
**Status:** Ready to Begin (Milestone V-0 completed 2025-12-28)

#### Questions to Answer:
- [x] How will we know if this resonates with users...
  - Qualitative: beta testers describe calmer or more hopeful moods; testimonials or reviews call Aurora a positive refuge; anecdotes logged in PROJECT_JOURNAL.
  - Lightweight quantitative: track installs and ensure >10 active testers each week so the cohort stays engaged (no deeper retention targets yet).
- [x] What metrics matter...
  - User sentiment inputs (surveys, written quotes), install count, number of curated drops delivered, and daily curation time. Retention metrics deferred until automation exists.
- [x] When do we pivot or persevere...
  - Persevere if testers keep offering constructive feedback/testimonials and daily curation stays under ~45 minutes. Pivot or pause if feedback dries up or curation exceeds that threshold for two weeks.
- [x] What is our beta testing strategy...
  - Recruit 8-10 trusted testers (friends/community), onboard via internal test tracks, share a one-page guide, and gather async feedback via Google Forms plus optional DM follow-ups (weekly check-ins only if time allows).
- [x] How do we gather and incorporate feedback...
  - In-app ...Did this lift your mood...... prompt, shared feedback form, fortnightly interviews, telemetry dashboard, and PROJECT_JOURNAL summaries driving updates to the Phase 2 backlog.

#### Strategy:
- Run a 4-week beta cycle: baseline mood survey ... weekly builds ... sentiment + usage review ... final retro with go/no-go decision.
- Maintain a validation log mapping feedback to actions (ship/backlog/reject) to keep scope disciplined.
- Use success metrics to gate Phase 2 features; only expand scope once retention + curation targets hold for at least one month.

## Key Decisions Log

| Date | Decision | Rationale |
|------|----------|-----------|
| 2025-11-17 | Project codename: Aurora | Connection to Eos (goddess of dawn), symbolizes new beginnings and opening gates to brighter perspective |
| 2025-11-17 | Rejected dating app concept | Network effect problem insurmountable for solo developer; positive news app more feasible |
| 2025-11-17 | Considered "Happybara" branding | Charming but potentially too cutesy; "Eunoia" rejected due to phonetic similarity to "annoying" |
| 2025-11-23 | MVP scope defined (daily curated list, Vibe of the Day, anonymous reactions, sharing) | Lean feature set maximizes value vs effort |
| 2025-11-23 | Selected .NET MAUI + Azure Functions stack | Aligns with developer expertise while minimizing ops overhead |
| 2025-11-23 | Validation focuses on qualitative beta feedback and manual curation guardrails | Keeps success criteria realistic for hand-curated MVP |
| 2025-12-07 | Downgraded Swashbuckle.AspNetCore to 6.5.0 | Resolved `TypeLoadException` and `Microsoft.OpenApi` version conflict with `Microsoft.AspNetCore.OpenApi` 9.0.0 for .NET 9 compatibility. |
| 2025-12-12 | Refactored schema generation to single project | Simplified and made robust the schema generation process, moving from a two-project system to an in-memory console application using `Newtonsoft.Json` and a custom converter for clean, minimal output. |
| 2025-12-21 | Implemented GitHub Actions CI | Automates "Verify, Then Trust" mandate; requires successful build/test before merge. |
| 2025-12-23 | Adopted Azure Table Storage & Azurite | Selected for low cost, simplicity, and ease of local offline-first development for reaction persistence. |
| 2025-12-24 | Established "Morning Mist" Design System | Adopted a pastel, rounded, "Cozy & Warm" aesthetic (Nunito font, soft corners) to align with the "Refuge from Negativity" mission. |
| 2025-12-24 | Enabled Local LAN Testing | Configured API to listen on 0.0.0.0 and opened Firewall port 7071 to allow physical device testing over Wi-Fi, superior to emulator. |
| 2025-12-26 | Defined Milestone V-0: Beta Readiness | Created prerequisite milestone before Phase 5 validation to deploy cloud infrastructure, integrate real content, and establish content management tooling. Two-phase approach: Phase 1 (core infrastructure, V-0.1 to V-0.4) followed by Phase 2 (PowerShell automation tooling, V-0.5 to V-0.7) after verification. Chose Azure Blob Storage for content delivery, hotlinking for images with placeholder fallback, and PowerShell for scripting. |
| 2025-12-27 | Adopted Chrome Custom Tabs for article reader | Selected `BrowserLaunchMode.SystemPreferred` over external browser for superior Android UX (faster load, smoother transitions, standard pattern used by major apps). |
| 2025-12-27 | Implemented Polly retry policy for blob storage | Added exponential backoff with jitter (3 retries, 1s/2s/4s delays) to prevent transient blob storage failures from affecting users. |
| 2025-12-28 | PowerShell chosen for content management tooling | Selected PowerShell over .NET console apps for V-0 to minimize dependencies (Windows-native, no install required); deferred .NET migration to CT-1 based on beta workflow feedback. |
| 2025-12-29 | Milestone V-2 defined for Phase 5 validation | Created structured beta testing milestone with 10 stories across feedback infrastructure, Google Play setup, and self-validation. Scope: Android-only, self + 1-4 trusted testers, 2-week timeline. Package name: `com.projectaurora.app` (permanent), App Title: "Aurora - Positive News" (changeable). Using "beta" terminology for consistency with Azure naming despite alpha-quality testing. Go/No-Go thresholds: ≥50% first-app-opened, ≥3 resonant stories, positive sentiment, ≤45 min curation time. |
| 2026-01-02 | Acquired iOS testing hardware (Mac Mini M4, iPhone 13) | Favorable pricing on refurbished hardware made iOS support economically viable; enables cross-platform testing and broader tester recruitment beyond Android-only limitation. Hardware expected to arrive within 1-2 weeks. |

---

## Alternative Ideas Considered

### Application Names/Branding
- **Happybara** - Wordplay on capybara, cute and coexist metaphor
- **Eunoia** - "Beautiful thinking" in Greek, but sounds like "annoying"
- Other Greek mythology options: Euphrosyne, Eleos, Astraea, Iris

### Application Concepts (Rejected)
- **Dating app focused on transparency and long-term relationships**
  - Rejected due to: network effect challenges, high user acquisition costs, complex trust & safety requirements

---

## Notes & Observations

- Developer has ~4 years .NET professional experience
- Beginner-level mobile development experience (some Android learning)
- Prefers Greek mythology references
- Target is general audience, not enterprise/professional focus
- MVP approach: Start simple, iterate based on validation

---

## Deferred Business & Legal Items

These items are documented for future consideration but are not required for beta testing (Milestone V-2).

### Account & Identity Strategy

**Current State (Beta Testing):**
- **GitHub:** Personal account (`brycesimm/Project-Aurora`) - Repo can be transferred to organization later without losing history
- **Google Forms/Sheets:** Personal Gmail account (themetanoiasociety@gmail.com) - Data is exportable and transferable via sharing
- **Azure Subscription:** Personal Pay-As-You-Go subscription - Resources can be migrated between subscriptions if needed
- **Billing:** Personal credit card (expected to remain permanent for independent development)
- **Domain Registration:** ✅ **COMPLETED** - Acquired metanoiasociety.com/.net/.org, metanoia-society.com, the-metanoia-society.com
- **Professional Email:** Pending setup (using themetanoiasociety@gmail.com temporarily)

**Google Play Console Decision:** ✅ **COMPLETED**
- **Account:** themetanoiasociety@gmail.com (2025-12-31)
- **Developer Name:** Metanoia Society (PERMANENT)
- **Package Name:** com.metanoiasociety.aurora (locked after first AAB upload)
- **Cost:** $25 one-time fee (paid)

### Post-Beta Infrastructure Items

**Priority: Low (defer until Go decision after V-2)**

1. **Professional Email Setup** ✅ **NEXT ACTION ITEM**
   - **Domains Available:** metanoiasociety.com/.net/.org (already owned)
   - **Target Email:** contact@metanoiasociety.com (or support@, dev@)
   - **Purpose:** Professional contact for Play Console, support inquiries, privacy policy hosting
   - **Timing:** Before public launch (post-beta if Go decision)
   - **Options:** Google Workspace, Zoho Mail, ProtonMail, custom hosting
   - **Migration:** Update Google Play Console email from themetanoiasociety@gmail.com to custom domain

2. **GitHub Organization Migration**
   - **Current:** `brycesimm/Project-Aurora` (personal account)
   - **Future:** Transfer to `projectaurora` organization if bringing on collaborators
   - **Process:** GitHub allows repository transfers without losing commit history
   - **Timing:** Only if partnerships/collaboration materialize

3. **Business Entity Formation (LLC)**
   - **Current:** Sole proprietor (personal income, Schedule C)
   - **Threshold for Consideration:** Revenue exceeds ~$5K/year OR liability protection desired
   - **Tax Implications:** Google sends 1099-K tax forms to Play Console account email if revenue threshold met
   - **Current Aurora Status:** Free app, no immediate tax/legal implications
   - **Timing:** Only if app monetizes successfully post-launch

4. **Privacy Policy & Terms of Service Hosting**
   - **Current:** Not required for Internal Testing
   - **Future:** Host on metanoiasociety.com (privacy policy required for public Play Store release)
   - **Timing:** Before Open Testing or Production release
   - **Tools:** Static site generator (Jekyll, Hugo) or simple HTML on custom domain

**Documentation Reference:**
- Developer account strategy discussion documented in PROJECT_JOURNAL.md (2025-12-29 session)
- Google Play Console account creation details in Story V-2.3.1 (BACKLOG.md)

---

**Last Updated:** 2026-01-01