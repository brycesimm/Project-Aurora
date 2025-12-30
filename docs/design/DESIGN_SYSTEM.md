# Project Aurora - Design System

This document defines the visual language and user interface standards for Project Aurora.

## 1. Core Philosophy
**"Cozy, Warm, & Uplifting"**
The interface should feel like a comfortable refuge. We avoid harsh contrasts, sharp corners, and clinical whites. Animations are soft; colors are pastel and soothing.

---

## 2. Typography
**Font Family:** [Nunito](https://fonts.google.com/specimen/Nunito) (Rounded Sans-Serif)
*Why Nunito? It has rounded stroke terminals that give it a friendly, approachable, and "soft" character without sacrificing legibility.*

### Scale
- **Display (Hero Title):** Nunito ExtraBold, 24sp
- **Heading 1 (Section Headers):** Nunito ExtraBold, 20sp
- **Body 1 (Article Text):** Nunito SemiBold, 16sp, Line Height 1.25
- **Body 2 (Secondary/Metadata):** Nunito Regular, 14sp
- **Button:** Nunito ExtraBold / Black, 14-16sp

---

## 3. Color Palette ("Morning Mist")

### Light Mode (Cozy Day)
- **Background:** `#FAF9F6` (Alabaster - A warm, off-white)
- **Surface (Cards):** `#FFFFFF` (Pure White)
- **Primary (Action):** `#7986CB` (Periwinkle Blue - Soft, calming indigo)
- **OnPrimary (Text on Btn):** `#FFFFFF`
- **Text Primary:** `#37474F` (Blue Grey 800 - Softer than black)
- **Text Secondary:** `#78909C` (Blue Grey 400)

### Dark Mode (Soft Night)
- **Background:** `#263238` (Blue Grey 900 - Deep, warm charcoal)
- **Surface (Cards):** `#37474F` (Blue Grey 800)
- **Primary (Action):** `#9FA8DA` (Indigo 200 - Lighter periwinkle for contrast)
- **OnPrimary:** `#263238`
- **Text Primary:** `#FFFFFF` (Pure White - High Contrast)
- **Text Secondary:** `#E0E0E0` (Light Gray - High Contrast)

### Semantic Accents (Pastels)
- **Uplift (Heart):** `#F48FB1` (Pastel Pink)
- **Vibe Accent:** `#FFCC80` (Sun-Kissed Apricot)
- **Share:** `#80CBC4` (Pastel Teal)
- **Comment:** `#CE93D8` (Lavender Mist)

---

## 4. Shapes & Layout

### Corner Radii
*Everything is soft.*
- **Cards/Containers:** `16dp`
- **Buttons:** `22-24dp` (Pill/Circle)
- **Images:** `12dp` (Consistent with inner borders)

### Button Style: "Tinted Outline"
Buttons use a colored pastel background with a darker, high-contrast 2.0dp border (Stroke) and matching text/icon color. This provides an illustrative, comic-book-like clarity.

### Spacing (8pt Grid)
- **Small:** `8dp` (Standard padding)
- **Medium:** `16dp` (Card padding, separation)
- **Large:** `24dp` (Section separation)

### Elevation (Shadows)
- **Soft Shadow:** `Offset: 0, 4`, `Radius: 10`, `Opacity: 0.1` (Subtle lift, no harsh drops)

---

## 5. Iconography
- **Set:** Material Design Icons (MDI)
- **Style:** Rounded variants where available.

---

## 6. App Icon & Branding

### App Icon
<img src="../../src/Aurora/Resources/AppIcon/appicon.png" alt="Aurora App Icon" width="256" height="256" />

### App Icon Specifications
- **Source File:** `src/Aurora/Resources/AppIcon/appicon.png`
- **Dimensions:** 2048 x 2048 pixels (square, 1:1 aspect ratio)
- **Format:** PNG with RGB color (8-bit/color)
- **File Size:** ~4.4 MB source (auto-optimized by MAUI resizetizer for deployment)
- **BaseSize Configuration:** `1536x1536` (75% fill ratio)
  - Provides appropriate padding for Android adaptive icons
  - Prevents cropping on circular/squircle icon shapes
  - Can be adjusted in `Aurora.csproj` via `<MauiIcon BaseSize="1536,1536" />`

### Design Guidelines
- **Safe Zone:** Logo design should remain recognizable within the central 75% area
- **Padding:** 25% margin (512px on each edge) accounts for platform-specific icon masking
- **Color Profile:** Should complement Morning Mist palette (pastels, warm tones)
- **Scalability:** Design must be recognizable at small sizes (48dp launcher icons)

### Platform-Specific Rendering
- **Android:** MAUI generates adaptive icon layers (foreground + background)
  - Supports circular, squircle, rounded square masks
  - Icon automatically scaled to `mdpi`, `hdpi`, `xhdpi`, `xxhdpi`, `xxxhdpi` densities
- **iOS:** Generated at 1024x1024 for App Store + various sizes for device home screens
- **Windows/macOS:** Generated at appropriate sizes for each platform

### Configuration Reference
```xml
<!-- Aurora.csproj -->
<MauiIcon Include="Resources\AppIcon\appicon.png" BaseSize="1536,1536" />
```

**Adjustment Guidelines:**
- Too much cropping? Reduce BaseSize to `1433,1433` (70% fill)
- Too much padding? Increase BaseSize to `1638,1638` (80% fill)