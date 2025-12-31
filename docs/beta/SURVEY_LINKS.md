# Aurora Beta Testing - Survey Links

This document tracks the Google Forms used for structured feedback collection during beta testing.

---

## Baseline Survey
**Purpose:** Pre-use context and expectations. Captures tester contact info, app usage patterns, social media habits, sentiment analysis, positivity definitions, and value proposition validation.

**When to Complete:** Before installing Aurora (one-time, pre-test)

**Link:** https://forms.gle/eRq3qY1EveJbP6Q87

**Responses Sheet:** https://docs.google.com/spreadsheets/d/1Xl90ykDVii1rTgsgFDcUnoQAjxG-JjT0nxnbYyjb1ok/edit?gid=1658055882#gid=1658055882

**Question Count:** 14 questions (11 optional, 3 required)

**Required Questions:**
1. Preferred contact method (Email, Discord, Reddit, Other)
2. Contact details (email/username)
11. Value proposition rating (1-5 scale)

**Optional Questions:**
3. Social media usage hours
4. Most-used apps
5. Positive/negative impact assessment
6. Impact details (content, styling, features)
7. Typical emotions during usage
8. Avoidance frequency
9. Positive story types
10. Topics to avoid
12. First app opened
13. Mobile-optimized reading importance
14. Additional feedback/comments

**Confirmation Message:** "Thank you for sharing your perspective! This baseline survey is complete—you won't need to fill it out again. Next, you'll receive the Aurora installation link via the method you specified. We're excited to have you join us in building a more hopeful news experience."

**Status:** ✅ Live and verified (2025-12-29)

---

## Weekly Feedback Survey
**Purpose:** Usage tracking, sentiment analysis, friction points, and continuation intent.

**When to Complete:** End of each week during beta testing period

**Link:** https://forms.gle/SqN9ZH54zea1GkTJ8

**Responses Sheet:** https://docs.google.com/spreadsheets/d/1RuqIF1tZcrW-FH_0PGXqACNon_Vjx-SNkxwpLmjIcQo/edit?gid=1863402994#gid=1863402994

**Question Count:** 12 questions (11 optional, 1 required)

**Required Questions:**
1. How many days this week did you open Aurora?

**Optional Questions:**
2. Was Project Aurora ever the first app you opened when picking up your phone?
3. How many articles from Project Aurora did you read this week?
4. Which story resonated with you most this week?
5. Did you share any stories from Aurora? If not, why?
6. How did using Aurora make you feel this week? (Select all that apply)
7. Want to elaborate? (Optional - for Q6)
8. What felt good when using Project Aurora this week?
9. What felt bad or frustrating when using Project Aurora this week?
10. Did you encounter any bugs or crashes?
11. Assuming development continued in a similar direction for a full release with your feedback in mind, would you continue using Project Aurora?
12. Want to elaborate? (Optional - for Q11)

**Confirmation Message:** "Thank you for your feedback! Your insights help make Aurora better each week."

**Status:** ✅ Live and verified (2025-12-29)

---

## Beta Tester Guide
**Purpose:** Onboarding documentation for beta testers explaining Aurora's purpose, installation steps, feedback mechanisms, testing focus, and known limitations.

**Location:**
- Markdown: `docs/beta/BETA_TESTER_GUIDE.md`
- PDF: `docs/beta/BETA_TESTER_GUIDE.pdf`
- HTML (source): `docs/beta/BETA_TESTER_GUIDE.html`

**Format:** Markdown + HTML + PDF for email distribution

**Sections:**
1. Welcome & Purpose (with beta definition)
2. What is Aurora? (value proposition and feature overview)
3. Known Limitations (5 subsections: platform, content updates, article accessibility, Uplift behavior, minimal features)
4. ✅ You'll Know Aurora is Working If... (expected behavior guide)
5. Installation (4 detailed steps with email expectations and app confirmation)
6. How to Provide Feedback (weekly survey, in-app button, bug/question template)
7. What We're Testing (content quality, UX, value proposition)
8. Time Commitment (with usage reassurance)

**Tone:** Warm, conversational, approachable with emojis throughout (matches "Morning Mist" design philosophy)

**Word Count:** ~1,400 words

**Key Features:**
- 🎨 **Emoji-enhanced:** Friendly icons throughout for visual engagement
- 🖋️ **Custom typography:** Nunito font via CSS for warm, rounded aesthetic
- 🎨 **Morning Mist palette:** Purple-blue headings (#7986CB) matching app design system
- 📧 **Email distribution clarity:** Google Play link sent manually via email/Discord after baseline survey
- 📱 **Device info instructions:** Step-by-step guidance for non-tech-savvy users (Settings → About phone)
- 🐛 **Beginner-friendly bug template:** Copy-paste template with filled example
- 📱 **App confirmation:** Logo image (80x80px) + visual description for installation verification
- ⚠️ **Known limitations disclosure:** Stale Uplift counts, no state tracking, manual curation, paywalls
- ⏰ **Response time expectations:** 24-48 hours for email replies
- 💬 **Non-urgent question support:** Email welcome for general questions, not just bugs
- ✅ **Expected behavior guide:** Reduces false bug reports (2-3 sec spin-up, count jumps normal)
- 🕐 **Flexible timing:** Weekly survey "whenever works for you" (no specific day pressure)
- 💯 **Usage reassurance:** "Used it twice or ten times? Either way, fill out the survey"

**Status:** ✅ Complete (2025-12-30) - PDF finalized with custom styling, ready for email distribution

---

## Notes
- All forms configured to NOT require Google account sign-in
- Responses automatically saved to Google Sheets with timestamps
- Forms are editable post-submission: **Disabled** (prevents accidental data loss)
- Limit to 1 response: **Unchecked** (allows multiple testers without friction)
