# Aurora Beta - External Tester Recruitment & Onboarding Guide

## Overview

This guide outlines the process for recruiting and onboarding 1-4 trusted external testers for Aurora's Beta Round 1. External testers will use Aurora in parallel with the project owner's self-validation testing to provide diverse perspectives on the value proposition.

---

## Prerequisites

Before recruiting testers, ensure the following resources are ready:

- ✅ **Beta Tester Guide PDF:** `docs/beta/BETA_TESTER_GUIDE.pdf`
- ✅ **Baseline Survey:** Google Form created and published
- ✅ **Weekly Feedback Survey:** Google Form created and published
- ✅ **Google Play Internal Testing:** App published with opt-in capability
- ✅ **Fresh Content:** Recent stories deployed to production

---

## Step 1: Identify Potential Testers

**Target:** 1-4 trusted contacts with Android devices

### Selection Criteria:
- Has Android phone or tablet (iOS not supported in beta)
- Willing to commit 5-10 minutes daily for 1 week
- Provides honest, constructive feedback
- Aligns with Aurora's mission (interested in uplifting/positive content)
- Preferably diverse perspectives: different ages, backgrounds, news consumption habits

### Candidate Pool Ideas:
- Friends/family who've expressed interest in the project
- Online community members (Discord, Reddit, local tech groups)
- Colleagues interested in app testing
- Former classmates in tech/design fields

---

## Step 2: Send Initial Recruitment Email

### Email Template

```
Subject: Help me test Aurora - uplifting news app

Hi [Name],

I'm working on a mobile app called Aurora that curates uplifting news stories to provide a refuge from the negativity and fear-mongering that dominates most media. The goal is to restore faith in humanity through credible, positive journalism.

**What I'm asking:**
- Fill out a short baseline survey (3-5 minutes)
- Install Aurora on your Android phone via Google Play Internal Testing
- Use it for 1 week, naturally integrating it into your daily routine
- Submit a brief weekly feedback survey at the end

**What's included:**
I've attached a Beta Tester Guide (PDF) that explains everything: how to install, what to expect, and how to share feedback. The baseline survey link is also included in the guide.

If you're interested in helping shape something genuinely positive before it launches publicly, I'd be grateful for your time and honest feedback.

**First step:** Please review the attached Beta Tester Guide and complete the baseline survey. Once you've submitted it, I'll send you the Google Play installation link.

Thanks for considering this!

[Your Name]
```

### Attachments:
- `docs/beta/BETA_TESTER_GUIDE.pdf`

### Personalization Tips:
- Add a personal note explaining why you chose them specifically
- Reference shared interests or past conversations about news/media
- Adjust tone based on relationship (formal for colleagues, casual for friends)
- Mention specific Aurora features that might appeal to them

---

## Step 3: Monitor Baseline Survey Submissions

### After Sending Recruitment Email:

1. Check baseline survey responses spreadsheet periodically
2. Note which candidates have completed the survey
3. Once a tester completes the baseline survey, proceed to Step 4

### If No Response After 3 Days:
- Send friendly follow-up: "Hi [Name], just checking if you had any questions about the Aurora beta test? No pressure if timing doesn't work!"
- Document non-responders as "Declined/No Response"

---

## Step 4: Send Google Play Opt-In Link

### After Tester Completes Baseline Survey:

**A. Add Tester to Google Play Console**

1. Navigate to Google Play Console
2. Select Aurora app → Testing → Internal testing
3. Click Testers tab
4. Under "Email addresses" section, add tester's email (must match their Google account)
5. Save changes

**B. Retrieve Opt-In URL**

1. In Google Play Console → Testing → Internal testing → Testers tab
2. Scroll to "How testers join your test" section
3. Copy the opt-in link

**C. Send Follow-Up Email**

```
Subject: Aurora Beta - Installation Link

Hi [Name],

Thanks for completing the baseline survey! Here's your Google Play opt-in link to install Aurora:

[INSERT OPT-IN LINK]

**Next steps:**
1. Click the link above (must be signed into your Google account)
2. Accept the Internal Testing invitation
3. Install Aurora from Google Play
4. Open the app and start exploring!

The Beta Tester Guide (attached previously) has full details, but the app is self-explanatory. Use it naturally over the next week - there's no "correct" way to use Aurora.

I'll check in around Day 7 to share the weekly feedback survey link. Feel free to reach out with any questions or use the in-app "Share Feedback" button anytime.

Thanks again!

[Your Name]
```

---

## Step 5: Track Tester Progress

### Recommended Tracking Method

Create a simple spreadsheet or document:

| Tester Name | Recruitment Sent | Baseline Survey | Opt-In Link Sent | App Installed | Week 1 Survey | Notes |
|-------------|------------------|-----------------|------------------|---------------|---------------|-------|
| Alice Smith | 2025-12-31       | ✅ 2025-12-31   | ✅ 2025-12-31    | ✅ Confirmed  | ⏳ Due 2026-01-07 | Excited |
| Bob Jones   | 2025-12-31       | ⏳ Pending      | ❌ Not sent      | ❌ No         | ⏳ Pending    | Follow-up needed |

### Status Definitions:
- **Recruitment Sent:** Initial email with PDF sent
- **Baseline Survey:** Tester completed survey (check responses spreadsheet)
- **Opt-In Link Sent:** Google Play link sent after baseline completion
- **App Installed:** Confirmed via tester communication or Google Play Console analytics
- **Week 1 Survey:** Submitted weekly feedback survey (check responses spreadsheet)

---

## Step 6: Support Testers During Testing Period

### Expected Tester Timeline:

- **Day 0:** Receive recruitment email, review PDF, complete baseline survey
- **Day 0-1:** Receive opt-in link, install Aurora
- **Days 1-7:** Use Aurora naturally (5-10 min/day)
- **Day 7 (Sunday):** Submit weekly feedback survey
- **Optional:** Use in-app "Share Feedback" button for ad-hoc comments

### Common Tester Questions & Answers:

**Q: "I don't see Aurora in the Play Store search."**
- A: You must use the opt-in URL I sent. The app is in Internal Testing and not publicly searchable.

**Q: "The opt-in link says I don't have access."**
- A: Ensure you're signed into the Google account matching the email I added. Email me if still blocked.

**Q: "How often should I open Aurora?"**
- A: Ideally daily, but use it naturally. Even 4-5 days out of 7 provides valuable data.

**Q: "Should I read every article?"**
- A: No! Only read what genuinely interests you. We're testing whether Aurora sparks curiosity.

**Q: "Can I share feedback outside the weekly survey?"**
- A: Yes! Use the "Share Feedback" button in the app, or email me directly.

---

## Step 7: Send Weekly Survey Reminder (Day 7)

### Email Template:

```
Subject: Aurora Beta - Week 1 Feedback Survey

Hi [Name],

Thanks for testing Aurora this week! I'd love to hear your thoughts.

**Please complete this 5-minute weekly feedback survey:**
[INSERT WEEKLY SURVEY LINK]

Your honest feedback - positive or negative - is incredibly valuable for shaping Aurora before broader release.

If you didn't get a chance to use Aurora much this week, that's useful feedback too! Just let me know what got in the way.

Thanks again for your help!

[Your Name]
```

---

## Step 8: Synthesize Feedback

### After Testing Period:

1. Review survey responses for all testers
2. Document findings in `docs/beta/BETA_ROUND_1_FINDINGS.md`
3. Compare external tester sentiment with self-validation results
4. Identify common themes (resonant stories, UX friction, feature requests)
5. Make Go/No-Go decision based on combined data

---

## Key Resources Reference

### Local Documents:
- **Beta Tester Guide:** `docs/beta/BETA_TESTER_GUIDE.pdf`
- **Survey Links Metadata:** `docs/beta/SURVEY_LINKS.md` (contains form URLs and spreadsheet links)
- **Self-Validation Log:** `docs/beta/BETA_ROUND_1_SELF_VALIDATION.md`

### External Tools:
- **Google Play Console:** Manage Internal Testing track and tester list
- **Google Forms:** Baseline and weekly surveys (URLs in SURVEY_LINKS.md)
- **Google Sheets:** Survey response data (links in SURVEY_LINKS.md)

---

## Troubleshooting & Edge Cases

### Tester Ghosts After Initial Email
- **Action:** One follow-up after 3 days, then document as "Declined/No Response"
- **If all ghost:** Valid signal that concept lacks initial appeal

### Tester Completes Survey But Doesn't Install
- **Action:** Follow up: "Did you have any trouble with the installation link?"
- **Document:** Onboarding friction point

### Tester Installs But Doesn't Use
- **Action:** Reach out: "What happened after you installed? Any blockers?"
- **Document:** Potential first-launch experience issue

### Tester Reports Critical Bug
- **Action:** Reproduce locally, document as "Blocker" in findings
- **Decision:** May need to fix before recruiting additional testers

### Tester Doesn't Submit Weekly Survey
- **Action:** Send reminder on Day 7 evening
- **If still no response:** Personal follow-up via preferred channel (text, call, etc.)

---

## Success Metrics

### Recruitment:
- ✅ 1-4 testers recruited
- ✅ 75%+ install rate (3/4 testers install app)
- ✅ 50%+ survey completion rate (2/4 submit weekly survey)

### Feedback Quality:
- ✅ Specific examples in free-text responses (not just "It's good")
- ✅ At least 1 tester identifies UX friction not caught in self-validation
- ✅ Tester sentiment provides validation or new perspective vs. self-validation

---

**Last Updated:** 2025-12-31
