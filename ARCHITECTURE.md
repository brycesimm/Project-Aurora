# Project Aurora: Architectural Overview

This document describes the high-level architecture and design patterns of Project Aurora.

## 🏗 System Structure

Aurora follows a decoupled, multi-project architecture designed for maximum code reuse, testability, and cross-platform flexibility.

### Component Map

1.  **`src/Aurora` (.NET MAUI):**
    - The primary UI layer.
    - Handles platform-specific lifecycle, navigation, and visual rendering.
    - Uses XAML for UI definition and Code-Behind for interaction logic.

2.  **`src/Aurora.Client.Core` (.NET 9 Class Library):**
    - The "Brains" of the client application.
    - Contains business logic, service implementations (`ContentService`), and data-fetching logic.
    - This project is independent of any UI framework, enabling easy unit testing.

3.  **`src/Aurora.Shared` (.NET 9 Class Library):**
    - Contains shared contracts: Data Models (`ContentItem`, `ContentFeed`) and Interface definitions (`IContentService`).
    - References by both the Client and the API to ensure data contract consistency.

4.  **`src/Aurora.Api` (Azure Functions):**
    - The serverless backend.
    - Serves content payloads via HTTP-triggered functions.
    - Currently serves local mock data, prepared for cloud deployment.

5.  **`src/SchemaBuilder` (Console App):**
    - A specialized build-time tool.
    - Generates `content.schema.json` directly from the C# `ContentItem` models.
    - Ensures that our documentation and data contracts are always synchronized with the code.

## ⚙ Key Architectural Patterns

### Service Testability & Decoupling
To avoid the complexities of testing within a mobile framework, all core logic is isolated in `Aurora.Client.Core`. 
- **Typed Clients:** We use the `HttpClient` Factory pattern in `MauiProgram.cs` to inject pre-configured clients into our services.
- **Interface Segregation:** The UI depends on interfaces from `Aurora.Shared`, allowing for easy mocking during UI development or testing.

### Automated Schema Synchronization
We treat our C# models as the "Single Source of Truth." 
- The `SchemaBuilder` project is integrated into the build process.
- Whenever the solution is built, the JSON schema for our content is updated, preventing "drift" between the backend expectations and the frontend implementation.

### Platform-Specific Networking
To accommodate the unique networking environment of mobile emulators (specifically Android's `10.0.2.2` loopback), we utilize platform-detection logic within our Dependency Injection setup to automatically remap local API calls to the correct host machine interface.

## 📊 Data Flow

1.  **Build Time:** Models -> `SchemaBuilder` -> `content.schema.json`.
2.  **Runtime:** 
    - `Aurora` (UI) requests data via `IContentService`.
    - `ContentService` (Core) fetches JSON from `Aurora.Api`.
    - JSON is deserialized using PascalCase-to-snake_case mapping.
    - UI updates via Data Binding.
