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
    - **Resolution:** Find a reliable, official "cheatsheet" for the exact font library and version you are using (e.g., from `pictogrammers.com` for the community Material Design Icons). Do not assume codepoints are stable between major versions or different font families (e.g., Google's Material Icons vs. Pictogrammers' MDI). In this project, the solid 'heart' glyph (`f2d1`) failed to render, but 'heart-outline' (`F02D5`) worked correctly.
