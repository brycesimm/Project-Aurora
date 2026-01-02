# Milestone B: Backend/API + Curation Workflow

**Status:** ✅ Completed
**Completion Date:** 2025-12-12

*This milestone covers the definition of our data structure and the creation of a local API endpoint to serve mock content, forming the foundation for our backend.*

---

## Feature B-01: Content Structure & API Definition
- [x] **Story B-01.1:** As a developer, I need a clearly defined JSON schema for the app's content so that the front-end and back-end have a shared understanding of the data model.
    - **AC 1:** [x] A `content.schema.json` file is created to formally define the structure, including fields like `id`, `title`, `snippet`, `sourceUrl`, `imageUrl`, and `publicationDate`.
    - **AC 2:** [x] The schema distinguishes between the "Vibe of the Day" and standard "Daily Picks," allowing for unique fields on the Vibe card.
    - **AC 3:** [x] A `sample.content.json` file is created that validates against the schema, containing one "Vibe" and at least five "Picks."
- [x] **Story B-01.2:** As a developer, I need a local, HTTP-triggered function to serve the mock content, allowing for front-end development without a live cloud environment.
    - **AC 1:** A new, runnable .NET Azure Functions project is added to the solution.
    - **AC 2:** An HTTP GET endpoint named `GetDailyContent` is created within the new project.
    - **AC 3:** When called, the endpoint reads `sample.content.json` and returns its contents with a `200 OK` status and correct `Content-Type` header (`application/json`).

---

## Feature B-02: Automated JSON Schema Generation
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
