# Project Aurora - Backlog

This document tracks the features, user stories, and tasks for Project Aurora. It is organized by milestones defined in `PLANNING.md`.

**Note:** Completed milestones have been archived to keep this document focused on active and planned work. See [Completed Milestones](#completed-milestones-archive) below for navigation to historical work.

---

## Completed Milestones (Archive)

The following milestones have been completed and archived for reference:

| Milestone | Status | Completion Date | Archive Location |
|-----------|--------|-----------------|------------------|
| **Milestone A:** MAUI Onboarding + Shell with Mock Data | ✅ Complete | 2025-12-07 | [backlog/completed/milestone-a-maui-onboarding.md](backlog/completed/milestone-a-maui-onboarding.md) |
| **Milestone B:** Backend/API + Curation Workflow | ✅ Complete | 2025-12-12 | [backlog/completed/milestone-b-backend-api.md](backlog/completed/milestone-b-backend-api.md) |
| **Milestone C:** Live Data Integration & Core Features | ✅ Complete | 2025-12-21 | [backlog/completed/milestone-c-live-data.md](backlog/completed/milestone-c-live-data.md) |
| **Milestone D:** Robust Testing Framework & Service Testability | ✅ Complete | 2025-12-23 | [backlog/completed/milestone-d-testing-framework.md](backlog/completed/milestone-d-testing-framework.md) |
| **Milestone E:** Core Interactions & Polish | ✅ Complete | 2025-12-25 | [backlog/completed/milestone-e-core-interactions.md](backlog/completed/milestone-e-core-interactions.md) |
| **Milestone V-0:** Beta Readiness | ✅ Complete | 2025-12-28 | [backlog/completed/milestone-v-0-beta-readiness.md](backlog/completed/milestone-v-0-beta-readiness.md) |

---

## Milestone V-2: Beta Testing Round 1 (Early Alpha Quality)

**Objective:** Establish beta testing infrastructure and execute the first round of validation (self-testing + 1-4 trusted testers) to identify improvements and validate Aurora's core value proposition.

**Target Completion:** 2 weeks from start

**Success Criteria:**
- ✅ Google Forms created and integrated into app for structured feedback
- ✅ Google Play Internal Testing track operational with professional distribution
- ✅ 7-14 day self-validation completed with structured findings documented
- ✅ 1-4 external testers recruited and onboarded (optional, based on availability)
- ✅ Improvement backlog documented with prioritization (Critical/High/Medium/Low)
- ✅ Go/No-Go decision made: Continue refining Aurora OR Pivot substantially

**Platform Scope:** Android-only (iOS explicitly deferred pending Mac/device acquisition)

**Testing Philosophy:** This is early-stage beta testing (alpha quality) focused on discovering critical gaps and validating core value proposition with controlled audience before broader distribution.

**Terminology Note:** We use "beta" for consistency with Azure infrastructure naming (`rg-aurora-beta`), but this milestone represents alpha-quality testing with a small, trusted cohort.

---

### Feature V-2.1: Feedback Collection Infrastructure
*This feature establishes structured surveys to capture quantitative and qualitative insights without survey fatigue.*

- [x] **Story V-2.1.1:** As a project owner, I want a baseline survey that captures tester context and expectations so that I can establish a pre-use benchmark for measuring Aurora's impact.
    - **AC 1:** ✅ Google Form created with 14 questions (expanded from 8) covering: contact info (method + details), app usage patterns, social media context, sentiment analysis, positivity definition, and value proposition validation
    - **AC 2:** ✅ Most questions configured as optional (3 required: contact method, contact details, value proposition rating)
    - **AC 3:** ✅ Questions with simple answers include follow-up text box: "Want to elaborate? (Optional)" for detailed feedback
    - **AC 4:** ✅ Form configured to save responses to Google Sheets automatically with timestamp
    - **AC 5:** ✅ Shareable link generated (`https://forms.gle/eRq3qY1EveJbP6Q87`) and documented in `docs/beta/SURVEY_LINKS.md`
    - **AC 6:** ✅ Form tested with 2 test responses, verified data appears correctly in Google Sheets with all columns populated
    - **AC 7:** 🔄 Deferred to Story V-2.2.1 (Beta Tester Guide not yet created)
    - **Completed:** 2025-12-29
    - **Notes:** Survey expanded beyond original spec to include Google Play Store email collection (1 required question) and enhanced app usage analysis (Q4-Q6, Q14). Email-only contact simplifies tester onboarding (email = whitelist identifier = communication channel). Updated 2025-12-31 to remove Discord/Reddit options for streamlined workflow.
    - **Edge Cases:**
        - Tester skips optional questions: Allowed; only value proposition question (#6 in proposed list) is required
        - Political sensitivity (topics to avoid): Question phrased neutrally: "Are there topics you'd prefer Aurora avoid, even if generally positive? (e.g., political figures, religious themes, specific social issues)"
        - Google account required: Configure form to NOT require sign-in (Settings → Responses → "Limit to 1 response" unchecked)
    - **Survey Questions (Final):**
        1. "On average, how many hours per day do you spend on social media or news apps?" (Scale: <1, 1-2, 2-4, 4-6, 6+) + Optional text: "Want to elaborate?"
        2. "How does social media/news consumption typically make you feel?" (Multiple choice: Informed, Anxious, Entertained, Angry, Hopeful, Drained, Other) + Optional text
        3. "How often do you avoid news or social media due to negative content?" (Scale: Never → Always) + Optional text
        4. "What types of stories do you consider 'positive'? Select all that apply:" (Checkboxes: Scientific breakthroughs, Environmental progress, Human kindness, Animal welfare, Social justice, Economic good news, Health/medical advances, Arts & culture, Other)
        5. "Are there topics you'd prefer Aurora avoid, even if generally positive?" (Open text, optional)
        6. **"How valuable would an app focused exclusively on uplifting news be to you?"** (Scale: Not valuable → Extremely valuable) **[REQUIRED]**
        7. "What app do you typically open first when you pick up your phone?" (Open text, optional)
        8. "How important is it that a news app has a mobile-optimized reading experience?" (Scale: Not important → Extremely important) + Optional text

- [x] **Story V-2.1.2:** As a project owner, I want a weekly feedback survey to track sentiment, usage patterns, and friction points during beta testing so that I can identify improvements iteratively.
    - **AC 1:** ✅ Google Form created with 12 questions (expanded to include standalone "Want to elaborate?" questions) covering: usage frequency, first-app-opened behavior, articles read, content resonance, sharing behavior, mood impact, UX friction (good/bad moments), bugs, and continuation intent
    - **AC 2:** ✅ All questions optional except "How many days this week did you open Aurora?" (allows tracking zero-usage weeks)
    - **AC 3:** ✅ Simple-answer questions include "Want to elaborate?" text boxes for detailed feedback (Q7 for mood, Q12 for continuation intent)
    - **AC 4:** ✅ Form configured to save responses to **separate Google Sheets** from baseline survey (new spreadsheet created)
    - **AC 5:** ✅ Shareable link generated and documented in `docs/beta/SURVEY_LINKS.md`
    - **AC 6:** ✅ Form tested with 2 test submissions, verified data separation from baseline responses
    - **AC 7:** 🔄 Deferred to Story V-2.2.1 (Beta Tester Guide not yet created)
    - **Completed:** 2025-12-29
    - **Notes:** Survey expanded from 10 to 12 questions by making "Want to elaborate?" standalone questions rather than conditional follow-ups. Changed Q6 to "Select all that apply" for better mood tracking. Question wording adjusted to include "Project Aurora" for clarity. Q11 changed from "next week" to general "would you continue" for broader applicability.
    - **Edge Cases:**
        - Tester skips a week: Weekly form is optional; no enforcement or reminders (low-pressure alpha environment)
        - Tester had zero usage that week: Question #1 allows "0 days" response; remaining questions can be skipped
        - Response volume too low for analysis: If <3 responses after 2 weeks, pivot to 1-on-1 interviews instead (document in findings)
    - **Survey Questions (Final):**
        1. **"How many days this week did you open Aurora?"** (Scale: 0, 1, 2-3, 4-5, 6-7) **[REQUIRED]**
        2. "Was Aurora the first app you opened when picking up your phone?" (Multiple choice: Never, Rarely, Sometimes, Often, Always) + Optional text
        3. "How many articles did you read in full this week?" (Scale: 0, 1-2, 3-5, 6-10, 10+) + Optional text
        4. "Which story resonated with you most this week?" (Open text, optional)
        5. "Did you share any stories from Aurora? If not, why?" (Open text, optional)
        6. "How did using Aurora make you feel this week?" (Multiple choice: More hopeful, Less anxious, Informed, Neutral, Disappointed, Other) + Optional text
        7. "What felt **good** when using Aurora this week?" (Open text, optional)
        8. "What felt **bad** or frustrating?" (Open text, optional)
        9. "Did you encounter any bugs or crashes?" (Yes/No + description if yes)
        10. "Would you continue using Aurora next week?" (Yes / Maybe / No) + "Why?" text box

- [x] **Story V-2.1.3:** As a beta tester, I want easy access to the feedback form from within Aurora so that I don't have to search for the link in emails or documentation.
    - **AC 1:** ✅ "Share Feedback" button added at bottom of Daily Picks list (CollectionView.Footer) for natural "end of content" placement
    - **AC 2:** ✅ Button uses "Morning Mist" design system: CommentAccent background, 2.0dp purple border, rounded pill shape (24dp corner radius), Nunito ExtraBold font
    - **AC 3:** ✅ Button icon: Material Design Icons `file-document-edit-outline` (U+F0DC9) with label "Share Feedback"
    - **AC 4:** ✅ Confirmation dialog before opening ("Would you like to provide feedback on your experience with Aurora?" → "Yes, Open Survey" / "Not Right Now")
    - **AC 5:** ✅ Tapping "Yes" opens weekly survey in Chrome Custom Tabs (BrowserLaunchMode.SystemPreferred)
    - **AC 6:** ✅ Configuration via nested `BetaSettings` object in appsettings.json with `IsBetaTesting` flag and `WeeklyFeedbackFormUrl`
    - **AC 7:** ✅ Button visibility controlled by `BetaSettings:IsBetaTesting` flag (production-ready: set false to hide)
    - **AC 8:** ✅ Graceful error handling with MainThread-safe alerts for invalid URLs or browser launch failures
    - **AC 9:** ✅ Threading fix: Button re-enable wrapped in `MainThread.BeginInvokeOnMainThread()` to prevent crash
    - **AC 10:** ✅ Zero-warning build maintained (Debug and Release configurations)
    - **AC 11:** ✅ Verified on Android emulator and S24 Ultra physical device (Wi-Fi + cellular)
    - **Completed:** 2025-12-29
    - **Enhancements Implemented:**
        - Beta mode toggle for clean production transition (no code changes required)
        - Polite confirmation dialog prevents accidental taps
        - `file-document-edit-outline` icon for semantic accuracy (editing/providing feedback)
        - Nested `BetaSettings` object enables future expansion (expiration dates, baseline survey URL, debug flags)
        - Custom Aurora logo integrated (2048x2048px with BaseSize="1536,1536" for optimal scaling)

---

### Feature V-2.2: Beta Tester Onboarding Documentation
*This feature provides clear, concise instructions for beta testers so they understand expectations and can successfully install/use Aurora.*

- [x] **Story V-2.2.1:** As a beta tester, I want a clear onboarding guide so that I understand Aurora's purpose, how to install it, and how to provide valuable feedback.
    - **AC 1:** `docs/beta/BETA_TESTER_GUIDE.md` created with sections: Welcome & Purpose, Installation, How to Provide Feedback, What We're Testing, Known Limitations
    - **AC 2:** Guide is 1-2 pages (300-500 words) in **conversational, warm tone** matching "Morning Mist" design philosophy (friendly, approachable, not clinical)
    - **AC 3:** All links clearly marked as placeholders until dependencies complete:
        - `[BASELINE SURVEY LINK]` (Story V-2.1.1)
        - `[WEEKLY SURVEY LINK]` (Story V-2.1.2)
        - `[GOOGLE PLAY INSTALL LINK]` (Story V-2.3.2)
    - **AC 4:** Guide includes time commitment transparency: "~5-10 min/day for 1-2 weeks, plus one 5-min survey weekly"
    - **AC 5:** Known Limitations section explicitly states: "Android-only for now; iOS planned for future," "Manual content curation (may lag on weekends)," "Minimal features intentionally (testing core concept)"
    - **AC 6:** Contact information section: Provide email (themetanoiasociety@gmail.com) for urgent bugs + reminder that forms are preferred for structured feedback
    - **AC 7:** Guide reviewed for clarity (no jargon, no assumptions about technical knowledge)
    - **AC 8:** PDF export created for easy email distribution: `docs/beta/BETA_TESTER_GUIDE.pdf`
    - **Edge Cases:**
        - Tester loses the guide: Email includes PDF attachment + link to GitHub docs (`https://github.com/.../BETA_TESTER_GUIDE.md`)
        - Links change: Guide lives in Git; can be updated and redistributed quickly
        - Non-technical tester confused by installation: Guide uses screenshots (add in V-2.3.2 after first successful install)
    - **Guide Structure (Final):**
        - **Section 1: Welcome & Purpose** (2-3 sentences: refuge from negative news, you're helping validate if this concept resonates)
        - **Section 2: Installation** (Step 1: Baseline survey, Step 2: Google Play opt-in link, Step 3: Open app and explore)
        - **Section 3: How to Provide Feedback** (Weekly survey every Sunday, In-app "Share Feedback" button, Email for urgent bugs)
        - **Section 4: What We're Testing** (Content quality, user experience, value proposition: "Does Aurora make you feel more hopeful?")
        - **Section 5: Known Limitations** (Android-only, manual curation, minimal features, early beta quality)
    - **Completed:** 2025-12-30
    - **Notes:** Guide significantly expanded beyond original 300-500 word spec (~1,400 words final) to address non-tech-savvy tester needs. Key additions: "What is Aurora?" value proposition section, "✅ You'll Know Aurora is Working If..." expected behavior guide, comprehensive device info instructions with manufacturer variations, beginner-friendly bug template with copy-paste format + filled example, article accessibility limitations (paywall disclosure), installation confirmation with Aurora logo (80x80px), response time expectations (24-48hr), flexible weekly survey timing, usage reassurance ("any feedback is good feedback"). PDF generated via Pandoc→HTML→browser Print-to-PDF with custom CSS (Nunito font, Morning Mist palette #7986CB, emojis throughout). Comprehensive review from non-tech-savvy perspective identified and resolved 11 critical gaps. Contact simplified to email-only (themetanoiasociety@gmail.com); Discord references removed per user preference. Survey links active (baseline/weekly forms from V-2.1.1/V-2.1.2); Google Play link referenced as "sent via email" pending V-2.3.2. All 8 ACs met; guide ready for email distribution.

- [x] **Story V-2.2.2:** As a developer, I want documented steps for generating signed AABs and distributing via Google Play so that I can repeat the process reliably for future beta rounds.
    - **NOTE:** This story **MOVED AFTER V-2.3.2** to capture lessons learned from first deployment.
    - **AC 1:** ✅ `docs/technical/DEPLOYMENT.md` created with step-by-step signing and upload instructions based on actual experience from Story V-2.3.2
    - **AC 2:** ✅ Keystore creation and **backup procedure** documented with explicit security warnings (3 secure locations: Bitwarden + cloud storage + external USB)
    - **AC 3:** ✅ AAB generation documented: PowerShell environment variable setup + `dotnet publish -c Release -f net9.0-android` command
    - **AC 4:** ✅ Google Play Internal Testing release process documented: Console navigation, AAB upload, release notes format, tester configuration, rollout steps
    - **AC 5:** ✅ Store listing configuration documented: app name, descriptions, icon upload, screenshots
    - **AC 6:** ✅ Process verified with Story V-2.3.2 successful deployment (com.metanoiasociety.aurora-Signed.aab uploaded, release "1 (1.0.0)" rolled out)
    - **Edge Cases:**
        - **Keystore lost:** Cannot update the app without losing all installs. Mitigation: Document backup to 3 locations (local, cloud, password manager) in bold/red text
        - AAB rejected by Google: Console provides reason; likely signing config issue. Document common errors: version code conflict, missing permissions, improper signing
        - Tester can't install: Check device compatibility (minimum Android version). Guide should list requirements: "Android 8.0+ (API 26)"
    - **Technical Considerations:**
        - Keystore password must be stored securely (never in Git)
        - Automation deferred: Manual process acceptable for beta (revisit GitHub Actions in Beta Round 2+ if needed)
        - AAB vs. APK: AAB is Google's preferred format (smaller download size, dynamic delivery benefits)
        - Document version code increment requirement: `<ApplicationVersion>1.0.1</ApplicationVersion>` in `Aurora.csproj` for each upload

---

### Feature V-2.3: Google Play Internal Testing Setup
*This feature establishes professional distribution infrastructure for reliable installs and automatic updates.*

- [x] **Story V-2.3.1:** As a developer, I want a Google Play Console account so that I can distribute Aurora via Internal Testing track without requiring testers to sideload APKs.
    - **NOTE:** **Execute this story EARLY in milestone** (Day 1-2) to mitigate 24-48 hour account approval wait time.
    - **AC 1:** ✅ Visit `play.google.com/console/signup` and create Google Play Developer account (Google account: themetanoiasociety@gmail.com)
    - **AC 2:** ✅ Pay $25 one-time registration fee (credit card required, non-refundable)
    - **AC 3:** ✅ Complete developer profile: Developer name "Metanoia Society" (PERMANENT), developer type (Individual), contact information
    - **AC 4:** ✅ Accept Google Play Developer Distribution Agreement (read terms, especially content policy)
    - **AC 5:** ✅ Account status "Active" - approval granted 2025-12-31 (phone verification completed same day)
    - **AC 6:** ✅ Create new application entry: **App Title:** "Aurora - Uplifting Media" (package name assigned on first AAB upload in V-2.3.2)
    - **AC 7:** ✅ Application created in draft state (Internal Testing only, not published)
    - **Progress Notes:**
        - ✅ Developer name research completed: "Metanoia Society" selected after trademark/domain availability analysis
        - ✅ Application submitted with detailed developer background (curating/aggregating positive news content)
        - ✅ Identity verification completed (app install to device)
        - ✅ Phone verification completed (2025-12-31)
        - ✅ Account approved (2025-12-31, ~24 hours after submission)
    - **Edge Cases:**
        - Account approval delayed beyond 48 hours: Contact Google Play support via Console help center (usually resolves within 72 hours)
        - Payment issues: Verify billing address matches credit card; use card that accepts international transactions (Google Ireland)
        - Name collision: If "Aurora" package name taken, use `com.projectaurora.positivenews` or append unique identifier
        - **Name change concern:** Package name (`com.projectaurora.app`) is **permanent** and locked after first publish; App Title ("Aurora - Positive News") can be changed anytime without penalty
    - **Technical Considerations:**
        - One-time cost: $25 USD (lifetime developer account, no renewal fees)
        - Required info: Legal name (individual or business), physical address, phone number, email
        - Account unlocks: Internal Testing (100 testers), Closed Testing (unlimited), Open Testing (unlimited), Production releases
        - Package name recommendation: Use `com.projectaurora.app` to match existing Azure infrastructure naming (`rg-aurora-beta`, `func-aurora-beta-*`)
    - **Time Estimate:** 30-60 minutes (form filling + payment) + 24-48 hours wait (approval)
    - **Decision Points:**
        - **Package Name:** Does `com.projectaurora.app` work, or prefer more generic like `com.positivity.newsapp` to future-proof against rebranding?
        - **Developer Name (Shown on Play Store - PERMANENT):** This appears as "by [Developer Name]" on the Play Store and **cannot be changed** after account creation. Options to consider:
            - **"Project Aurora"** - Clean, established brand identity
            - **"Lilly Light Studios"** - Honors Lilly ("my light in this world"), professional studio naming
            - **"Morning Light Studios"** - Combines Aurora (dawn/morning) with Lilly's light symbolism
            - **"Aurora Light Labs"** - Merges both Aurora and light themes professionally
            - **"Lilly Labs"** - Simple, memorable, honors Lilly directly
            - **Personal name** (e.g., "Bryce Simmerman") - Standard indie developer approach, keeps personal branding
        - **Note:** App can include "In loving memory of Lilly" dedication in About section regardless of developer name chosen
        - **Recommendation:** Create **separate Google account** for Play Console (not personal Gmail) to enable brand identity and isolation (see PLANNING.md "Deferred Business & Legal Items" for full rationale)

- [x] **Story V-2.3.2:** As a developer, I want Aurora deployed to Google Play Internal Testing so that beta testers receive professional installs and automatic updates.
    - **AC 1:** ✅ Generate signed Release AAB - Completed 2025-12-31
        - **Keystore generated:** `C:\Programming\Keystores\aurora-release.keystore`
            - Alias: `aurora`, Validity: 10,000 days (~27 years), Algorithm: RSA 2048-bit
            - SHA-256 fingerprint: `EA:41:5B:D4:AC:73:D0:BE:A0:E5:F5:52:24:27:63:59:69:25:8A:1E:72:76:7D:E4:BC:3A:55:42:0B:DE:B6:D5`
            - Distinguished Name: `CN=Metanoia Society, OU=Unknown, O=Metanoia Society, L=Unknown, ST=Unknown, C=US`
        - **Backup strategy:** Keystore password stored in Bitwarden; keystore file backed up to 3 secure locations (Bitwarden attachment + cloud storage + external USB)
        - **Package name decision:** Changed from `com.projectaurora.app` to **`com.metanoiasociety.aurora`** (organization-scoped namespace for future Metanoia Society apps, aligns with owned domains: metanoiasociety.com/net/org)
        - **Aurora.csproj configuration:** Release signing added with environment variable for password (`$(AURORA_KEYSTORE_PASSWORD)`), AAB format enforced
        - **Build command:** `$env:AURORA_KEYSTORE_PASSWORD = "***"; dotnet publish -c Release -f net9.0-android`
        - **Output:** `com.metanoiasociety.aurora-Signed.aab` (30 MB) - 12,929 supported Android devices, API 21+, Target SDK 35
    - **AC 2:** ✅ Upload AAB to Google Play Console (validates signature, locks package name permanently)
    - **AC 3:** ✅ Add release notes: "Initial beta release for internal testing. Features: Daily Picks, Vibe of the Day, reactions, sharing"
    - **AC 4:** ✅ Release "1 (1.0.0)" created and rolled out to Internal Testing track
    - **AC 5:** ✅ Testers configured (developer email added)
    - **AC 6:** ✅ Opt-in URL generated (retrievable from Google Play Console → Internal Testing)
    - **AC 7:** ✅ Self-verification: Installed on S24 Ultra, app launches with correct name "Aurora" and proper icon
    - **AC 8:** ✅ Store listing configured: App name "Aurora - Uplifting Media", short/full descriptions, app icon (512x512px), feature graphic, screenshots uploaded
    - **AC 9:** ✅ Beta Tester Guide updated with opt-in page expectations (placeholder icon/package name normal for internal testing)
    - **Edge Cases:**
        - **Upload rejected by Console:** Common reasons: improper signing (keystore mismatch), version code conflict (already uploaded this version), missing required permissions in AndroidManifest.xml. Console provides specific error; resolve and re-upload.
        - Opt-in URL doesn't activate: Verify email added to Internal Testing list; propagation takes ~5 minutes. If still broken after 15 min, remove and re-add email.
        - App crashes on install: Check Android Studio Logcat for stack trace. Likely causes: signing config mismatch (Debug vs Release), missing dependencies in Release build, ProGuard obfuscation issues (disable for beta).
        - First install delayed: Google's automated security scan (Play Protect) can take 10-30 minutes before install link activates; show "Pending publication" during this time.
    - **Technical Considerations:**
        - **Version Code:** Ensure `<ApplicationVersion>1.0.0</ApplicationVersion>` in `Aurora.csproj` is set correctly; increment to `1.0.1` for second upload (Google rejects duplicate versions)
        - **Permissions:** Verify `AndroidManifest.xml` includes `<uses-permission android:name="android.permission.INTERNET" />` (already present from V-0)
        - **Signing:** MAUI auto-generates debug keystore for local builds; Release requires explicit keystore configuration in project properties or CLI
        - **AAB benefits:** Smaller download size than APK (Google's dynamic delivery), required for apps >150MB, supports Play Feature Delivery
    - **Time Estimate:** 1-2 hours (includes first-time AAB generation learning curve + Console navigation)

---

### Feature V-2.4: Self-Validation & External Tester Execution
*This feature validates Aurora's core value proposition through personal use and small cohort feedback before broader distribution.*

- [~] **Story V-2.4.1:** As a project owner, I want to use Aurora daily for 7 days so that I can validate the value proposition from a user's perspective and identify friction points.
    - **NOTE:** Testing period reduced from 14 days to 7 days (2025-12-28 to 2026-01-04) to accelerate feature development cycle.
    - **AC 1:** Install Aurora on S24 Ultra via Google Play Internal Testing opt-in link (professional install, not sideloaded APK)
    - **AC 2:** Complete baseline survey (Story V-2.1.1) using own responses
    - **AC 3:** Use Aurora for **7 consecutive days** (2025-12-28 to 2026-01-04)
    - **AC 4:** Track usage via **end-of-week recap** (not daily logging) with the following data points:
        - [ ] "How many days this week did I open Aurora?" (count)
        - [ ] "How many times was Aurora the first app I opened?" (count out of total phone pickups - estimate acceptable)
        - [ ] "How many articles did I read in full?" (count)
        - [ ] "Which stories resonated most?" (list titles + 1-sentence why)
        - [ ] "Did I share any stories?" (Yes/No + platform if yes: Discord, Reddit, etc.)
        - [ ] "UX moments that felt good" (specific examples: "Chrome Custom Tabs loaded smoothly," "Uplift animation was satisfying")
        - [ ] "UX moments that felt bad/frustrating" (specific examples: "Button too small on S24," "Snippet text too long")
    - **AC 5:** Submit weekly feedback survey (Story V-2.1.2) at end of Week 1 (2026-01-04) using own responses
    - **AC 6:** Document findings in `docs/beta/BETA_ROUND_1_SELF_VALIDATION.md` using provided template (see below)
    - **AC 7:** **Success Criteria Met:** "I used Aurora ≥4 days out of 7 (≥57% usage rate)" AND "Weekly survey responses indicate positive sentiment (More hopeful, Informed, or similar)"
    - **Edge Cases:**
        - Miss logging for a day: End-of-week recap format allows estimation; doesn't invalidate test if ≥5 days have data
        - Zero usage some days: Valid data point; document reason in recap ("Too busy," "Forgot," "Didn't feel like it" - all useful insights)
        - No stories resonate: **Critical finding**; indicates content curation needs major pivot (document in findings as blocker)
        - Extend testing beyond 14 days: Allowed if decision is unclear; document rationale in findings
    - **Tracking Template (docs/beta/BETA_ROUND_1_SELF_VALIDATION.md):**
        ```markdown
        # Beta Round 1 - Self-Validation Log

        ## Testing Period
        - **Start Date:** [YYYY-MM-DD]
        - **End Date:** [YYYY-MM-DD]
        - **Total Days:** [7-14]

        ## Week 1 Recap: [Date Range]

        ### Usage Summary
        - **Days opened:** [X/7]
        - **First app opened:** [X times]
        - **Articles read:** [X total]
        - **Stories shared:** [Yes/No - Platform if yes]

        ### Resonant Stories
        1. **[Story Title]** - Why: [emotional impact, credibility, shareability]
        2. ...

        ### Good UX Moments
        - [Specific example: "READ button opened Chrome Custom Tab smoothly; back button returned to Aurora correctly"]
        - ...

        ### Bad UX Moments / Friction Points
        - [Specific example: "Uplift button too small on S24 Ultra; required precision tap"]
        - ...

        ### Weekly Survey Response
        [Copy-paste your survey answers here for reference]

        ## Week 2 Recap: [Date Range] (if applicable)
        [Same structure as Week 1]

        ## Overall Impression
        [2-3 paragraphs answering:]
        - Did Aurora make me feel more hopeful about humanity?
        - Would I continue using it after this test?
        - Would I recommend it to a friend? Why or why not?

        ## Success Criteria Met?
        - [ ] Used app ≥4 days out of 7 (≥57% usage rate)
        - [ ] Positive sentiment in weekly survey
        ```
    - **Time Estimate:** 7 days (actual testing) + 1 hour (documentation)

- [x] **Story V-2.4.2:** As a content curator, I want to update Aurora's content at least once during self-validation so that I can measure curation effort and ensure fresh stories for ongoing testing.
    - **AC 1:** Identify 1 Vibe of the Day + 10 Daily Picks from real uplifting news sources (credible outlets, published within last 7-14 days)
    - **AC 2:** Create new `content.json` using `New-ContentTemplate.ps1` as starting point: `.\New-ContentTemplate.ps1 -VibeCount 1 -PicksCount 10 -OutputFile .\beta-round-1-update.json`
    - **AC 3:** Populate template with real stories: titles, 2-3 sentence snippets, article URLs, image URLs (hotlinked or placeholder), published dates
    - **AC 4:** Validate using `Validate-Content.ps1`: `.\Validate-Content.ps1 -FilePath .\beta-round-1-update.json` (must return zero errors, warnings acceptable)
    - **AC 5:** Deploy to Azure Blob Storage: `.\Deploy-Content.ps1 -FilePath .\beta-round-1-update.json -Environment Production`
    - **AC 6:** Verify deployment: Open Aurora on S24 Ultra, pull to refresh (if implemented) or force-stop and reopen app; confirm new stories appear
    - **AC 7:** **Track total curation time:** Start timer at "begin searching for stories," stop at "deployment verified." Document time in format: "X minutes for 11 stories (1 Vibe + 10 Picks)"
    - **AC 8:** **Success Threshold:** Total curation time **≤45 minutes** (per PLANNING.md sustainability criteria). If exceeds 45 min, document why and identify bottlenecks.
    - **AC 9:** Document curation experience in `docs/beta/BETA_ROUND_1_SELF_VALIDATION.md` under new section: "Content Curation Experience"
    - **Edge Cases:**
        - **Curation exceeds 45 min:** Not a failure; document bottlenecks (finding credible sources took 20 min, writing snippets took 15 min, image search took 15 min). Identifies automation opportunities for future.
        - Deployment fails (blob upload error): Rollback using `Rollback-Content.ps1`, investigate error in Azure Portal (connection string, permissions), retry
        - New content doesn't appear in app: Verify blob upload succeeded in Azure Portal; clear app cache or reinstall; check API logs for blob read errors
        - Stories go stale (published >2 weeks ago): Acceptable for beta testing; prioritize credibility and uplift quality over recency
    - **Content Curation Template (add to BETA_ROUND_1_SELF_VALIDATION.md):**
        ```markdown
        ## Content Curation Experience

        ### Update #1: [Date]
        - **Total Time:** [X minutes]
        - **Breakdown (estimate):**
            - Story discovery/research: [X min]
            - Snippet writing: [X min]
            - Image sourcing: [X min]
            - Deployment/verification: [X min]

        ### Bottlenecks Identified
        - [e.g., "Finding credible sources for environmental news took 15 min; need RSS feed automation"]
        - ...

        ### Success Threshold Met?
        - [ ] Total time ≤45 minutes (sustainable for daily curation)
        - [ ] If exceeded, are bottlenecks addressable with tooling? (Yes/No + ideas)
        ```
    - **Time Estimate:** Variable (target ≤45 min, could be 30-60 min for first update)

- [~] **Story V-2.4.3:** As a project owner, I want to synthesize self-validation findings and make Go/No-Go decision for Beta Round 2.
    - **NOTE:** External tester recruitment (AC 3) is OPTIONAL and DEFERRED for Beta Round 1. Self-validation alone is sufficient for Go/No-Go decision (2026-01-04). iOS hardware investment ($1,000+) deferred until Aurora proves value proposition on Android first.
    - **Status:** In Progress (AC 1-2, 4-5 required; AC 3 optional if testers become available)
    - **Recruitment Guide:** See `docs/beta/TESTER_RECRUITMENT_GUIDE.md` for complete onboarding workflow (if needed in future)
    - **AC 1:** Create `docs/beta/BETA_ROUND_1_FINDINGS.md` with the following sections:
        - [ ] **Executive Summary:** 2-3 sentences answering: "Did Aurora deliver on its value proposition for me? Should we proceed with external testers?"
        - [ ] **Quantitative Data:** First-app-opened rate (X%), days used (X/7 or X/14), articles read (total), stories shared (count), curation time (X minutes)
        - [ ] **Qualitative Insights:** Stories that resonated (themes), stories that didn't (themes), emotional impact, habit formation potential
        - [ ] **Critical Bugs:** Issues that block external testing (must-fix before Round 1 expansion) - e.g., app crashes, content fails to load
        - [ ] **UX Friction Points:** Annoyances that degrade experience (should-fix before Round 2) - e.g., button sizing, text readability
        - [ ] **Feature Requests:** Nice-to-haves discovered during testing (could-fix post-beta) - e.g., dark mode improvements, search functionality
    - **AC 2:** Update `BACKLOG.md` with new stories for identified improvements, categorized as:
        - [ ] **Blocker (Beta Round 1 - Fix Before External Testers):** Critical bugs that prevent testing (e.g., app crashes on launch)
        - [ ] **High Priority (Beta Round 2):** Significant UX issues that degrade value prop (e.g., "Uplift button too small on large screens")
        - [ ] **Medium Priority (Post-Beta):** Enhancements for broader release (e.g., "Add pull-to-refresh for content updates")
        - [ ] **Low Priority (Future Backlog):** Nice-to-haves (e.g., "User accounts for cross-device sync")
    - **AC 3:** **[OPTIONAL/DEFERRED] Recruit 1-4 External Testers** (if Android testers become available):
        - [ ] Identify 1-4 trusted contacts with Android devices (friends, family, online community members)
        - [ ] Send recruitment email with: Beta Tester Guide PDF, Baseline Survey link, Google Play opt-in URL, personal note explaining Aurora's purpose
        - [ ] Add tester emails to Google Play Console Internal Testing track
        - [ ] Verify testers successfully install app and complete baseline survey
        - [ ] Set expectation: "Use for 1 week, submit weekly survey on Sunday"
        - **NOTE:** AC 3 is NOT REQUIRED for Beta Round 1 completion. Self-validation (n=1) is sufficient for Go/No-Go decision.
    - **AC 4:** Make **Go/No-Go Decision** documented in findings:
        - [ ] **Go:** Continue refining Aurora; implement blockers/high-priority fixes; proceed to Beta Round 2 with larger cohort
        - [ ] **No-Go:** Substantial pivot required; Aurora doesn't deliver value proposition as designed; decide whether to restart concept or salvage
    - **AC 5:** Document decision rationale in findings: "Why Go?" or "Why No-Go?" (specific data points and insights that drove decision)
    - **Go Decision Criteria (Thresholds):**
        - ✅ Opened Aurora first ≥50% of tested days (shows habit formation potential)
        - ✅ At least 3 stories resonated emotionally over testing period (content quality validated)
        - ✅ Weekly survey shows "More hopeful," "Informed," or similar positive sentiment (value prop validated)
        - ✅ Curation time ≤45 min/update OR bottlenecks identified with clear automation path (sustainability validated)
        - ✅ No critical bugs encountered (app stability validated)
    - **No-Go Decision Triggers (Pivot Signals):**
        - ❌ Never or rarely opened Aurora first (<30% of days) - no habit formation
        - ❌ No stories resonated; felt neutral or negative about content - content quality failure
        - ❌ Curation time consistently >60 min with no clear automation path - unsustainable
        - ❌ Critical bugs made app frequently unusable - stability failure
    - **Edge Cases:**
        - **Mixed results:** Some criteria pass, some fail. Document as "Conditional Go" with specific blockers that must be addressed before Round 2. Example: "Content resonated, but curation time was 55 min; automate RSS parsing before Round 2."
        - **No strong signal:** Neutral data (neither great nor terrible). Recommendation: Recruit 1-2 external testers for additional perspectives; extend testing to Week 2 if only tested 7 days.
        - **Cannot recruit external testers:** Acceptable to proceed with self-only if self-validation was strongly positive; document as limitation ("Round 1 limited to self-testing due to tester availability; will expand in Round 2")
        - **All testers decline or ghost:** Valid finding; may indicate low perceived value. Document in No-Go rationale: "Could not recruit testers; suggests concept lacks initial appeal."
    - **External Tester Recruitment Email Template:**
        ```
        Subject: Help me test Aurora - a refuge from negative news

        Hi [Name],

        I'm working on a mobile app called Aurora that curates uplifting news stories - a refuge from the fear-mongering and negativity that dominates most media. I'd love your help testing it for 1-2 weeks.

        **What I'm asking:**
        - Install Aurora on your Android phone (via Google Play link)
        - Use it for 1 week (~5 min/day)
        - Fill out a quick weekly survey (5 min)

        **What you get:**
        - Early access to something genuinely positive
        - A say in shaping the app before broader release
        - Daily dose of hope for humanity 😊

        Interested? Here's what to do:
        1. Fill out this baseline survey: [LINK]
        2. Install Aurora: [GOOGLE PLAY OPT-IN LINK]
        3. Open the app and explore!

        Full guide attached. Let me know if you have any questions!

        Thanks,
        [Your Name]
        ```
    - **Time Estimate:** 3-4 hours (write findings, update backlog, recruit testers)

---

## Milestone CT-1: Content Management Tooling Refactor

**Objective:** Migrate PowerShell-based content management scripts to testable .NET console applications with unified CLI and local global tool distribution.

**Success Criteria:**
- All 4 PowerShell scripts migrated to .NET with functional parity
- 20+ unit tests covering validation, deployment, and template generation logic
- Zero-warning build maintained
- Local global tool installation documented and verified
- PowerShell scripts remain in repo until user confirms deletion

**Estimated Effort:** 2-3 weeks

**Priority:** HIGH (7/10 pain, aligns with main tech stack, improves confidence in content curation workflow)

---

### Feature CT-01: Project Infrastructure & Unified CLI Setup
*Establish the foundational .NET console application with CommandLineParser, global tool packaging, and shared infrastructure.*

- [ ] **Story CT-1.1.1:** As a developer, I want a .NET 9 console application project for content management tools so that I have a foundation for migrating PowerShell scripts.
    - **AC 1:** Create `src/Aurora.ContentTools/Aurora.ContentTools.csproj` (.NET 9 console application)
    - **AC 2:** Add `CommandLineParser` NuGet package for CLI argument parsing
    - **AC 3:** Create `Program.cs` with basic command routing structure (validate, template, deploy, rollback subcommands)
    - **AC 4:** Add project reference to `Aurora.Shared` for schema access
    - **AC 5:** Configure project as .NET global tool in .csproj:
        ```xml
        <PackAsTool>true</PackAsTool>
        <ToolCommandName>aurora-tools</ToolCommandName>
        <PackageId>Aurora.ContentTools</PackageId>
        ```
    - **AC 6:** Add to `Project-Aurora.sln` under `tools/` solution folder
    - **AC 7:** Verify build succeeds with zero warnings
    - **Edge Cases:**
        - Tool name collision: `aurora-tools` may conflict if multiple versions installed; document uninstall/reinstall process
        - .NET 9 SDK requirement: Document prerequisite in tool README
    - **Time Estimate:** 2 hours

- [ ] **Story CT-1.1.2:** As a developer, I want shared configuration and authentication infrastructure so that all commands can access Azure resources and user settings consistently.
    - **AC 1:** Create `Configuration/ToolSettings.cs` to load from:
        - Command-line arguments (highest priority)
        - `aurora-tools.config.json` (medium priority)
        - Environment variables (lowest priority)
    - **AC 2:** Create `Configuration/AzureAuthProvider.cs` to handle Azure SDK authentication:
        - Use `DefaultAzureCredential` for local development (Az CLI, Visual Studio, etc.)
        - Support connection string override via config/env var for CI/CD
    - **AC 3:** Create `aurora-tools.config.json` template with documented structure:
        ```json
        {
          "DefaultEnvironment": "Dev",
          "Environments": {
            "Dev": {
              "StorageAccountName": "staurora4tcguzr2zm32w",
              "ContainerName": "aurora-content",
              "BlobName": "content.json"
            },
            "Production": {
              "StorageAccountName": "staurora4tcguzr2zm32w",
              "ContainerName": "aurora-content",
              "BlobName": "content.json"
            }
          }
        }
        ```
    - **AC 4:** Document configuration precedence and authentication methods in tool README
    - **AC 5:** Add `.gitignore` entry for local config overrides (`aurora-tools.local.config.json`)
    - **Edge Cases:**
        - Missing Az CLI authentication: Tool should provide clear error message with setup instructions
        - Invalid config JSON: Tool should validate and show line number of syntax error
        - Multiple authentication methods available: Document precedence order clearly
    - **Time Estimate:** 3 hours

- [ ] **Story CT-1.1.3:** As a developer, I want comprehensive documentation for local global tool installation so that I can confidently use the .NET tools instead of PowerShell scripts.
    - **AC 1:** Create `tools/content-management/README-DOTNET.md` with sections:
        - **Prerequisites:** .NET 9 SDK, Azure CLI (for authentication)
        - **Installation:** Local global tool install command with step-by-step walkthrough
        - **Configuration:** aurora-tools.config.json setup and environment variable options
        - **Usage Examples:** Complete examples for each subcommand (validate, template, deploy, rollback)
        - **Troubleshooting:** Common errors and solutions
        - **Migration from PowerShell:** Side-by-side command comparison table
    - **AC 2:** Installation instructions include:
        ```bash
        # From repository root
        dotnet pack src/Aurora.ContentTools/Aurora.ContentTools.csproj -o ./nupkg
        dotnet tool install --global Aurora.ContentTools --add-source ./nupkg

        # Verify installation
        aurora-tools --version
        ```
    - **AC 3:** Uninstall/upgrade instructions documented:
        ```bash
        # Uninstall
        dotnet tool uninstall --global Aurora.ContentTools

        # Upgrade (after rebuilding)
        dotnet tool update --global Aurora.ContentTools --add-source ./nupkg
        ```
    - **AC 4:** Include troubleshooting section for:
        - "Tool 'aurora-tools' is already installed" → uninstall first
        - "No executable found matching command 'aurora-tools'" → check PATH, restart terminal
        - Azure authentication failures → `az login` required
    - **AC 5:** Add migration notes: "PowerShell scripts will remain until .NET tools are verified; safe to use side-by-side"
    - **Time Estimate:** 2 hours

---

### Feature CT-02: Validate Command Migration
*Migrate Validate-Content.ps1 to .NET with JSON schema validation, URL validation, and quality warnings.*

- [ ] **Story CT-1.2.1:** As a content curator, I want a .NET-based validate command that performs JSON schema validation so that I can verify content files before deployment.
    - **AC 1:** Create `Commands/ValidateCommand.cs` implementing CommandLineParser verb
    - **AC 2:** Command accepts arguments:
        - `--file <path>` (required): Path to content.json file
        - `--schema <path>` (optional): Path to content.schema.json (defaults to repo root)
        - `--strict` (optional flag): Treat warnings as errors
    - **AC 3:** Load `content.schema.json` using `Newtonsoft.Json.Schema` or `NJsonSchema`
    - **AC 4:** Validate JSON file against schema:
        - Syntax validation (valid JSON)
        - Required fields present (id, title, snippet, image_url, article_url, uplift_count)
        - Field types correct (uplift_count is non-negative integer)
        - URLs are well-formed (http/https protocol)
    - **AC 5:** Output format matches PowerShell script style:
        - Success: `✓ content.json is valid and ready to upload`
        - Errors: `✗ Error on line X: [detailed message]`
        - Return exit code 0 (success) or 1 (failure) for CI/CD integration
    - **AC 6:** Errors include helpful context:
        - JSON path to invalid field (e.g., `DailyPicks[2].title`)
        - Expected vs actual value
        - Schema constraint violated
    - **Edge Cases:**
        - File not found: Clear error with absolute path shown
        - Schema not found: Default to `content.schema.json` in repo root; error if missing
        - Malformed JSON: Show line/column number from parser
        - Empty file: Treat as validation failure
    - **Time Estimate:** 4 hours

- [ ] **Story CT-1.2.2:** As a content curator, I want the validate command to check image URL accessibility so that I can catch broken images before deployment.
    - **AC 1:** Add `--check-images` flag (optional, defaults to true)
    - **AC 2:** For each `image_url` in content file, perform HEAD request with 5-second timeout
    - **AC 3:** Warnings (not errors) for:
        - HTTP 404 (image not found)
        - Timeout (slow/unresponsive server)
        - HTTP 403 (hotlink protection)
        - Invalid URL (malformed after schema validation)
    - **AC 4:** Warning format: `⚠ Warning: Image URL for '[Article Title]' returned [status code/error]`
    - **AC 5:** Allow `--strict` flag to treat image warnings as errors (fails validation)
    - **AC 6:** Skip image checks if `--check-images false` specified (for offline validation)
    - **Edge Cases:**
        - Offline network: Skip checks with warning "Network unavailable, skipping image validation"
        - HTTPS certificate errors: Warn but don't fail (some valid sites have cert issues)
        - Redirect chains: Follow up to 3 redirects, warn if >3
    - **Time Estimate:** 3 hours

- [ ] **Story CT-1.2.3:** As a content curator, I want quality warnings for snippet length and Daily Picks count so that I maintain content standards.
    - **AC 1:** Quality checks (warnings only, never errors):
        - Snippet length >200 characters: `⚠ Warning: Snippet for '[Title]' is long (X chars). Consider shortening.`
        - Snippet length <50 characters: `⚠ Warning: Snippet for '[Title]' is short (X chars). Consider expanding.`
        - Daily Picks count <5: `⚠ Warning: DailyPicks contains only X items. Recommend 5-10 stories for variety.`
        - Daily Picks count >10: `⚠ Warning: DailyPicks contains X items. Recommend 5-10 stories to avoid overwhelming users.`
    - **AC 2:** Quality warnings controlled by `--quality-checks` flag (defaults to true)
    - **AC 3:** Summary output shows count of errors vs warnings:
        ```
        Validation complete: 0 errors, 3 warnings
        ⚠ 2 snippets could be improved
        ⚠ 1 Daily Picks count outside recommended range
        ```
    - **AC 4:** With `--strict` flag, quality warnings do NOT become errors (schema/URL errors only)
    - **Time Estimate:** 2 hours

---

### Feature CT-03: Template Command Migration
*Migrate New-ContentTemplate.ps1 to .NET with schema-compliant template generation and helpful placeholders.*

- [ ] **Story CT-1.3.1:** As a content curator, I want a .NET-based template command that generates schema-compliant content.json scaffolding so that I can quickly create new content files.
    - **AC 1:** Create `Commands/TemplateCommand.cs` implementing CommandLineParser verb
    - **AC 2:** Command accepts arguments:
        - `--vibe-count <int>` (optional, default: 1): Number of Vibe items to generate (note: schema supports only 1, warn if >1)
        - `--picks-count <int>` (optional, default: 7): Number of Daily Picks to generate
        - `--output <path>` (optional, default: `./content-template.json`): Output file path
    - **AC 3:** Generated template includes all required schema fields with helpful placeholders:
        ```json
        {
          "id": "REPLACE_WITH_UNIQUE_ID_1",
          "title": "TODO: Article Title Here",
          "snippet": "TODO: Write a 2-3 sentence summary that explains the uplifting news in a clear, engaging way. Focus on the positive impact and specific outcomes.",
          "image_url": "https://via.placeholder.com/800x600/7986CB/FFFFFF?text=Aurora+Placeholder",
          "article_url": "https://example.com/article-1",
          "uplift_count": 0
        }
        ```
    - **AC 4:** IDs auto-numbered sequentially: `REPLACE_WITH_UNIQUE_ID_1`, `REPLACE_WITH_UNIQUE_ID_2`, etc.
    - **AC 5:** Output structure matches schema:
        ```json
        {
          "VibeOfTheDay": { /* single vibe item */ },
          "DailyPicks": [ /* array of pick items */ ]
        }
        ```
    - **AC 6:** Success output: `✓ Template generated: content-template.json (1 Vibe + 7 Picks)`
    - **AC 7:** Generated file must pass validation: `aurora-tools validate --file content-template.json` succeeds (warnings acceptable)
    - **AC 8:** Match PowerShell color-coded output style with box-drawing and emoji
    - **Edge Cases:**
        - Output file exists: Prompt "File exists. Overwrite? (y/n)" or add `--force` flag
        - Invalid counts (0 or negative): Error "Counts must be positive integers"
        - Output directory doesn't exist: Create parent directories automatically
        - Vibe count >1: Warn "Multiple Vibes requested, but only 1 is supported in schema. Using first entry only."
    - **Time Estimate:** 3 hours

- [ ] **Story CT-1.3.2:** As a content curator, I want the template command to include next steps guidance and curation resources so that I understand how to populate the template.
    - **AC 1:** Console output includes next steps guidance (matching PowerShell script):
        ```
        ✓ Template generated: content-template.json (1 Vibe + 7 Picks)

        Next Steps:
        1. Edit the template file:
           code content-template.json

        2. Replace all placeholder values:
           - Update 'id' with unique kebab-case identifiers
           - Replace 'TODO: Article Title Here' with actual titles
           - Write engaging 2-3 sentence snippets
           - Set real article URLs from credible sources
           - Update image URLs (or keep placeholder)

        3. Validate the content:
           aurora-tools validate --file content-template.json --check-images

        4. Deploy to Dev environment:
           aurora-tools deploy --file content-template.json --environment Dev

        5. After testing, deploy to Production:
           aurora-tools deploy --file content-template.json --environment Production
        ```
    - **AC 2:** Include content curation resources section:
        ```
        Content Curation Resources:
          • r/UpliftingNews - https://reddit.com/r/UpliftingNews
          • Good News Network - https://goodnewsnetwork.org
          • Positive News - https://positive.news
          • The Optimist Daily - https://optimistdaily.com
        ```
    - **AC 3:** Use color-coded output matching PowerShell style (green boxes, cyan info, gray secondary text)
    - **Time Estimate:** 1 hour

---

### Feature CT-04: Deploy Command Migration
*Migrate Deploy-Content.ps1 to .NET with Azure SDK, pre-deployment validation, backup creation, and environment support.*

- [ ] **Story CT-1.4.1:** As a content curator, I want a .NET-based deploy command that uploads content to Azure Blob Storage so that I can publish content updates without redeploying the API.
    - **AC 1:** Create `Commands/DeployCommand.cs` implementing CommandLineParser verb
    - **AC 2:** Command accepts arguments:
        - `--file <path>` (required): Path to content.json file to deploy
        - `--environment <name>` (optional, default: "Dev"): Environment name (Dev/Production)
        - `--skip-validation` (optional flag): Skip pre-deployment validation
        - `--skip-backup` (optional flag): Skip backup creation
        - `--force` (optional flag): Skip confirmation prompt (for automation)
    - **AC 3:** Pre-deployment validation (unless `--skip-validation`):
        - Run validate command internally
        - Abort deployment if validation fails
        - Display validation summary before proceeding
    - **AC 4:** Confirmation prompt (unless `--force`):
        ```
        Ready to deploy to Production:
        - File: content.json (1 Vibe + 10 Picks)
        - Destination: staurora4tcguzr2zm32w/aurora-content/content.json

        Continue? (y/n):
        ```
    - **AC 5:** Use Azure.Storage.Blobs SDK (already in Aurora.Api dependency):
        - Create `BlobServiceClient` using `DefaultAzureCredential`
        - Get container reference (`aurora-content`)
        - Upload blob with overwrite
    - **AC 6:** Success output:
        ```
        ✓ Content deployed to Production at 2026-01-01 17:40:24
        ```
    - **AC 7:** Return exit code 0 (success) or 1 (failure) for CI/CD integration
    - **AC 8:** Match PowerShell color-coded output style with progress indicators (Step 1/4, Step 2/4, etc.)
    - **Edge Cases:**
        - Azure authentication failure: Clear error "Authentication failed. Run 'az login' and try again."
        - Storage account not found: Error with account name shown
        - Network timeout: Retry 3 times with exponential backoff (use Polly policy from Aurora.Api pattern)
        - Blob container doesn't exist: Error "Container 'aurora-content' not found in storage account"
    - **Time Estimate:** 4 hours

- [ ] **Story CT-1.4.2:** As a content curator, I want the deploy command to create automatic backups before uploading so that I can rollback if needed.
    - **AC 1:** Before upload (unless `--skip-backup`):
        - Download current `content.json` from blob storage
        - Save to `tools/content-management/backups/content.backup.<Environment>.YYYY-MM-DD-HHMMSS.json`
        - Example: `backups/content.backup.Production.2026-01-01-174024.json`
    - **AC 2:** Backup creation output:
        ```
        Creating backup of current content...
        ✓ Backup saved: content.backup.Production.2026-01-01-174024.json
        ```
    - **AC 3:** If no existing blob found (first deployment):
        ```
        ℹ No existing content found; skipping backup
        ```
    - **AC 4:** Automatic cleanup: Keep only last 5 backups per environment
        - Sort backups by timestamp (filename)
        - Delete oldest backups if count >5
        - Output: `Cleaned up 2 old backups (keeping last 5)`
    - **AC 5:** Backup directory created automatically if doesn't exist
    - **Edge Cases:**
        - Disk full during backup: Error before upload, don't corrupt production
        - Backup download fails: Warn but allow deployment with explicit confirmation
        - Manual backups in folder: Only auto-cleanup files matching pattern `content.backup.<Env>.*.json`
    - **Time Estimate:** 3 hours

---

### Feature CT-05: Rollback Command Migration
*Migrate Rollback-Content.ps1 to .NET with interactive backup selection and re-deployment.*

- [ ] **Story CT-1.5.1:** As a content curator, I want a .NET-based rollback command that restores previous content versions so that I can recover from deployment mistakes.
    - **AC 1:** Create `Commands/RollbackCommand.cs` implementing CommandLineParser verb
    - **AC 2:** Command accepts arguments:
        - `--environment <name>` (optional, default: "Dev"): Environment to rollback
        - `--backup <path>` (optional): Specific backup file to restore (skips interactive selection)
        - `--force` (optional flag): Skip confirmation prompt
    - **AC 3:** Interactive backup selection (if `--backup` not provided):
        - List backups from `tools/content-management/backups/content.backup.<Environment>.*.json`
        - Sort by timestamp descending (newest first)
        - Display selection menu:
            ```
            Available backups for Production:
            [1] content.backup.Production.2026-01-01-174024.json
                Created: 2026-01-01 17:40:24 | Size: 12.34 KB
            [2] content.backup.Production.2025-12-31-121530.json
                Created: 2025-12-31 12:15:30 | Size: 11.89 KB
            [3] content.backup.Production.2025-12-30-092211.json
                Created: 2025-12-30 09:22:11 | Size: 12.01 KB

            [0] Cancel rollback

            Select backup to restore [0-3]:
            ```
    - **AC 4:** Confirmation prompt (unless `--force`):
        ```
        ⚠ Warning: This will replace current Production content with backup from 2026-01-01 17:40:24.
        Current content will be backed up automatically before rollback.

        Continue? (y/n):
        ```
    - **AC 5:** Rollback process:
        - Create backup of current content (same as deploy command)
        - Validate selected backup file before uploading
        - Upload selected backup file to blob storage
        - Verify upload succeeded
    - **AC 6:** Success output:
        ```
        ✓ Backup created: content.backup.Production.2026-01-01-180530.json
        ✓ Rolled back to backup from 2026-01-01 17:40:24
        ```
    - **AC 7:** Match PowerShell color-coded output style (yellow warning boxes, green success boxes)
    - **Edge Cases:**
        - No backups found: Error "No backups found for Production environment"
        - Invalid backup file: Validate JSON before uploading (run validation internally)
        - Backup file corrupted: Error "Backup file is not valid JSON"
        - User cancels selection: Exit gracefully with code 0
    - **Time Estimate:** 3 hours

---

### Feature CT-06: Unit Testing & Documentation
*Comprehensive unit tests for all commands and testing documentation.*

- [ ] **Story CT-1.6.1:** As a developer, I want unit tests for the validate command so that I can confidently refactor validation logic.
    - **AC 1:** Create `src/Aurora.ContentTools.Tests/Commands/ValidateCommandTests.cs`
    - **AC 2:** Test scenarios (minimum 8 tests):
        - Valid content file passes validation
        - Missing required field fails with specific error
        - Invalid URL format fails validation
        - Negative uplift_count fails validation
        - Long snippet (>200 chars) shows warning
        - Short snippet (<50 chars) shows warning
        - Daily Picks count <5 shows warning
        - Malformed JSON shows line number error
    - **AC 3:** Use test fixtures in `src/Aurora.ContentTools.Tests/Fixtures/`:
        - `valid-content.json` (passes all checks)
        - `invalid-schema.json` (schema violations)
        - `quality-warnings.json` (triggers quality warnings)
    - **AC 4:** Mock `HttpClient` for image URL checks (use pattern from Aurora.Client.Core.Tests)
    - **AC 5:** All tests pass; zero warnings
    - **Time Estimate:** 4 hours

- [ ] **Story CT-1.6.2:** As a developer, I want unit tests for the template, deploy, and rollback commands so that I can verify core functionality.
    - **AC 1:** Create test files:
        - `TemplateCommandTests.cs` (5+ tests)
        - `DeployCommandTests.cs` (6+ tests)
        - `RollbackCommandTests.cs` (5+ tests)
    - **AC 2:** Template tests:
        - Default parameters generate 1 Vibe + 7 Picks
        - Custom counts work correctly
        - Generated template passes validation
        - Output file created at specified path
        - Auto-numbered IDs are sequential
    - **AC 3:** Deploy tests:
        - Validation runs before deployment
        - Backup created before upload
        - Azure SDK BlobServiceClient called correctly (mocked)
        - Deployment fails if validation fails
        - Skip flags work correctly
        - Old backups cleaned up (>5)
    - **AC 4:** Rollback tests:
        - Backup list filtered by environment
        - Selected backup uploaded correctly
        - Current content backed up before rollback
        - Invalid backup file rejected
        - No backups found shows error
    - **AC 5:** Mock Azure SDK clients using Moq (pattern: create mock `BlobContainerClient`)
    - **AC 6:** All 16+ tests pass; zero warnings
    - **Time Estimate:** 6 hours

- [ ] **Story CT-1.6.3:** As a developer, I want testing documentation that explains how to write and run content tools tests so that future contributors can maintain quality.
    - **AC 1:** Create `docs/testing/CONTENT_TOOLS_TESTING.md` with sections:
        - **Overview:** Purpose of Aurora.ContentTools.Tests project
        - **Running Tests:** `dotnet test` command with filtering examples
        - **Test Structure:** Fixtures, mocking patterns, naming conventions
        - **Adding New Tests:** Step-by-step guide for new command tests
        - **Mocking Azure SDK:** Example of BlobServiceClient mocking pattern
        - **CI Integration:** How tests run in GitHub Actions
    - **AC 2:** Include example test with detailed annotations:
        ```csharp
        [Fact]
        public async Task ValidateCommand_ValidContent_ReturnsSuccess()
        {
            // Arrange: Set up test fixture and expected behavior
            var contentPath = "Fixtures/valid-content.json";
            var command = new ValidateCommand { FilePath = contentPath };

            // Act: Execute the command
            var result = await command.ExecuteAsync();

            // Assert: Verify expected outcome
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("✓", result.Output);
        }
        ```
    - **AC 3:** Document test data management:
        - Where to add fixtures (`Aurora.ContentTools.Tests/Fixtures/`)
        - How to mark files as "Copy to Output Directory"
        - When to use embedded resources vs file fixtures
    - **AC 4:** Link to existing Aurora.Client.Core.Tests and Aurora.Api.Tests for cross-reference
    - **Time Estimate:** 2 hours

---

---

**Planned Milestones:** See [backlog/planned/](backlog/planned/) for future milestones not yet scheduled.

**Milestones Last Updated:** 2026-01-01
