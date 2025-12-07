# Project Aurora - Backlog

This document tracks the features, user stories, and tasks for Project Aurora. It is organized by milestones defined in `.gemini/PLANNING.md`.

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
- [ ] **Story B-01.2:** As a developer, I need a local, HTTP-triggered function to serve the mock content, allowing for front-end development without a live cloud environment.
    - **AC 1:** A new, runnable .NET Azure Functions project is added to the solution.
    - **AC 2:** An HTTP GET endpoint named `GetDailyContent` is created within the new project.
    - **AC 3:** When called, the endpoint reads `sample.content.json` and returns its contents with a `200 OK` status and correct `Content-Type` header (`application/json`).

---

### Feature B-02: Developer Tooling
*This feature covers the creation of internal tools to improve the development workflow and ensure long-term project maintainability.*

- [ ] **Story B-02.1:** As a developer, I need an automated way to generate the JSON schema from C# models to prevent drift between the backend and the data contract.
    - **AC 1:** A new console application project (e.g., `Aurora.SchemaGenerator`) is added to the solution.
    - **AC 2:** The tool uses a NuGet package (e.g., `Newtonsoft.Json.Schema`) to generate the schema.
    - **AC 3:** Running the tool successfully generates/updates `content.schema.json` based on the `ContentItem` C# class and a root object definition.
