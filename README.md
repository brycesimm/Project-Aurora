# Project Aurora: A Refuge of Positive News

Project Aurora is a cross-platform mobile application built with .NET MAUI, designed to provide a sanctuary from the often negative and sensationalized traditional media landscape. It delivers a curated stream of uplifting, inspiring, and family-friendly stories.

## 🌟 The Mission

In a world where news feeds are often engineered to maximize outrage and fear, Aurora serves as an oasis of credible optimism. Our goal is to protect mental wellbeing by counterbalancing negativity with narratives that inspire productivity, empathy, and hope for humanity.

## ✨ Key Features

- **Vibe of the Day:** A standout daily feature—one exceptional story, video, or article showcased as a focal point for inspiration.
- **Daily Picks:** A curated list of 5-10 uplifting news stories delivered daily.
- **Anonymous Reactions:** Share your positive sentiment (e.g., "Uplifted me") without the friction of user accounts or social media pressure.
- **Native Sharing:** Seamlessly share inspiring stories with friends and family using your device's native share sheet.

## 🛠 Tech Stack

- **Front-End:** .NET MAUI (C# / XAML) targeting Android, iOS, Windows, and macOS.
- **Back-End:** Azure Functions (Serverless API).
- **Architecture:** Decoupled "Core" logic library for business rules and service testability.
- **Infrastructure:** Automated JSON Schema generation to ensure data contract synchronization.

## 🚀 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio](https://visualstudio.microsoft.com/vs/) (any version supporting **.NET 9 development**)
- [Azure Functions Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local) (for local API development)

### Running the Application

1. **Clone the repository:**
   ```bash
   git clone https://github.com/brycesimm/Project-Aurora.git
   cd Project-Aurora
   ```

2. **Launch the API:**
   Navigate to `src/Aurora.Api` and start the local host:
   ```bash
   func start
   ```

3. **Run the MAUI Client:**
   Open `Project-Aurora.sln` in Visual Studio and set the `Aurora` project as the Startup Project. Select your desired target (Emulator or Physical Device) and press F5.

## 🧪 Testing and Quality

Aurora maintains a "Verify, Then Trust" policy. 
- **Unit Tests:** Business logic is isolated in `Aurora.Client.Core` and tested via xUnit in `Aurora.Client.Core.Tests`.
- **CI Pipeline:** Every pull request is automatically built and tested via GitHub Actions to ensure stability.

## 📚 Documentation

- [**Architecture:**](docs/ARCHITECTURE.md) Learn about the system design, project structure, and key patterns.
- [**Design System:**](docs/DESIGN_SYSTEM.md) Explore the "Morning Mist" visual language, color palette, and typography standards.
- [**Contributing:**](docs/CONTRIBUTING.md) Development workflow, prerequisites, and coding standards.

## ⚖️ Legal & Copyright

**Copyright © 2025 Bryce Simmerman. All Rights Reserved.**

This software is proprietary and confidential. Unauthorized copying, modification, distribution, or use of this software, via any medium, is strictly prohibited for both commercial and private purposes without explicit written permission from the copyright holder.
