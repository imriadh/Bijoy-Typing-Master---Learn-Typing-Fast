# Phase 2: Engagement & Gamification Features

**Status:** 🚧 In Progress  
**Start Date:** February 10, 2026  
**Target:** 7 New Features to Boost User Engagement

---

## 🎯 Phase 2 Objectives

**Transform Bijoy Typing Master from a learning tool into an engaging platform:**
- Make practice fun and addictive with gamification
- Encourage daily usage with challenges and streaks
- Reward progress with achievements and badges
- Foster healthy competition with leaderboards
- Provide flexibility with custom practice modes

---

## 🎮 Feature Breakdown (7 Features)

### 1. 📅 Daily Challenges System
**Priority:** HIGH  
**Complexity:** Medium  
**Estimated Time:** 45 minutes

**Description:**  
Automatically generated daily typing challenges that encourage users to practice every day. Builds habit formation through streaks and rewards.

**Components:**
- **Model:** `DailyChallenge.cs`
  - Properties: Id, Date, ChallengeType, TargetText, TargetWPM, TargetAccuracy, TimeLimit, IsCompleted, CompletedAt, AchievedWPM, AchievedAccuracy, XPEarned
  - Enum: ChallengeType (SpeedChallenge, AccuracyChallenge, ComboChallenge, EnduranceChallenge)

- **Service:** `DailyChallengeManager.cs`
  - GenerateDailyChallenge() - Creates new challenge at midnight
  - GetTodayChallenge() - Retrieves current day's challenge
  - CompleteChallenge(result) - Marks challenge complete, awards XP
  - GetChallengeStreak() - Calculates consecutive days
  - GetChallengeHistory(days) - Past challenge results

- **Database:**
  - Table: DailyChallenges (Id, Date, Type, TargetText, TargetWPM, TargetAccuracy, TimeLimit, IsCompleted, CompletedAt, AchievedWPM, AchievedAccuracy, XPEarned)

- **UI:** `Views/DailyChallengeWindow.xaml/.cs`
  - Shows today's challenge with requirements
  - Start challenge button → typing interface
  - Real-time progress vs. target
  - Completion animation with XP earned
  - Streak counter with flame icon
  - Calendar view of completed challenges

**Challenge Types:**
1. **Speed Challenge:** "Type at [X] WPM or faster"
2. **Accuracy Challenge:** "Maintain [X]% accuracy or higher"
3. **Combo Challenge:** "Achieve [X] WPM AND [Y]% accuracy"
4. **Endurance Challenge:** "Type for [X] minutes without stopping"

**Rewards:**
- Base XP: 50-200 (based on difficulty)
- Streak bonus: +10 XP per consecutive day
- Perfect completion: +50 XP bonus

---

### 2. 🏆 Achievement/Badge System
**Priority:** HIGH  
**Complexity:** Medium  
**Estimated Time:** 60 minutes

**Description:**  
Unlockable achievements and badges that reward milestones and special accomplishments. Provides long-term goals and sense of progression.

**Components:**
- **Model:** `Achievement.cs`
  - Properties: Id, Name, Description, Icon, Category, Tier (Bronze/Silver/Gold/Platinum), RequirementType, RequirementValue, XPReward, IsUnlocked, UnlockedAt, Progress, MaxProgress
  - Enum: AchievementCategory (Speed, Accuracy, Consistency, Practice, Mastery, Special)
  - Enum: RequirementType (TotalWPM, SingleWPM, TotalAccuracy, SingleAccuracy, LessonsCompleted, DaysStreak, TestsCompleted, TotalPracticeTime)

- **Service:** `AchievementManager.cs`
  - GetAllAchievements() - Returns 30+ predefined achievements
  - GetUnlockedAchievements() - User's earned badges
  - CheckAchievements(stats) - Evaluates progress after each session
  - UnlockAchievement(id) - Awards badge, XP, shows notification
  - GetAchievementProgress(id) - Current progress %

- **Database:**
  - Table: Achievements (Id, Name, Description, Icon, Category, Tier, RequirementType, RequirementValue, XPReward)
  - Table: UserAchievements (UserId, AchievementId, UnlockedAt, Progress)

- **UI:** `Views/AchievementsWindow.xaml/.cs`
  - Grid of achievement cards (locked/unlocked states)
  - Filter by category (All, Speed, Accuracy, etc.)
  - Progress bars for in-progress achievements
  - Popup notification on unlock
  - Total XP and achievement count

**Achievement Examples (30 total):**

**Speed Achievements:**
- "First Steps" - Reach 10 WPM (Bronze, 50 XP)
- "Speed Demon" - Reach 50 WPM (Silver, 150 XP)
- "Lightning Fingers" - Reach 80 WPM (Gold, 300 XP)
- "Sonic Typer" - Reach 100 WPM (Platinum, 500 XP)

**Accuracy Achievements:**
- "Perfectionist" - 100% accuracy in a test (Gold, 200 XP)
- "Steady Hand" - 95%+ accuracy for 10 tests (Silver, 150 XP)
- "Precision Master" - 98%+ average accuracy (Gold, 300 XP)

**Consistency Achievements:**
- "Daily Dedication" - 7-day practice streak (Silver, 100 XP)
- "Month Master" - 30-day practice streak (Gold, 500 XP)
- "Year Legend" - 365-day practice streak (Platinum, 2000 XP)

**Practice Achievements:**
- "Beginner Graduate" - Complete 10 lessons (Bronze, 100 XP)
- "Lesson Master" - Complete all 30 lessons (Gold, 400 XP)
- "Marathon Typer" - 10 hours total practice time (Silver, 200 XP)

**Special Achievements:**
- "Night Owl" - Practice at 2 AM (Bronze, 50 XP)
- "Early Bird" - Practice at 6 AM (Bronze, 50 XP)
- "Weekend Warrior" - Complete 5 challenges on weekends (Silver, 150 XP)

---

### 3. 🎯 Mini-Games (Type Race & Word Hunter)
**Priority:** MEDIUM  
**Complexity:** High  
**Estimated Time:** 90 minutes

**Description:**  
Two arcade-style typing games that make practice fun and competitive. Combines entertainment with skill development.

#### Game 1: Type Race 🏎️
**Concept:** Race against time/opponent by typing words to move forward

- **Mechanics:**
  - Horizontal race track with player car
  - Random Bangla words scroll from right to left
  - Type word correctly → car moves forward
  - Mistake → car slows down
  - Race length: 1000 meters
  - Timer: 120 seconds

- **Scoring:**
  - Speed bonus: WPM-based multiplier
  - Accuracy penalty: -10% per error
  - Time bonus: Finish early → bonus points
  - Final score: (Words × 10) + (WPM × 5) + (TimeBonus)

- **UI:** `Views/TypeRaceGameWindow.xaml/.cs`
  - Animated race track visual
  - Current word display (large font)
  - Progress bar (distance covered)
  - WPM/Accuracy meters
  - Countdown timer
  - Leaderboard integration

#### Game 2: Word Hunter 🎯
**Concept:** Type words before they disappear, arcade-style

- **Mechanics:**
  - Words appear at random positions on screen
  - Each word has lifetime: 5-8 seconds (based on length)
  - Type word correctly → word explodes, +points
  - Word expires → -1 life (total 3 lives)
  - Difficulty increases: more words, shorter lifetimes
  - Game ends: when lives = 0 or 60 seconds elapsed

- **Scoring:**
  - Short word (3-5 chars): 10 points
  - Medium word (6-8 chars): 20 points
  - Long word (9+ chars): 30 points
  - Combo multiplier: consecutive words × 1.5×
  - Speed bonus: Type in <2 seconds → ×2 points

- **UI:** `Views/WordHunterGameWindow.xaml/.cs`
  - Canvas with floating word labels
  - Lives indicator (❤️❤️❤️)
  - Score counter
  - Combo meter
  - High score display
  - Particle effects on word destroy

**Shared Components:**
- **Model:** `GameResult.cs`
  - Properties: GameType, Score, Duration, WPM, Accuracy, HighScore, XPEarned
- **Service:** `GameManager.cs`
  - GetRandomWords(count, difficulty) - Word generation
  - CalculateScore(gameResult) - Scoring logic
  - SaveGameResult(result) - Database persistence
  - GetHighScores(gameType, limit) - Leaderboard

---

### 4. 🔖 Lesson Bookmarks
**Priority:** LOW  
**Complexity:** Easy  
**Estimated Time:** 30 minutes

**Description:**  
Allow users to bookmark favorite lessons for quick access. Improves navigation for focused practice.

**Components:**
- **Database:**
  - Table: BookmarkedLessons (UserId, LessonNumber, BookmarkedAt, Notes)

- **Service:** Extend `LessonManager.cs`
  - BookmarkLesson(lessonNumber, notes) - Add bookmark
  - UnbookmarkLesson(lessonNumber) - Remove bookmark
  - GetBookmarkedLessons() - Retrieve user's bookmarks
  - IsBookmarked(lessonNumber) - Check status

- **UI Updates:**
  - Add bookmark star icon to lesson list items
  - Toggle bookmark on click
  - "Bookmarked" filter in lesson browser
  - Optional notes field per bookmark

**Features:**
- Star icon toggles between ⭐ (bookmarked) and ☆ (not bookmarked)
- Bookmarks section on MainPage for quick access
- Personal notes per lesson (e.g., "Need more practice with juktakkhor")

---

### 5. 📝 Custom Text Practice Mode
**Priority:** MEDIUM  
**Complexity:** Easy  
**Estimated Time:** 40 minutes

**Description:**  
Users can import their own text to practice typing. Useful for real-world scenarios like typing essays, articles, or specific content.

**Components:**
- **Model:** `CustomPracticeSession.cs`
  - Properties: Id, Title, CustomText, CreatedAt, LastPracticed, TimesCompleted, BestWPM, BestAccuracy

- **Service:** `CustomTextManager.cs`
  - SaveCustomText(title, text) - Store user text
  - GetSavedTexts() - Retrieve library
  - DeleteCustomText(id) - Remove text
  - ImportFromFile(path) - Load from .txt file
  - ValidateText(text) - Check for Bangla characters, length

- **Database:**
  - Table: CustomTexts (Id, Title, Text, CreatedAt, LastPracticed, TimesCompleted, BestWPM, BestAccuracy)

- **UI:** `Views/CustomPracticeWindow.xaml/.cs`
  - Text input area (multi-line editor)
  - Title field
  - "Import from File" button (.txt file picker)
  - "Save & Practice" button
  - Library of saved texts (list view)
  - Start practice from saved text
  - Delete saved texts

**Features:**
- Support for Bangla Unicode and Bijoy keyboard
- Text validation: minimum 50 characters, contains Bangla
- Preview before practice
- Reuse saved texts multiple times
- Track best performance per custom text

---

### 6. 🏅 Local Leaderboard
**Priority:** MEDIUM  
**Complexity:** Medium  
**Estimated Time:** 45 minutes

**Description:**  
Competitive rankings based on various metrics. Motivates improvement through friendly competition (single-user device, comparing historical sessions).

**Components:**
- **Model:** `LeaderboardEntry.cs`
  - Properties: Rank, SessionDate, Category, Value, WPM, Accuracy, LessonOrTest, AchievementCount

- **Service:** `LeaderboardManager.cs`
  - GetTopSessions(category, limit) - Top 10/20/50
  - GetUserRank(category) - Current ranking
  - GetLeaderboardByCategory(category) - Full rankings
  - Categories: BestWPM, BestAccuracy, MostLessons, MostAchievements, LongestStreak, HighestXP

- **Database:**
  - Use existing UserProgress, SpeedTestResults tables
  - Aggregate queries for rankings

- **UI:** `Views/LeaderboardWindow.xaml/.cs`
  - Tab view for categories:
    * 🚀 Highest WPM
    * 🎯 Best Accuracy
    * 📚 Most Lessons Completed
    * 🏆 Most Achievements
    * 🔥 Longest Streak
    * ⭐ Total XP
  - Table with rank, date, value, details
  - Highlight user's best session
  - Filter by time period (week, month, all-time)

**Visual Design:**
- Gold/Silver/Bronze colors for top 3 ranks
- Trophy icons: 🥇🥈🥉
- User's entry highlighted in different color
- Progress to next rank indicator

---

### 7. 💎 XP & Level System
**Priority:** HIGH  
**Complexity:** Medium  
**Estimated Time:** 50 minutes

**Description:**  
Experience points and level progression system that ties all gamification features together. Provides overarching sense of advancement.

**Components:**
- **Model:** `UserProfile.cs`
  - Properties: UserId, TotalXP, CurrentLevel, XPForNextLevel, TotalLessons, TotalTests, TotalAchievements, JoinDate, LastActive, Streak

- **Service:** `XPManager.cs`
  - CalculateLevel(totalXP) - XP to level formula
  - GetXPForNextLevel(currentLevel) - Required XP
  - AwardXP(amount, source) - Give XP, check level up
  - GetXPSources() - Breakdown of XP earned
  - LevelUpRewards(newLevel) - Unlock features/badges

- **XP Sources:**
  - Complete lesson: 25 XP
  - Complete speed test: 30 XP
  - Daily challenge: 50-200 XP
  - Unlock achievement: 50-500 XP (tier-based)
  - Play mini-game: 10-50 XP (score-based)
  - 7-day streak: 100 XP bonus
  - Custom text practice: 15 XP

- **Level Formula:**
  - Level 1: 0 XP
  - Level 2: 100 XP
  - Level 3: 250 XP
  - Level N: `100 × N + 50 × N^2` XP
  - Max level: 50 (25,000+ XP)

- **Database:**
  - Table: UserProfile (UserId, TotalXP, CurrentLevel, JoinDate, LastActive, Streak)
  - Table: XPHistory (Id, UserId, Date, Amount, Source, Description)

- **UI Updates:**
  - Add XP bar to MainPage header
  - Level badge display
  - Progress to next level (e.g., "Level 5 - 75/500 XP")
  - Level-up animation/notification
  - XP breakdown chart in StatisticsWindow

**Level-Up Rewards:**
- Level 5: Unlock custom themes
- Level 10: Unlock mini-games
- Level 15: Unlock advanced statistics
- Level 20: Special "Master Typer" achievement
- Level 30: Gold theme unlock
- Level 50: Platinum achievement + special certificate

---

## 🗂️ File Structure (Phase 2)

### New Files to Create (28):

**Models (7):**
- `Models/DailyChallenge.cs`
- `Models/Achievement.cs`
- `Models/GameResult.cs`
- `Models/CustomPracticeSession.cs`
- `Models/LeaderboardEntry.cs`
- `Models/UserProfile.cs`
- `Models/XPHistory.cs`

**Services (7):**
- `Services/DailyChallengeManager.cs`
- `Services/AchievementManager.cs`
- `Services/GameManager.cs`
- `Services/CustomTextManager.cs`
- `Services/LeaderboardManager.cs`
- `Services/XPManager.cs`
- `Services/NotificationService.cs`

**Views (14):**
- `Views/DailyChallengeWindow.xaml` + `.cs`
- `Views/AchievementsWindow.xaml` + `.cs`
- `Views/TypeRaceGameWindow.xaml` + `.cs`
- `Views/WordHunterGameWindow.xaml` + `.cs`
- `Views/CustomPracticeWindow.xaml` + `.cs`
- `Views/LeaderboardWindow.xaml` + `.cs`
- `Views/UserProfileWindow.xaml` + `.cs`

### Files to Modify (4):

- `Services/DatabaseManager.cs` - Add 6 new tables
- `Views/MainPage.xaml` - Add XP bar, new navigation buttons
- `Views/MainPage.xaml.cs` - Add navigation handlers
- `MauiProgram.cs` - Register new services + views

---

## 📊 Database Schema Updates

### New Tables (6):

```sql
-- Daily Challenges
CREATE TABLE DailyChallenges (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date TEXT NOT NULL,
    ChallengeType TEXT NOT NULL,
    TargetText TEXT NOT NULL,
    TargetWPM INTEGER,
    TargetAccuracy INTEGER,
    TimeLimit INTEGER,
    IsCompleted INTEGER DEFAULT 0,
    CompletedAt TEXT,
    AchievedWPM INTEGER,
    AchievedAccuracy INTEGER,
    XPEarned INTEGER DEFAULT 0
);

-- Achievements (predefined 30 achievements)
CREATE TABLE Achievements (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT NOT NULL,
    Icon TEXT,
    Category TEXT NOT NULL,
    Tier TEXT NOT NULL,
    RequirementType TEXT NOT NULL,
    RequirementValue INTEGER NOT NULL,
    XPReward INTEGER NOT NULL
);

-- User Achievements (unlocked status)
CREATE TABLE UserAchievements (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER DEFAULT 1,
    AchievementId INTEGER NOT NULL,
    UnlockedAt TEXT NOT NULL,
    Progress INTEGER DEFAULT 0,
    FOREIGN KEY (AchievementId) REFERENCES Achievements(Id)
);

-- Game Results
CREATE TABLE GameResults (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    GameType TEXT NOT NULL,
    Score INTEGER NOT NULL,
    Duration INTEGER NOT NULL,
    WPM INTEGER,
    Accuracy INTEGER,
    HighScore INTEGER,
    XPEarned INTEGER,
    PlayedAt TEXT NOT NULL
);

-- Custom Texts
CREATE TABLE CustomTexts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Text TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    LastPracticed TEXT,
    TimesCompleted INTEGER DEFAULT 0,
    BestWPM INTEGER DEFAULT 0,
    BestAccuracy INTEGER DEFAULT 0
);

-- User Profile & XP
CREATE TABLE UserProfile (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER DEFAULT 1,
    TotalXP INTEGER DEFAULT 0,
    CurrentLevel INTEGER DEFAULT 1,
    JoinDate TEXT NOT NULL,
    LastActive TEXT,
    Streak INTEGER DEFAULT 0
);

-- XP History
CREATE TABLE XPHistory (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER DEFAULT 1,
    Date TEXT NOT NULL,
    Amount INTEGER NOT NULL,
    Source TEXT NOT NULL,
    Description TEXT
);

-- Bookmarked Lessons
CREATE TABLE BookmarkedLessons (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER DEFAULT 1,
    LessonNumber INTEGER NOT NULL,
    BookmarkedAt TEXT NOT NULL,
    Notes TEXT
);
```

---

## 🎨 UI/UX Enhancements

### MainPage Updates:
- Add XP bar at top: `[Level 5] ████████░░ 450/500 XP`
- New buttons:
  - 📅 Daily Challenge (badge shows "NEW" if incomplete)
  - 🏆 Achievements (shows count: "15/30")
  - 🎮 Mini-Games (submenu: Type Race, Word Hunter)
  - 📝 Custom Practice
  - 🏅 Leaderboard

### Visual Elements:
- **XP Progress Bar:** Gradient from blue to gold
- **Level Badge:** Circular badge with level number
- **Achievement Popup:** Animated toast notification on unlock
- **Streak Flame:** 🔥 icon with number of consecutive days
- **Star Ratings:** ⭐⭐⭐⭐⭐ for performance
- **Trophy Icons:** 🏆 for achievements, 🥇🥈🥉 for leaderboard

### Animations:
- Level-up: Confetti + sound effect
- Achievement unlock: Badge slide-in + glow
- Daily challenge complete: Check mark animation
- Game score: Number count-up animation

---

## 🎯 Success Metrics

### User Engagement:
- ✅ Daily challenge completion rate > 70%
- ✅ Average session length increases by 50%
- ✅ Streak retention > 30% at 7 days
- ✅ Achievement unlock rate > 10 per user

### Feature Usage:
- ✅ Mini-games played > 5 times per user
- ✅ Custom text practice used by > 40% of users
- ✅ Leaderboard viewed weekly
- ✅ Lesson bookmarks > 3 per user

---

## ⏱️ Implementation Timeline

**Total Estimated Time:** 6-7 hours

**Priority Order:**
1. **Week 1 (Core Systems):**
   - XP & Level System (1 hour)
   - Daily Challenges (1 hour)
   - Achievement System (1 hour)

2. **Week 2 (Engagement):**
   - Mini-Games: Type Race (1.5 hours)
   - Mini-Games: Word Hunter (1.5 hours)

3. **Week 3 (Polish):**
   - Custom Text Practice (40 min)
   - Lesson Bookmarks (30 min)
   - Local Leaderboard (45 min)

---

## 🚀 Ready to Implement!

**Phase 2 will transform your typing tutor into an engaging, game-like experience that users will want to return to daily!**

Let's start with the core systems (XP, Daily Challenges, Achievements) and build from there. 💪

---

**Next Step:** Begin implementation with Feature 7 (XP System) as it's the foundation for other features.
