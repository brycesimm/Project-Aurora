# Milestone E: Core Interactions & Polish

**Status:** ✅ Completed
**Completion Date:** 2025-12-25

*This milestone transforms the application from a read-only prototype into an interactive experience, completing the core "Uplift" loop and ensuring visual consistency.*

---

## Feature E-01: User Reactions
*This feature enables the anonymous "Uplift" reaction system, requiring backend state persistence.*

- [x] **Story E-01.1:** As a developer, I need to update the data model to track reaction counts so that the UI can display how many people have been uplifted.
    - **AC 1:** `ContentItem.cs` includes a new integer property `UpliftCount`.
    - **AC 2:** `content.schema.json` is updated via `SchemaBuilder` to include `uplift_count`.
    - **AC 3:** `sample.content.json` is updated with mock values for testing.
- [x] **Story E-01.2:** As a developer, I need a backend storage service to persist reaction counts so that data survives app restarts.
    - **AC 1:** `Aurora.Api` includes the `Azure.Data.Tables` package.
    - **AC 2:** A `TableStorageService` is implemented to Handle `Get` and `Upsert` operations for reaction entities.
    - **AC 3:** The service is configured to use Azurite (local emulator) for development.
- [x] **Story E-01.3:** As a developer, I need an API endpoint to handle reactions so that the client can submit user feedback.
    - **AC 1:** A new HTTP POST endpoint `/api/articles/{id}/react` is created.
    - **AC 2:** The endpoint increments the count in Table Storage and returns the new total.
    - **AC 3:** The endpoint is anonymous (no user login required).
- [x] **Story E-01.4:** As a user, I want to click the "Uplift" button to register my positive sentiment and see the count increase.
    - **AC 1:** The "Uplift" button in `MainPage` is wired to the `ContentService`.
    - **AC 2:** Clicking the button immediately increments the count visually (optimistic update).
    - **AC 3:** The app sends the request to the API in the background.
    - **AC 4:** If the API fails, the count is rolled back or an error is logged (silent failure preferred for UX).

## Feature E-02: Native Sharing
*This feature leverages the device's native capabilities to share content externally.*

- [x] **Story E-02.1:** As a user, I want to share a story with friends so that I can spread positivity.
    - **AC 1:** A "Share" icon button is added to the "Vibe of the Day" card and each "Daily Pick".
    - **AC 2:** Clicking the button opens the native OS share sheet (iOS/Android).
    - **AC 3:** The shared content includes the Story Title and the Article URL.

## Feature E-03: Visual Design System
*This feature covers the definition and implementation of a cohesive visual language.*

- [x] **Story E-03.1: Define Design System & Palette**
    - **AC 1:** `docs/DESIGN_SYSTEM.md` is populated with a finalized color palette (Primary, Background, Surface) for Light and Dark modes.
    - **AC 2:** Typography scale (Headings, Body, Caption) is defined and documented.
    - **AC 3:** Corner radius and spacing standards are defined.
- [x] **Story E-03.2: Implement Color & Theme Resources**
    - **AC 1:** `Colors.xaml` is updated with the new palette, replacing default MAUI colors.
    - **AC 2:** Semantic resource keys (e.g., `PrimaryBrush`, `SurfaceBrush`) are created to abstract raw hex values.
    - **AC 3:** `Styles.xaml` is updated to apply these resources globally to common controls.
- [x] **Story E-03.3: Apply Design System to Main Page**
    - **AC 1:** The "Vibe of the Day" card uses the new Surface color, corner radius, and shadow elevation.
    - **AC 2:** Typography styles are applied to all Labels (Hero Title vs. Body Text).
    - **AC 3:** Buttons (Uplift, Share) use the new primary/secondary styling.
    - **AC 4:** Spacing (Margins/Padding) is audited to match the defined grid (e.g., 8pt multiples).
- [x] **Story E-03.4: Visual Refinements (Mobile Verification)**
    - **AC 1:** Title font sizes and weights are adjusted for better legibility on physical devices.
    - **AC 2:** Paragraph line heights and spacing are tuned for comfortable reading on small screens.
    - **AC 3:** Button icons and text labels are aligned and spaced correctly to avoid cramping.
