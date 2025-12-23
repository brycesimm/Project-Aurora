# Contributing to Project Aurora

Thank you for your interest in helping us build a refuge for positive news! This guide will help you get your development environment set up and explain our workflow.

## 🛠 Prerequisites

Before you start, ensure you have the following installed:
- **.NET 9.0 SDK**
- **Visual Studio** (any version supporting **.NET 9 development**).
- **Android Emulator** or a physical device for testing.
- **Azure Functions Core Tools** (Optional, but required for local API changes).

## 🚀 Local Development Workflow

### 1. Running the Backend
The MAUI app fetches data from an Azure Function. For local development:
1.  Navigate to `src/Aurora.Api`.
2.  Run `func start` to host the mock API locally (usually at `http://localhost:7071`).

### 2. Running the Client
1.  Open `Project-Aurora.sln` in Visual Studio.
2.  Set `Aurora` as your startup project.
3.  Select your target platform (Android, iOS, or Windows).
4.  Press **F5** to build and run.

## 📱 Android Networking Notes

When running on the Android Emulator, `localhost` refers to the emulator itself. We have configured the app to automatically use `10.0.2.2` (the host machine's loopback) when an Android device is detected.

If you change the API port or host, update the `appsettings.json` in `src/Aurora`.

## 🌿 Branching and Commits

We follow a strict **"Branching First"** and **"Verify, Then Trust"** mandate:

1.  **Branching:** Never work directly on `main`. Create a feature or bugfix branch:
    - `feature/F-##-description`
    - `bugfix/B-##-description`
2.  **Stories:** Work is tracked via `BACKLOG.md`. Ensure your branch maps to a specific story.
3.  **CI Checks:** All Pull Requests trigger a GitHub Actions build and test suite. These must pass before a merge can occur.
4.  **Commits:** Use clear, concise commit messages. If a change is complex, use a multi-line format explaining *why* the change was made.

## 🧪 Testing

New features should include unit tests in the `Aurora.Client.Core.Tests` project.
To run tests locally:
```bash
dotnet test
```

## 📐 Coding Standards

Project Aurora enforces strict coding standards to maintain a high-quality codebase. We use **native .NET 9 Analyzers** configured via a root `.editorconfig` file.

### Core Rules
1.  **Zero Warnings Policy:** The solution must build with **0 warnings**. Any new warning is considered a blocker and must be resolved before merging.
2.  **Indentation:** We use **Tabs** (set to 4-character width) for indentation.
3.  **Namespaces:** Use **File-Scoped Namespaces** (`namespace Aurora.Shared;`) to reduce nesting depth.
4.  **Documentation:** All public members (classes, methods, properties) must have XML documentation (`/// summary`).
5.  **Using Directives:** Place `using` directives **outside** the namespace declaration at the top of the file.

### Quality & Analysis
- **Implicit Analysis:** We use the `<AnalysisLevel>latest-all</AnalysisLevel>` property to enable the full suite of Microsoft's code quality rules.
- **Auto-Formatting:** You can (and should) run `dotnet format` locally to automatically fix whitespace and style violations before pushing.

### Best Practices
- **Primary Constructors:** Prefer C# 12 primary constructors for Dependency Injection classes.
- **Async/Await:** Always use `async`/`await` for I/O operations. In library code (Core/Shared), use `.ConfigureAwait(false)` to prevent deadlocks.
- **DTOs:** Data Transfer Objects (in `Shared.Models`) should favor initialization-friendly properties but avoid complex logic.
