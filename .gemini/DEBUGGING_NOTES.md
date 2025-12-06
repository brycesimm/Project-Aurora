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
