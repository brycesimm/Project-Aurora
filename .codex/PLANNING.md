# Project Aurora - Planning Framework

**Project Codename:** Aurora
**Mission:** A positive news application that provides refuge from negativity, fear-mongering, and divisive media
**Target Audience:** General audience (PG-13+)

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
- **Milestone B (In Progress - Updated 2025-12-12): Backend/API + curation workflow**
  - The strategy and initial stories for Milestone B have been collaboratively defined and documented in `BACKLOG.md`. The focus is on creating a local API prototype using Azure Functions to serve a structured JSON payload, decoupling the front-end from the data source. **Story B-02.1 (Define the Content Schema)** has been completed, as well as **Story B-01.2 (Local, HTTP-triggered function for mock content)**. The automation for schema export has been significantly refactored and simplified into a single console application (`SchemaBuilder`) as part of **Story B-02.2**, and integrated into the build process (**Story B-02.3**).
- **Milestone C (Completed as of 2025-12-21): Live data integration + reactions/sharing/push**
  1. Add typed data services in the MAUI app (e.g., `IDailyFeedService`) using `HttpClient` + Polly retry policy to fetch the Azure Function endpoint.
  2. Implement loading/error states in the home screen; fall back to cached JSON if offline.
  3. Build anonymous reaction endpoint (`POST /react`) storing aggregate counts in Azure Table Storage; wire the MAUI button to call it and display totals (read-only).
  4. Integrate native sharing via `Share.Default.RequestAsync`, formatting a message with Vibe title + link.
  5. Configure Firebase Cloud Messaging for Android: create Firebase project, upload google-services.json, register device tokens, and create a server-side push trigger (can reuse Azure Function with HTTP POST) to send the daily notification.
  6. Smoke-test the full loop end-to-end: update JSON, trigger notification, confirm app displays new data and reactions persist.
- **Milestone D (In Progress): Robust Testing Framework & Service Testability**
  1. Apply consistent theming (typography, color palette) and ensure accessibility basics (font scaling, contrast checks).
  2. Test on multiple device sizes/emulators; document layout issues and fixes.
  3. Instrument basic telemetry (App Center or Azure Insights) for crash/error tracking.
  4. Recruit a small beta circle; prepare test script (install instructions, feedback form) and capture sentiment in PROJECT_JOURNAL.
  5. Compile store-readiness checklist (privacy policy draft, screenshot requirements, push notification disclosures) even if launch is later.
  6. **(Completed)** Implement GitHub Actions CI pipeline for automated build and test verification.
  7. **(Completed)** Standardize code style using Roslyn Analyzers and `.editorconfig`.
  8. Prioritize follow-up items feeding into Phase 5 validation experiments.

### Phase 5: Validation Strategy
**Status:** In Progress

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

**Last Updated:** 2025-12-23