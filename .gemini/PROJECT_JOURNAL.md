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
