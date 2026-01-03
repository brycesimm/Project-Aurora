# Post-Beta Round 1 Improvement Requirements

**Source:** Beta Round 1 self-validation findings (2025-12-28 to 2026-01-04)
**Status:** Requirements documented for future planning session
**Last Updated:** 2026-01-03

This document tracks improvement requirements identified during Beta Round 1 testing. These are **not yet full stories** - they represent high-level needs that will be broken down into actionable stories during a future planning session.

---

## Blocker Priority (Beta Round 1 - Fix Before External Tester Expansion)

**None identified.** No critical bugs preventing external testing expansion.

---

## High Priority (Beta Round 2 Prerequisites)

These improvements should be implemented before recruiting additional external testers for Beta Round 2.

### 1. Pull-to-Refresh Functionality

**Problem:**
Users have no way to manually check for new content updates. Must force-stop app or wait for automatic refresh.

**Why It Matters:**
- Friction point identified in self-validation ("Missing refresh button")
- Standard mobile UX pattern expected by users
- Provides user control over content updates (important for morning routine use case)

**Requirements:**
- Add pull-to-refresh gesture to Daily Picks list
- Trigger content reload from API (existing GetDailyContent endpoint)
- Show loading indicator during refresh
- Display success/failure feedback (toast or snackbar)

**Open Questions:**
- Should refresh also reload Vibe of the Day, or only Daily Picks?
- Cache invalidation strategy?

---

### 2. Uplift Button State Persistence

**Problem:**
Uplift button doesn't track state (no visual indication of already-tapped stories). Users cannot remember which stories they've already uplifted.

**Why It Matters:**
- Degrades user experience (repeated taps feel meaningless)
- No feedback loop for user engagement
- Common mobile interaction pattern (like "like" buttons should show state)

**Requirements:**
- Persist tapped state locally (SQLite, Preferences, or similar)
- Show visual state difference: filled icon (tapped) vs. outlined icon (not tapped)
- Sync state across app restarts
- Consider: Should tapped state reset when new content loads?

**Open Questions:**
- Local-only persistence or sync to backend?
- State lifetime: permanent, daily reset, or reset on new content?
- UI design: filled/outlined icon, color change, or both?

---

### 3. Bottom Navigation Bar (Multi-Page Foundation)

**Problem:**
Single-page app feels incomplete. Most apps have navigation bars and menus for multiple features. Users expect multi-page architecture.

**Why It Matters:**
- Self-validation feedback: "Only having one page makes the app feel like it's missing something"
- Sets foundation for future features (videos, community, profile)
- Provides visual affordance for app growth

**Requirements:**
- Add bottom navigation bar UI component
- Define 3-4 tabs (e.g., Home, Discover, Community, Profile)
- Home tab: Current feed (Vibe + Daily Picks) - fully functional
- Other tabs: Placeholder pages with "Coming Soon" messaging
- Ensure tab selection state persists during app lifecycle

**Open Questions:**
- Tab names and icon choices (align with Aurora brand)
- Which tabs to include initially? (Home + 2-3 placeholders)
- Navigation library recommendation (.NET MAUI Shell vs. custom implementation)?

---

## Medium Priority (Post-Beta Round 2)

These enhancements improve user experience but are not blockers for external testing expansion.

### 4. Video Content Support

**Problem:**
Content limited to articles only. User worked through stories in two sessions (morning/evening) but still went to other apps for video content.

**Why It Matters:**
- Self-validation feedback: "I wish there was more content besides articles"
- Video consumption preference for many users
- Expands content variety and engagement time
- Differentiates Aurora from text-only news aggregators

**Requirements:**
- Support video content type in content schema (YouTube, TikTok, Shorts)
- Add video player integration (embedded or external)
- Update curation workflow to include video discovery
- Decide: Inline video player vs. external app launch?

**Open Questions:**
- Video sources: YouTube only, or multi-platform (TikTok, Instagram Reels, etc.)?
- Inline playback vs. external app (Chrome Custom Tabs pattern)?
- Video thumbnail generation and storage?
- Curation effort increase: How to find uplifting video content efficiently?

---

### 5. Content Filtering by Category/Theme

**Problem:**
No way to filter content by type or theme. Users see all content regardless of interest areas.

**Why It Matters:**
- Self-validation feedback: "Would appreciate filtering based on content type to allow user control over feed"
- Personalization enhances engagement
- Allows users to focus on specific interest areas (environment, health, technology, etc.)

**Requirements:**
- Define category taxonomy (Environment, Health, Technology, Arts, Community, Science, etc.)
- Tag content during curation with categories
- Add filter UI (toggle buttons, chips, or dropdown)
- Filter content client-side or server-side?
- Persist user filter preferences

**Open Questions:**
- Category taxonomy: How many categories? Fixed or expandable?
- UI placement: Top of feed, in navigation drawer, or dedicated filter page?
- Default behavior: Show all categories or require user selection?

---

### 6. Community Features (Comments/Discussion)

**Problem:**
Passive consumption only; no social engagement. User desires "community-related things like maybe posts/comments."

**Why It Matters:**
- Self-validation feedback: Multiple mentions of desire for community features
- Builds user retention through social engagement
- Allows users to share personal positive stories
- Creates sense of community around positivity mission

**Requirements:**
- Commenting system for articles/videos (optional, not required)
- User-generated story submissions (queue for manual curation)
- Community moderation strategy (pre-approval, flagging, etc.)
- User authentication required (impacts MVP "no accounts" decision)

**Open Questions:**
- Does this require user accounts? (Major scope change from MVP)
- Moderation burden: Manual review or automated tools?
- Comment threading depth? Reactions on comments?
- Community guidelines and enforcement strategy?

---

## Low Priority (Future Backlog)

Nice-to-have features that are not critical for beta testing or initial public release.

### 7. Dark Mode Support

**Problem:**
App only supports light theme. Some users prefer dark mode for eye strain reduction or battery savings.

**Why It Matters:**
- Standard mobile OS feature (system dark mode toggle)
- Accessibility consideration
- User preference accommodation

**Requirements:**
- Implement dark theme variant of "Morning Mist" design system
- Respect system dark mode setting
- Provide in-app toggle (optional)

**Open Questions:**
- Auto-follow system theme or manual toggle?
- Dark theme color palette (maintain Morning Mist pastel feel or shift to darker tones)?

---

### 8. Cross-Device Sync (User Accounts)

**Problem:**
No user accounts; cannot sync preferences, Uplift state, or reading history across devices.

**Why It Matters:**
- Multi-device users (phone + tablet) lose state
- Enables future premium features (save articles, personalized feeds, etc.)

**Requirements:**
- User authentication system (email, OAuth, etc.)
- Backend user profile storage
- Sync Uplift state, read articles, filter preferences
- Privacy considerations (anonymous usage OK?)

**Open Questions:**
- Authentication provider: Custom, Firebase, Auth0, or other?
- Anonymous usage still allowed or accounts required?
- Data privacy: GDPR compliance, data retention, deletion?

---

## Content Curation Tooling Improvements

These improvements address bottlenecks identified during content curation (Updates #1 and #2).

### 9. CLI Web Search Bug Fix

**Problem:**
Queuing multiple web searches causes glitch where hitting Escape only cancels first search, leaving others queued and blocking prompts. Required serial (one-at-a-time) searches during Update #2.

**Why It Matters:**
- Slows content discovery workflow
- Forces inefficient serial search pattern
- Developer experience degradation

**Requirements:**
- Investigate and fix queued web search behavior
- Ensure Escape cancels all queued operations, not just first
- Test with multiple parallel web searches

**Open Questions:**
- Root cause: Claude Code CLI bug or expected behavior?
- Workaround available or requires upstream fix?

---

### 10. Pre-Deployment Image URL Validation

**Problem:**
Deployed content first, discovered broken Vibe image URL on device, required second deployment during Update #2.

**Why It Matters:**
- Wastes deployment time (2 deployments instead of 1)
- Poor user experience if broken images reach production
- Preventable with automated validation

**Requirements:**
- Add `--check-images` flag to `Validate-Content.ps1`
- Perform HEAD requests to all image URLs during validation
- Fail validation if any images return 404, timeout, or 403
- Optional: Download and verify image file integrity (not just URL accessibility)

**Open Questions:**
- Should image validation be default behavior or opt-in?
- Timeout threshold for slow-loading images?
- Handling of redirect chains (e.g., URL shorteners)?

---

### 11. Story Queue Categorization/Tagging

**Problem:**
Story queue (`future-stories-queue.md`) lacks theme tagging. Significant time spent ensuring theme variety during Update #2 (medical, environment, conservation, heroism).

**Why It Matters:**
- Speeds up story selection for balanced content
- Enables intentional diversity (avoid 10 environment stories in one update)
- Improves curation efficiency

**Requirements:**
- Add theme/category tags to queued stories (markdown frontmatter, JSON, or inline tags)
- Filter queue by theme during selection
- Track theme distribution in `content.json` (e.g., "already used 3 environment stories this week")

**Open Questions:**
- Storage format: YAML frontmatter in Markdown, separate JSON file, or database?
- Category taxonomy alignment with user-facing filters (#5 above)?

---

### 12. RSS Feed Aggregation for Story Discovery

**Problem:**
Story discovery takes 20-25 minutes per update (58% of total curation time). Manual browsing across multiple sites without centralized feed.

**Why It Matters:**
- Largest bottleneck in curation workflow
- Could reduce curation time by 10-15 minutes per update
- Automates repetitive browsing task

**Requirements:**
- Subscribe to RSS feeds: r/UpliftingNews, Good News Network, Optimist Daily, Positive News
- Aggregate and filter by recency (published within last 7-14 days)
- Present unified feed for curator review
- Avoid paywalled sources (track known paywalls)

**Open Questions:**
- RSS reader tool: Build custom, use existing CLI tool, or web service?
- Auto-prioritization of stories by source credibility?
- Integration with story queue or separate workflow?

---

## Content Strategy Considerations

**Not feature requirements**, but strategic insights for discussion:

### Progressive-Leaning Audience Limitation

**Observation:**
Tester noted Aurora would be recommended to progressive friends but not conservative/right-wing individuals due to sources and topics stereotypically distrusted by conservative ideologies.

**Implications:**
- **Target Audience Narrowing:** Aurora may inherently appeal to progressive/liberal demographics
- **Source Diversity Consideration:** May need to balance progressive outlets with centrist/non-partisan sources for broader appeal
- **Mission Alignment:** "Refuge from negativity" may naturally skew progressive (environmental wins, social justice, labor rights, renewable energy, global health equity)

**Questions for Future Planning:**
- Is progressive-leaning audience acceptable, or should we strive for political neutrality?
- Can we find conservative-trusted sources for positive news (e.g., faith-based organizations, agricultural innovation, small business success)?
- Does Aurora's mission (countering fear/negativity) inherently align with progressive values?

---

**Document Status:** Requirements documented; awaiting planning session to convert into actionable stories with acceptance criteria.
