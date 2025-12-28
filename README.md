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
- **Back-End:** Azure Functions (Serverless API, .NET 9 Isolated Process).
- **Storage:** Azure Blob Storage (dynamic content delivery) + Azure Table Storage (reaction persistence).
- **Architecture:** Decoupled "Core" logic library for business rules and service testability.
- **Infrastructure:** Bicep IaC templates, automated JSON Schema generation, Polly retry resilience.
- **Local Development:** Azurite emulator with automated setup tooling.

## 🚀 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio](https://visualstudio.microsoft.com/vs/) (any version supporting **.NET 9 development**)
- [Azure Functions Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local) (for local API development)

### Running the Application

**Option 1: Local Development (Debug Builds)**

1. **Clone the repository:**
   ```bash
   git clone https://github.com/brycesimm/Project-Aurora.git
   cd Project-Aurora
   ```

2. **Setup Azurite Blob Storage:**
   Run the automated setup tool to create the local content container:
   ```bash
   cd tools/AzuriteSetup
   dotnet run
   ```
   This creates the `aurora-content` container and uploads sample content to Azurite.

3. **Launch the API:**
   Navigate to `src/Aurora.Api` and start the local host:
   ```bash
   cd ../../src/Aurora.Api
   func start
   ```
   Azurite will start automatically. Verify the endpoint works: `http://localhost:7071/api/GetDailyContent`

4. **Run the MAUI Client (Visual Studio):**
   - Open `Project-Aurora.sln`
   - Set `Aurora` as the Startup Project
   - Select **Debug** configuration (uses `localhost:7071`)
   - Select your target device (Android Emulator recommended for first run)
   - Press **F5**

**Option 2: Production Cloud (Release Builds)**

1. **Build for Release:**
   - Open `Project-Aurora.sln` in Visual Studio
   - Set configuration to **Release**
   - Build and deploy to your device

   Release builds connect to the production Azure Functions API automatically—no local API setup required.

### 📱 Android CLI Workflow (Manual Install & ADB)

If you prefer the command line or need to troubleshoot deployment:

**1. Enable Developer Mode:**
   - Go to `Settings > About Phone` on your Android device.
   - Tap `Build Number` 7 times to enable Developer Options.
   - In `Developer Options`, enable **USB Debugging**.

**2. Verify Connection:**
   Connect via USB and run:
   ```powershell
   adb devices
   # Ensure your device is listed and authorized
   ```

**3. Install the APK:**
   Use the .NET CLI to build and install directly to the connected device:
   ```powershell
   dotnet build src/Aurora/Aurora.csproj -f net9.0-android -t:Install
   ```

**4. Launch via ADB:**
   You can launch the app from the device screen or use ADB to force-start it:
   ```powershell
   adb shell monkey -p com.companyname.aurora -c android.intent.category.LAUNCHER 1
   ```

**5. Debugging Crashes:**
   If the app closes immediately, view the crash logs:
   ```powershell
   adb logcat -d -s AndroidRuntime:E
   ```

### 📡 Physical Device Connectivity (Debug Builds Only)

**Note:** Release builds connect to production Azure Functions—no local networking configuration needed.

For **Debug builds** on physical devices connecting to your local API:

1.  **Firewall:** Allow inbound traffic on port 7071 (Admin PowerShell):
    ```powershell
    New-NetFirewallRule -DisplayName "Allow Aurora API Port 7071" -Direction Inbound -LocalPort 7071 -Protocol TCP -Action Allow
    ```

2. **Configure IP:** Find your PC's local IP (via `ipconfig`) and update `src/Aurora/appsettings.Development.json`:
    ```json
    {
      "ApiSettings": {
        "BaseUrl": "http://192.168.1.5:7071/api/"
      }
    }
    ```
    Replace `192.168.1.5` with your actual local IP.

3.  **Host Binding:** Start the API with:
    ```bash
    func start --host 0.0.0.0
    ```

4. **Deployment:** Connect via USB for the initial installation. Once running, the app communicates with the API over Wi-Fi.

## 🧪 Testing and Quality

Aurora maintains a "Verify, Then Trust" policy. 
- **Unit Tests:** Business logic is isolated in `Aurora.Client.Core` and tested via xUnit in `Aurora.Client.Core.Tests`.
- **CI Pipeline:** Every pull request is automatically built and tested via GitHub Actions to ensure stability.

## 📚 Documentation

- [**Documentation Index:**](docs/README.md) Complete documentation map and navigation guide.
- [**Architecture:**](docs/technical/ARCHITECTURE.md) System design, project structure, and key architectural patterns.
- [**Design System:**](docs/design/DESIGN_SYSTEM.md) "Morning Mist" visual language, color palette, and typography standards.
- [**Contributing:**](docs/guides/CONTRIBUTING.md) Development workflow, prerequisites, and coding standards.
- [**Planning & History:**](docs/planning/PROJECT_JOURNAL.md) Project evolution, decisions, and development sessions.

## ⚖️ Legal & Copyright

**Copyright © 2025 Bryce Simmerman. All Rights Reserved.**

This software is proprietary and confidential. Unauthorized copying, modification, distribution, or use of this software, via any medium, is strictly prohibited for both commercial and private purposes without explicit written permission from the copyright holder.
