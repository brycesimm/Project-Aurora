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
    - Manages data persistence for user interactions (Reactions) using Azure Table Storage.

5.  **`src/Aurora.Api.Tests` (xUnit Project):**
    - Dedicated unit test project for the API layer.
    - Verifies service logic and storage interactions using mocked dependencies (`Moq`).

6.  **`src/SchemaBuilder` (Console App):**
    - A specialized build-time tool.
    - Generates `content.schema.json` directly from the C# `ContentItem` models.
    - Ensures that our documentation and data contracts are always synchronized with the code.

## ⚙ Key Architectural Patterns

### Service Testability & Decoupling
To avoid the complexities of testing within a mobile framework, all core logic is isolated in `Aurora.Client.Core`. 
- **Typed Clients:** We use the `HttpClient` Factory pattern in `MauiProgram.cs` to inject pre-configured clients into our services.
- **Interface Segregation:** The UI depends on interfaces from `Aurora.Shared`, allowing for easy mocking during UI development or testing.

### Cloud-Native Persistence (Local & Remote)
For handling user reactions, we utilize **Azure Table Storage** for its performance and cost-effectiveness.
- **Abstraction:** The `ReactionStorageService` encapsulates all database interactions, preventing leakage of storage concerns into the HTTP trigger layer.
- **Local Development:** We use **Azurite**, an open-source emulator, to mirror the Azure Table Storage API locally. This ensures our "Cloud Native" code works offline without modification.

### Automated Schema Synchronization
We treat our C# models as the "Single Source of Truth." 
- The `SchemaBuilder` project is integrated into the build process.
- Whenever the solution is built, the JSON schema for our content is updated, preventing "drift" between the backend expectations and the frontend implementation.

### Platform-Specific Networking
To accommodate the unique networking environment of mobile emulators (specifically Android's `10.0.2.2` loopback), we utilize platform-detection logic within our Dependency Injection setup to automatically remap local API calls to the correct host machine interface.

### Reactive UI Models
To ensure a responsive and "live" user experience, especially during interactive operations like reactions, we utilize the **Observer Pattern** via `INotifyPropertyChanged`. 
- **Automatic Sync:** Data models in `Aurora.Shared` that are bound to the UI implement this interface. 
- **Direct Interaction:** This allows service-layer updates (like an optimistic increment or a server confirmation) to reflect instantly in the UI without the need for manual view-model or collection refreshes.

## 📊 Data Flow

### Content Delivery (Read)
1.  **Build Time:** Models -> `SchemaBuilder` -> `content.schema.json`.
2.  **Runtime:** 
    - `Aurora` (UI) requests data via `IContentService`.
    - `ContentService` (Core) fetches JSON from `Aurora.Api`.
    - JSON is deserialized using PascalCase-to-snake_case mapping.
    - UI updates via Data Binding.

### User Interaction (Write)
1.  User clicks "Uplift" button.
2.  `ContentService` sends POST request to `Aurora.Api` (`/api/articles/{id}/react`).
3.  `ReactionStorageService` interacts with Azure Table Storage (or Azurite) to increment counts.
4.  Updated count is returned to Client and UI updates optimistically.

## ☁️ Cloud Deployment Architecture

### Azure Infrastructure (Production)

Aurora's cloud infrastructure is deployed to **Azure (East US region)** using a **serverless, consumption-based** architecture to minimize operational costs during beta testing.

**Resource Group:** `rg-aurora-beta`

#### Compute Layer
- **Azure Functions** (Consumption Plan - Y1 SKU)
  - Serverless, pay-per-execution model ($0/month for beta usage under 1M executions)
  - .NET 9 isolated process runtime
  - Linux hosting environment
  - Auto-scales 0-3 instances based on load
  - CORS configured for mobile application access

**Endpoints:**
- `GET /api/GetDailyContent` - Serves content feed (currently embedded JSON, migrating to Blob Storage in V-0.3)
- `POST /api/articles/{id}/react` - Increments reaction counts in Table Storage

#### Storage Layer
- **Azure Table Storage**
  - NoSQL key-value store for reaction counts
  - Table: "Reactions" (PartitionKey: article ID, RowKey: "uplift")
  - Provides global persistence across all users and sessions
  - Cost: ~$0.045/GB/month (expecting <100MB for beta)

- **Azure Blob Storage**
  - Container: `aurora-content` (private access)
  - Planned for V-0.3: Dynamic content delivery (content.json updates without redeployment)
  - Cost: ~$0.02/GB/month

#### Monitoring Layer
- **Application Insights**
  - Telemetry collection for Function App performance and errors
  - Integrated Log Analytics workspace for query and analysis
  - Enables cold start monitoring, exception tracking, and usage analytics

### Deployment Model

**Infrastructure as Code:**
- Bicep templates define all Azure resources declaratively
- Modular structure: `main.bicep` orchestrates storage, monitoring, and compute modules
- Parameterized for environment-specific configuration (beta, production)
- PowerShell deployment script (`deploy.ps1`) handles validation and deployment

**Code Deployment:**
- Azure Functions code deployed via `func azure functionapp publish`
- MAUI application uses environment-specific configuration:
  - **Debug builds:** `appsettings.Development.json` → localhost API (local development)
  - **Release builds:** `appsettings.json` → Azure Functions API (cloud-connected)

### Network Architecture

**Mobile Client → Cloud API:**
- MAUI app communicates with Azure Functions over HTTPS
- Release builds hardcode production API URL (no runtime discovery)
- Debug builds use localhost with platform-specific remapping (Android emulator: 10.0.2.2)

**Azure Functions → Table Storage:**
- Functions use managed service connection strings configured via App Settings
- No public internet traversal (Azure backbone network)
- Connection string injected at runtime via environment variables

### Security Model

**API Authentication:**
- Currently anonymous (no user accounts in MVP)
- CORS restricts requests to mobile application origins only
- HTTPS enforced for all endpoints

**Storage Security:**
- Table Storage accessed via connection string (secured in Function App settings)
- Blob Storage private access (no public listing or direct access)
- Connection strings never committed to source control

### Cost Profile (Beta Phase)

**Current Monthly Cost:** ~$0-5/month
- Consumption Plan: $0 (under 1M free executions)
- Table Storage: ~$0.05 (minimal data volume)
- Blob Storage: ~$0.02 (single content file)
- Application Insights: Free tier (5GB ingestion/month)

**Quota Allocation:**
- Y1 VMs (Consumption Plan): 3 instances (supports auto-scaling)
- Sufficient for beta testing with <100 concurrent users

### Disaster Recovery

**Stateful Components:**
- Reaction counts in Table Storage (persistent, replicated within region)
- Content JSON in Blob Storage (planned for V-0.3, replicated)

**Stateless Components:**
- Azure Functions (redeployable from source in <5 minutes)
- Infrastructure (redeployable from Bicep templates in <3 minutes)

**Backup Strategy:**
- Infrastructure templates in source control provide "restore from scratch" capability
- Table Storage data backed up via Azure's geo-redundant storage (GRS)
- No manual backup processes required for beta phase

### Observability

**Telemetry Streams:**
- Application Insights captures:
  - HTTP request/response telemetry (latency, status codes)
  - Exception tracking and stack traces
  - Custom events (future: reaction submissions, content loads)
  - Cold start metrics for Consumption Plan performance

**Monitoring Access:**
- Azure Portal → Application Insights dashboard
- Log Analytics queries for advanced troubleshooting
- Real-time metrics for function execution and failures
