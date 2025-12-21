# Contributing to Project Aurora

Thank you for your interest in helping us build a refuge for positive news! This guide will help you get your development environment set up and explain our workflow.

## 🛠 Prerequisites

Before you start, ensure you have the following installed:
- **.NET 9.0 SDK**
- **Visual Studio 2022** with the **.NET MAUI development** workload.
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

- **File-Scoped Namespaces:** Use `namespace MyNamespace;` at the top of the file.
- **Primary Constructors:** Prefer C# 12 primary constructors for Dependency Injection where appropriate.
- **Analyzers:** We use Roslyn Analyzers to enforce code style. Ensure your code is free of warnings before submitting a PR.
