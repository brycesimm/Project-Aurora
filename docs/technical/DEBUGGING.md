# Project Aurora - Debugging Notes

This document contains notes on recurring or significant technical issues and their solutions.

## Persistent Build Cache Corruption

**Symptom:**
- Changes made in XAML files (e.g., setting a `BackgroundColor`) do not appear in the running application, even after stopping, restarting, or using the "Rebuild Solution" command.
- The build output may show many "Skipping target..." messages, indicating that the build system incorrectly believes the files are up-to-date.

**Troubleshooting Steps (in order of severity):**

1.  **Standard Clean & Rebuild:**
    - In Visual Studio: `Build > Clean Solution`, then `Build > Rebuild Solution`.

2.  **Manual `bin`/`obj` Deletion:**
    - Close Visual Studio.
    - Navigate to the specific project directory (e.g., `src/Aurora`).
    - Manually delete the `bin` and `obj` folders.
    - Re-open and run.

3.  **The "Nuclear Option" - `.vs` Folder Deletion:**
    - Close Visual Studio.
    - Navigate to the **root** of the solution (e.g., `C:\Programming\Project-Graveyard\Project-Aurora`).
    - Enable "Show hidden items" in File Explorer.
    - Manually delete the hidden **`.vs`** folder.
    - This was the required solution for our initial severe caching issue.

**Verification Technique ("Forcing Change"):**

If you suspect changes are still not being picked up, create a strong dependency between XAML and C# to get a definitive result:
1.  Add a `x:Name` attribute to a XAML element (e.g., `<VerticalStackLayout x:Name="MyLayout">`).
2.  In the C# code-behind, reference this name in the constructor after `InitializeComponent()` (e.g., `MyLayout.BackgroundColor = Colors.Red;`).
3.  **Result:**
    - If the app builds and runs with the change, the cache is clear.
    - If the build fails with a "...does not exist in the current context" error, the build system is still not processing the XAML file correctly.

---

## Font Icon Rendering Issues

**Symptom:**
- A `FontImageSource` is used for a `Button` or `Image`, but the icon does not appear in the running application.
- The space for the icon might be reserved, but the icon itself is invisible.

**Troubleshooting Steps:**

1.  **Verify Font File and Registration:**
    - Ensure the font file (e.g., `.ttf`) exists in the `Resources/Fonts` directory.
    - Confirm the `.csproj` file includes the font with `<MauiFont Include="Resources\Fonts\*" />`.
    - Confirm `MauiProgram.cs` registers the font with a specific alias (e.g., `fonts.AddFont("myfont.ttf", "MyFontAlias");`).
    - The `FontFamily` property in the XAML must exactly match the alias.

2.  **Isolate the Problem with a Known-Good Icon:**
    - If you are unsure about a specific icon's code (glyph), temporarily switch to a very common one like "home" to test the rendering pipeline itself. If the "home" icon appears, the pipeline is working and the issue is with the specific glyph you want to use.

3.  **Check for Color Contrast Issues:**
    - **Symptom:** The icon appears briefly during a click animation but is otherwise invisible.
    - **Cause:** The `FontImageSource`'s `Color` property is the same as the parent `Button`'s `BackgroundColor`.
    - **Diagnosis:** Inspect the `Style` for the control (e.g., `Button`) in `Styles.xaml` to find its default `BackgroundColor`. Compare this with the `Color` being set on the `FontImageSource`.
    - **Resolution:** Explicitly set the `FontImageSource.Color` to a value with high contrast against the button's background (e.g., `White` for a dark button).

4.  **Verify the Glyph Codepoint:**
    - **Symptom:** A known-good icon (like "home") appears, but the desired icon (like "heart") does not, even after fixing colors.
    - **Cause:** The hexadecimal codepoint for the glyph is incorrect for the specific version of the font being used.
    - **Resolution:** Find a reliable, official "cheatsheet" for the exact font library and version you are using (e.g., from `pictogrammers.com` for the community Material Design Icons). Do not assume codepoints are stable between major versions or different font families (e.g., Google's Material Icons vs. Pictogrammers' MDI). In this project, the solid 'heart' glyph (`f2d1`) failed to render, but 'heart-outline' (`F02D5`) worked correctly. The 'share-variant' icon uses `F0497`.

---

## .NET 9 Deprecations & Modernization

**Symptom:**
- XAML warnings (green squigglies) indicating that certain controls or properties are obsolete in .NET 9.

**Issues & Resolutions:**

1.  **`Frame` is Obsolete:**
    - **Resolution:** Use the `Border` control instead.
    - **Migration:**
        - `Frame.CornerRadius` -> `Border.StrokeShape="RoundRectangle <radius>"`
        - `Frame.HasShadow` -> `Border.Shadow` (add a `Shadow` object).
        - `Frame.BorderColor` -> `Border.Stroke`.
        - `Frame.Padding` is supported on `Border`.
        - Note: `Border` requires an explicit `Background` (usually an `AppThemeBinding`) to match `Frame`'s adaptive behavior.

2.  **`AndExpand` Layout Options Deprecated:**
    - **Symptom:** Warnings on `HorizontalOptions="FillAndExpand"` or `VerticalOptions="CenterAndExpand"`.
    - **Cause:** Expansion options were used in the original `StackLayout` to manage how extra space was distributed. In modern `VerticalStackLayout` and `HorizontalStackLayout`, these options are ignored.
    - **Resolution:** Use `Fill`, `Start`, `Center`, or `End` without the `AndExpand` suffix. If precise space distribution is needed, use a `Grid`.

---

## Microsoft.OpenApi TypeLoadException (Swagger Integration)

**Symptom:**
- Application builds successfully, but at runtime, navigating to `/swagger` or any endpoint configured with `.WithOpenApi()` results in a `System.TypeLoadException`.
- The exception message typically indicates: `Could not load type 'Microsoft.OpenApi.Models.OpenApiOperation' from assembly 'Microsoft.OpenApi, Version=2.3.0.0...'`.
- This occurs despite explicit `PackageReference` for `Microsoft.OpenApi 2.3.0` being present.

**Cause:**
- A versioning conflict (often referred to as "dependency hell") where different NuGet packages (specifically `Swashbuckle.AspNetCore` and `Microsoft.AspNetCore.OpenApi` in this instance) implicitly or explicitly depend on *different* major or incompatible minor versions of the `Microsoft.OpenApi` assembly.
- Even if a direct `PackageReference` for the desired `Microsoft.OpenApi` version is added, if a transitive dependency *still* resolves to an older, incompatible version during assembly loading, the `TypeLoadException` can occur at runtime.
- In this specific case, `Microsoft.AspNetCore.OpenApi` 9.0.0 (for .NET 9) expected `Microsoft.OpenApi` 2.3.0, but `Swashbuckle.AspNetCore` 10.0.1 had an internal dependency chain that conflicted or was not fully compatible, causing runtime issues.

**Troubleshooting & Resolution:**

1.  **Identify Conflicting Versions:** The `TypeLoadException` message itself (e.g., `Version=2.3.0.0`) helps identify the problematic assembly and the version being *requested*.
2.  **Inspect Transitive Dependencies:** Use tools like `dotnet list package --include-transitive` (or examine `obj/project.assets.json`) to see the full dependency graph and identify which packages are pulling in which `Microsoft.OpenApi` versions.
3.  **Align Compatible Versions:** The most robust solution is to align all conflicting dependencies to a single, compatible version.
    - Attempting to force the higher version (`Microsoft.OpenApi 2.3.0`) directly did not resolve the `TypeLoadException`.
    - **Resolution:** Downgraded `Swashbuckle.AspNetCore` from `10.0.1` to `6.5.0`. This specific version of `Swashbuckle.AspNetCore` transitively depends on `Microsoft.OpenApi` 1.6.x, which proved compatible with `Microsoft.AspNetCore.OpenApi` 9.0.0 and resolved the `TypeLoadException`.
    - *(Note: Future versions of `Swashbuckle.AspNetCore` and `Microsoft.AspNetCore.OpenApi` may resolve this compatibility issue. This resolution is specific to the versions at hand and the .NET 9 target framework.)*

**Verification:**
- Application builds cleanly after package version adjustments.
- Running the application and navigating to the `/swagger` endpoint successfully loads the Swagger UI without runtime exceptions.
- The `/diagnostics/openapi-version` endpoint (if implemented) confirms the expected `Microsoft.OpenApi` assembly version is loaded.

---

## Android Emulator Network & Data Binding Issues

**Symptom:**
- The application runs on the Android emulator but displays no data from the local API (e.g., blank fields or missing images).
- No error alerts are displayed (silent failure), or "Connection Refused" errors occur.
- The API is running locally and accessible via browser on `localhost:7071`.

**Causes & Solutions:**

1.  **The "Localhost" Loopback Trap:**
    - **Cause:** On the Android emulator, `localhost` (127.0.0.1) refers to the *emulator device itself*, not the host machine running the API.
    - **Solution:** Use `10.0.2.2` to refer to the host machine's loopback interface.
    - **Implementation:** Add logic in `MauiProgram.cs` to detect `DevicePlatform.Android` and replace "localhost" with "10.0.2.2" in the Base URL.

2.  **Cleartext Traffic Blocking:**
    - **Cause:** Android 9+ (API 28+) blocks non-SSL (HTTP) traffic by default. Local development usually runs on HTTP.
    - **Solution:** Explicitly allow cleartext traffic in the Android Manifest.
    - **Implementation:** Add `android:usesCleartextTraffic="true"` to the `<application>` tag in `AndroidManifest.xml`.

3.  **JSON Deserialization Mismatch:**
    - **Cause:** The API returns JSON with `snake_case` keys (e.g., `image_url`), but the C# model uses `PascalCase` properties (e.g., `ImageUrl`).
    - **Solution:** Use `[JsonPropertyName("name")]` attributes or configure the `JsonSerializerOptions` to be case-insensitive.
    - **Implementation:** We applied `[JsonPropertyName]` attributes to `ContentItem` for explicit mapping.

4.  **Layout Collapse (CollectionView vs ScrollView):**
    - **Cause:** Nesting a `CollectionView` inside a `ScrollView` often causes the `CollectionView` to render with zero height because it has infinite scrollable space.
    - **Solution:** Do not nest them. Use the `CollectionView.Header` property to hold the top-of-page content (like hero cards) so the entire page scrolls as a single virtualized list.

---

## Physical Device Connectivity (Android)

**Symptom:**
- Application works in the Android Emulator but fails to load data when deployed to a physical Android device.
- Browser on the phone cannot reach `http://localhost:7071/api/...` or `http://<PC-IP>:7071/api/...`.
- `adb reverse tcp:7071 tcp:7071` does not seem to work reliably (Site Can't Be Reached).

**Causes & Solutions:**

1.  **"Localhost" is Local to the Device:**
    - **Cause:** On a physical phone, `localhost` refers to the phone itself. It cannot "see" the PC's `localhost` without a tunnel or explicit IP.
    - **Solution:** Use the PC's actual LAN IP address (e.g., `10.0.0.x`) instead of localhost.
    - **Implementation:**
        - In `MauiProgram.cs`, detect `DeviceInfo.DeviceType == DeviceType.Physical` and swap `localhost` for the hardcoded PC IP (for development only).
        - *Long term:* Host the API in the cloud.

2.  **Azure Functions Host Binding:**
    - **Cause:** By default, `func start` or `dotnet run` binds only to `127.0.0.1` (localhost). It rejects external connections from the LAN (Wi-Fi).
    - **Solution:** Force the host to listen on all interfaces (`0.0.0.0`).
    - **Implementation:** Update `launchSettings.json` command line arguments: `--port 7071 --host 0.0.0.0`.

3.  **Windows Firewall Blocking:**
    - **Cause:** Even with the host listening on `0.0.0.0`, Windows Firewall drops inbound TCP packets from the local network (subnet).
    - **Solution:** Create a specific Inbound Rule.
    - **Command (Admin PowerShell):** `New-NetFirewallRule -DisplayName "Allow Aurora API Port 7071" -Direction Inbound -LocalPort 7071 -Protocol TCP -Action Allow`

4.  **UI Crash on Startup (DisplayAlert):**
    - **Symptom:** The app crashes immediately on launch with `TargetInvocationException` if the network is unreachable.
    - **Cause:** Calling `DisplayAlert` from a background thread (the `catch` block of an async data fetch) or too early in the lifecycle causes a crash on Android.
    - **Solution:** Wrap UI interactions in `MainThread.BeginInvokeOnMainThread`.

---

## Missing Platform Entry Points (CI Build Failure / CS5001)

**Symptom:**
- CI build fails on GitHub Actions with `error CS5001: Program does not contain a static 'Main' method suitable for an entry point` for `net9.0-ios`, `net9.0-maccatalyst`, and `net9.0-windows` targets.
- The local build in Visual Studio works perfectly.
- The `src/Aurora/Platforms` folder contains the necessary `Program.cs` and `App.xaml` files locally.

**Cause:**
- The `.gitignore` file contained a rule `platforms/`, intended to ignore build output or template artifacts.
- However, since git is case-insensitive on Windows (default configuration), this rule matched and ignored the source directory `src/Aurora/Platforms`.
- Consequently, the entry point files (`Program.cs`, `App.xaml.cs`, etc.) were never committed to the repository, causing the clean CI build to fail.

**Resolution:**
1.  **Modify .gitignore:** Remove or comment out the `platforms/` line.
2.  **Force Add:** Use `git add src/Aurora/Platforms` to stage the previously ignored files.
3.  **Verification:** The CI build passed successfully after these files were pushed.

---

## XAML Formatting Limitations

**Symptom:**
- Running `dotnet format` standardizes C# files but leaves XAML files (`.xaml`) with inconsistent indentation, extra blank lines, or erratic attribute alignment.
- `.editorconfig` rules for indentation (e.g., `indent_style = tab`) do not seem to be automatically applied to XAML files during CLI-based formatting.

**Cause:**
- `dotnet format` primarily utilizes the Roslyn compiler and is designed for C# and MSBuild project files. It does not natively include a robust XAML/XML formatter.
- Standard `.editorconfig` support in the .NET CLI is currently limited for XAML-specific rules compared to its deep integration with C#.

**Resolution:**
1.  **Manual Oversight:** Carefully review XAML changes to ensure they match the project's tab-based indentation standard.
2.  **Visual Studio Tools:** Use Visual Studio's "Format Document" (`Ctrl+K, Ctrl+D`) which has better built-in XAML awareness, or consider a dedicated extension like **XAMLStyler** for team-wide consistency.
3.  **Atomic Rewrites:** When correcting erratic XAML formatting, a full file rewrite (ensuring consistent tabs) is often more reliable than incremental line-based replacements.

---

## Variable Font Rendering Issues (Android)

**Symptom:**
- A variable font (e.g., `Nunito-VariableFont_wght.ttf`) is registered and used in the application.
- Setting `FontAttributes="Bold"` has no visible effect, or the text appears much thinner than expected (Regular weight).
- The same font renders correctly on Windows but fails on Android emulators or physical devices.

**Cause:**
- The Android font renderer (specifically in certain versions of MAUI or the OS) may fail to correctly interpolate the weight axis of a variable font when requested via the standard `FontAttributes` enum.
- The renderer often falls back to the "Regular" (400) weight because it cannot resolve the "Bold" (700) coordinate within the variable file.

**Resolution:**
1.  **Use Static Fonts:** Instead of a single variable font file, import the full suite of static `.ttf` files (e.g., `Nunito-Regular.ttf`, `Nunito-Bold.ttf`, `Nunito-ExtraBold.ttf`).
2.  **Explicit Registration:** Register each weight with a unique alias in `MauiProgram.cs`:
    - `fonts.AddFont("Nunito-Bold.ttf", "NunitoBold");`
3.  **Explicit Application:** Use the alias directly in XAML and **remove** `FontAttributes="Bold"`, as the font file itself is already bold:
    - `<Label FontFamily="NunitoBold" ... />`
4.  **Verification:** This ensures the renderer loads the exact glyph shapes for that weight without attempting interpolation, providing a robust and consistent look across all platforms.

---

## API JSON Structure Mismatch (Deserialization Failures)

**Symptom:**
- The application makes an API call (e.g., `POST /react`), and the backend logs show a successful `200 OK` response with a JSON body.
- However, the client application either crashes, rolls back state, or fails to update the UI with the returned values.
- No explicit `JsonException` is visible in the console if caught by a broad `catch` block.

**Cause:**
- The client code uses `ReadFromJsonAsync<T>()` expecting a primitive type (e.g., `int`), but the API returns a structured JSON object (e.g., `{"uplift_count": 5}`).
- Deserializing a JSON object into a primitive `int` fails silently or throws an exception depending on the serializer configuration.

**Resolution:**
- Create a specific DTO or a `sealed record` that matches the JSON structure returned by the API.
- Use `[JsonPropertyName]` attributes to map snake_case JSON keys to PascalCase C# properties.
- Ensure the client calls `ReadFromJsonAsync<MyResponseDto>()` and extracts the value from the DTO.

---

## CollectionView Item Refresh (Static vs. Reactive UI)

**Symptom:**
- An item within a `CollectionView` (like a "Daily Pick" card) is updated in code-behind, but the UI does not reflect the change.
- Re-assigning the item to the `ObservableCollection` (e.g., `myList[index] = item`) does not trigger a refresh.

**Cause:**
- The data model (e.g., `ContentItem`) is a standard C# class and does not implement `INotifyPropertyChanged`.
- `CollectionView` and `ObservableCollection` only listen for *collection* changes (add/remove/reset), not for property changes on the objects *within* the collection.
- Re-assigning the same object reference to the same index in an `ObservableCollection` is often optimized away by the UI engine and does not force a re-render.

**Resolution:**
- **Best Practice:** Implement `INotifyPropertyChanged` on the data model. Ensure the setters call `OnPropertyChanged()`. This allows the UI binding engine to react directly to property changes without any manual collection manipulation.
- **Alternative:** If the model cannot be changed, you must replace the item with a *new* object instance (clone) to force the `ObservableCollection` to signal a change, though this is less efficient and can cause visual flickers.

---

## Azurite Not Starting in Multiple Startup Projects

**Symptom:**
- API calls requiring storage (e.g., Reactions) fail with "Connection Refused" (127.0.0.1:10002).
- Azurite process is not running, despite being configured in "Connected Services".
- "Service Dependencies" output pane is empty.

**Cause:**
- Visual Studio only reliably triggers the "Connected Services" initialization (which starts Azurite) for the projects at the top of the startup list. If a project without the dependency (like the MAUI UI project) is listed first, the orchestrator may skip or delay the auxiliary service startup.

**Resolution:**
- Open **Solution Properties > Startup Project**.
- Ensure the **`Aurora.Api`** project is moved to the **top** of the "Multiple Startup Projects" list.
- This ensures the API's service dependencies (Azurite) are fully initialized before the UI layer attempts to communicate with the backend.

---

## Azure App Service Plan Quota Limitations (New Accounts)

**Symptom:**
- Azure deployment fails with `SubscriptionIsOverQuotaForSku` error
- Error message shows: `Current Limit (Dynamic VMs): 0` or `Current Limit (Basic VMs): 0`
- Deployment requires 1 VM, but quota shows `0 of 0` available
- Occurs even after upgrading from Free Trial to Pay-As-You-Go subscription

**Example Error:**
```
SubscriptionIsOverQuotaForSku
Current Limit (Dynamic VMs): 0
Current Usage: 0
Amount required for this deployment (Dynamic VMs): 1
```

**Cause:**
- **New Pay-As-You-Go accounts have zero quota for all App Service Plan tiers by default**
- This is an anti-fraud measure by Microsoft to prevent abuse on newly created accounts
- Upgrading from Free Trial to Pay-As-You-Go does NOT automatically grant compute resource quotas
- Quotas must be explicitly requested via Azure Portal or Support

**Affected Resources:**
- Azure Functions (Consumption Plan / Y1 SKU) - "Dynamic VMs"
- App Service Plans (Basic B1, Standard S1, Premium P1v2, etc.)
- All tiers start at 0 quota until manually approved

**Resolution:**

1. **Access Azure Quotas Portal:**
   - Azure Portal → Search for "Quotas" → Click **Quotas** service
   - Select **Provider:** "App Service (Public Preview)"

2. **Identify the Required Quota:**
   - For **Consumption Plan (serverless)**: Look for "Y1 VMs" or "Total Regional VMs"
   - For **Basic/Standard/Premium tiers**: Look for specific SKU (e.g., "B1 VMs", "S1 VMs")
   - Quota will show `0 of 0` for the target region (e.g., East US)

3. **Request Quota Increase:**
   - Click the **edit/pen icon** next to the quota row
   - Enter new limit (e.g., 3 for Consumption Plan, 10 for production)
   - Provide justification: "Need quota for Azure Functions deployment. Upgraded to Pay-As-You-Go, currently have 0 quota."
   - Submit request

4. **Approval Timeline:**
   - Small requests (1-10 instances) for Consumption/Basic tiers: **Minutes to 1-2 hours** (often auto-approved)
   - Larger requests or Premium tiers: Up to 48 hours (manual review)
   - Check email for approval notification

5. **Verification:**
   - Return to Quotas page
   - Quota should update from `0 of 0` to `0 of <requested-limit>` (e.g., `0 of 3`)
   - Retry deployment - should succeed

**Important Notes:**
- **"Total Regional VMs" is informational only** - According to Microsoft's documentation, you should NOT request increases for this quota directly. It automatically adjusts when individual SKU quotas are approved.
- Quota requests are **per region** - East US quota does not apply to West US
- Consumption Plan quota is the most cost-effective (free tier, pay-per-execution)
- If blocked, consider requesting Consumption Plan (Y1) first for immediate free deployment

**Alternative (Temporary Workaround):**
If you cannot wait for quota approval and need immediate deployment:
- Deploy to a **different Azure region** that may have available quota
- Note: This is unlikely on brand new accounts; quota is subscription-wide, not region-specific

**Documentation References:**
- Azure Quotas Portal: [Quickstart - Request quota increase](https://learn.microsoft.com/en-us/azure/quotas/quickstart-increase-quota-portal)
- New quota UI announcement: [App Service Quota Self-Service Experience](https://techcommunity.microsoft.com/blog/appsonazureblog/announcing-the-public-preview-of-the-new-app-service-quota-self-service-experien/4450415)

