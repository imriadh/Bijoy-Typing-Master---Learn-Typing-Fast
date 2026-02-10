using BijoyTypingMaster.Models;

namespace BijoyTypingMaster.Services;

/// <summary>
/// Manages achievements and badges
/// </summary>
public class AchievementManager
{
    private readonly DatabaseManager _dbManager;
    private readonly XPManager _xpManager;
    private List<Achievement>? _allAchievements;

    public AchievementManager(DatabaseManager dbManager, XPManager xpManager)
    {
        _dbManager = dbManager;
        _xpManager = xpManager;
    }

    /// <summary>
    /// Gets all predefined achievements
    /// </summary>
    public List<Achievement> GetAllAchievements()
    {
        if (_allAchievements != null) return _allAchievements;

        _allAchievements = new List<Achievement>
        {
            // SPEED ACHIEVEMENTS
            new Achievement { Id = 1, Name = "First Steps", Description = "Reach 10 WPM", Icon = "👶", Category = AchievementCategory.Speed, Tier = AchievementTier.Bronze, RequirementType = RequirementType.SingleWPM, RequirementValue = 10, XPReward = 50 },
            new Achievement { Id = 2, Name = "Getting Faster", Description = "Reach 20 WPM", Icon = "🚶", Category = AchievementCategory.Speed, Tier = AchievementTier.Bronze, RequirementType = RequirementType.SingleWPM, RequirementValue = 20, XPReward = 75 },
            new Achievement { Id = 3, Name = "Speed Demon", Description = "Reach 50 WPM", Icon = "🏃", Category = AchievementCategory.Speed, Tier = AchievementTier.Silver, RequirementType = RequirementType.SingleWPM, RequirementValue = 50, XPReward = 150 },
            new Achievement { Id = 4, Name = "Lightning Fingers", Description = "Reach 80 WPM", Icon = "⚡", Category = AchievementCategory.Speed, Tier = AchievementTier.Gold, RequirementType = RequirementType.SingleWPM, RequirementValue = 80, XPReward = 300 },
            new Achievement { Id = 5, Name = "Sonic Typer", Description = "Reach 100 WPM", Icon = "🚀", Category = AchievementCategory.Speed, Tier = AchievementTier.Platinum, RequirementType = RequirementType.SingleWPM, RequirementValue = 100, XPReward = 500 },

            // ACCURACY ACHIEVEMENTS
            new Achievement { Id = 6, Name = "Perfectionist", Description = "100% accuracy in a test", Icon = "💯", Category = AchievementCategory.Accuracy, Tier = AchievementTier.Gold, RequirementType = RequirementType.SingleAccuracy, RequirementValue = 100, XPReward = 200 },
            new Achievement { Id = 7, Name = "Steady Hand", Description = "95%+ accuracy for 10 tests", Icon = "✋", Category = AchievementCategory.Accuracy, Tier = AchievementTier.Silver, RequirementType = RequirementType.PerfectTests, RequirementValue = 10, XPReward = 150 },
            new Achievement { Id = 8, Name = "Precision Master", Description = "98%+ average accuracy", Icon = "🎯", Category = AchievementCategory.Accuracy, Tier = AchievementTier.Gold, RequirementType = RequirementType.AverageAccuracy, RequirementValue = 98, XPReward = 300 },

            // CONSISTENCY ACHIEVEMENTS
            new Achievement { Id = 9, Name = "Daily Dedication", Description = "7-day practice streak", Icon = "📅", Category = AchievementCategory.Consistency, Tier = AchievementTier.Silver, RequirementType = RequirementType.DaysStreak, RequirementValue = 7, XPReward = 100 },
            new Achievement { Id = 10, Name = "Week Warrior", Description = "14-day practice streak", Icon = "💪", Category = AchievementCategory.Consistency, Tier = AchievementTier.Silver, RequirementType = RequirementType.DaysStreak, RequirementValue = 14, XPReward = 200 },
            new Achievement { Id = 11, Name = "Month Master", Description = "30-day practice streak", Icon = "🔥", Category = AchievementCategory.Consistency, Tier = AchievementTier.Gold, RequirementType = RequirementType.DaysStreak, RequirementValue = 30, XPReward = 500 },
            new Achievement { Id = 12, Name = "Year Legend", Description = "365-day practice streak", Icon = "👑", Category = AchievementCategory.Consistency, Tier = AchievementTier.Platinum, RequirementType = RequirementType.DaysStreak, RequirementValue = 365, XPReward = 2000 },

            // PRACTICE ACHIEVEMENTS
            new Achievement { Id = 13, Name = "Beginner Graduate", Description = "Complete 10 lessons", Icon = "🎓", Category = AchievementCategory.Practice, Tier = AchievementTier.Bronze, RequirementType = RequirementType.LessonsCompleted, RequirementValue = 10, XPReward = 100 },
            new Achievement { Id = 14, Name = "Intermediate Scholar", Description = "Complete 20 lessons", Icon = "📚", Category = AchievementCategory.Practice, Tier = AchievementTier.Silver, RequirementType = RequirementType.LessonsCompleted, RequirementValue = 20, XPReward = 200 },
            new Achievement { Id = 15, Name = "Lesson Master", Description = "Complete all 30 lessons", Icon = "🏆", Category = AchievementCategory.Practice, Tier = AchievementTier.Gold, RequirementType = RequirementType.LessonsCompleted, RequirementValue = 30, XPReward = 400 },
            new Achievement { Id = 16, Name = "Test Taker", Description = "Complete 10 speed tests", Icon = "📝", Category = AchievementCategory.Practice, Tier = AchievementTier.Bronze, RequirementType = RequirementType.TestsCompleted, RequirementValue = 10, XPReward = 100 },
            new Achievement { Id = 17, Name = "Marathon Typer", Description = "10 hours total practice", Icon = "⏰", Category = AchievementCategory.Practice, Tier = AchievementTier.Silver, RequirementType = RequirementType.TotalPracticeTime, RequirementValue = 600, XPReward = 200 },

            // MASTERY ACHIEVEMENTS
            new Achievement { Id = 18, Name = "Apprentice", Description = "Reach Level 5", Icon = "🥉", Category = AchievementCategory.Mastery, Tier = AchievementTier.Bronze, RequirementType = RequirementType.LevelReached, RequirementValue = 5, XPReward = 50 },
            new Achievement { Id = 19, Name = "Expert", Description = "Reach Level 10", Icon = "🥈", Category = AchievementCategory.Mastery, Tier = AchievementTier.Silver, RequirementType = RequirementType.LevelReached, RequirementValue = 10, XPReward = 100 },
            new Achievement { Id = 20, Name = "Master Typer", Description = "Reach Level 20", Icon = "🥇", Category = AchievementCategory.Mastery, Tier = AchievementTier.Gold, RequirementType = RequirementType.LevelReached, RequirementValue = 20, XPReward = 300 },
            new Achievement { Id = 21, Name = "Grandmaster", Description = "Reach Level 30", Icon = "💎", Category = AchievementCategory.Mastery, Tier = AchievementTier.Platinum, RequirementType = RequirementType.LevelReached, RequirementValue = 30, XPReward = 500 },
            new Achievement { Id = 22, Name = "XP Collector", Description = "Earn 1000 total XP", Icon = "⭐", Category = AchievementCategory.Mastery, Tier = AchievementTier.Silver, RequirementType = RequirementType.TotalXP, RequirementValue = 1000, XPReward = 100 },

            // SPECIAL ACHIEVEMENTS
            new Achievement { Id = 23, Name = "Night Owl", Description = "Practice at 2 AM", Icon = "🦉", Category = AchievementCategory.Special, Tier = AchievementTier.Bronze, RequirementType = RequirementType.LessonsCompleted, RequirementValue = 1, XPReward = 50 },
            new Achievement { Id = 24, Name = "Early Bird", Description = "Practice at 6 AM", Icon = "🐦", Category = AchievementCategory.Special, Tier = AchievementTier.Bronze, RequirementType = RequirementType.LessonsCompleted, RequirementValue = 1, XPReward = 50 },
            new Achievement { Id = 25, Name = "Weekend Warrior", Description = "Complete 5 challenges on weekends", Icon = "🎮", Category = AchievementCategory.Special, Tier = AchievementTier.Silver, RequirementType = RequirementType.ChallengesCompleted, RequirementValue = 5, XPReward = 150 },
            new Achievement { Id = 26, Name = "Challenge Champion", Description = "Complete 30 daily challenges", Icon = "🏅", Category = AchievementCategory.Special, Tier = AchievementTier.Gold, RequirementType = RequirementType.ChallengesCompleted, RequirementValue = 30, XPReward = 400 },
        };

        return _allAchievements;
    }

    /// <summary>
    /// Gets user's unlocked achievements
    /// </summary>
    public async Task<List<Achievement>> GetUnlockedAchievementsAsync()
    {
        var unlocked = await _dbManager.GetUserAchievementsAsync();
        var all = GetAllAchievements();

        foreach (var achievement in all)
        {
            var userAchievement = unlocked.FirstOrDefault(u => u.Id == achievement.Id);
            if (userAchievement != null)
            {
                achievement.IsUnlocked = true;
                achievement.UnlockedAt = userAchievement.UnlockedAt;
            }
        }

        return all.Where(a => a.IsUnlocked).ToList();
    }

    /// <summary>
    /// Gets all achievements with unlock status
    /// </summary>
    public async Task<List<Achievement>> GetAllAchievementsWithStatusAsync()
    {
        var unlocked = await _dbManager.GetUserAchievementsAsync();
        var all = GetAllAchievements();

        foreach (var achievement in all)
        {
            var userAchievement = unlocked.FirstOrDefault(u => u.Id == achievement.Id);
            if (userAchievement != null)
            {
                achievement.IsUnlocked = true;
                achievement.UnlockedAt = userAchievement.UnlockedAt;
                achievement.Progress = userAchievement.Progress;
            }
            else
            {
                // Calculate progress for locked achievements
                achievement.Progress = await CalculateProgressAsync(achievement);
            }
        }

        return all;
    }

    /// <summary>
    /// Checks and unlocks achievements based on current stats
    /// </summary>
    public async Task<List<Achievement>> CheckAndUnlockAchievementsAsync()
    {
        var newlyUnlocked = new List<Achievement>();
        var all = await GetAllAchievementsWithStatusAsync();
        var profile = await _xpManager.GetOrCreateUserProfileAsync();

        foreach (var achievement in all.Where(a => !a.IsUnlocked))
        {
            bool shouldUnlock = false;

            switch (achievement.RequirementType)
            {
                case RequirementType.SingleWPM:
                case RequirementType.SingleAccuracy:
                case RequirementType.PerfectTests:
                    // These are checked immediately after tests
                    break;

                case RequirementType.LessonsCompleted:
                    shouldUnlock = profile.TotalLessonsCompleted >= achievement.RequirementValue;
                    break;

                case RequirementType.TestsCompleted:
                    shouldUnlock = profile.TotalTestsCompleted >= achievement.RequirementValue;
                    break;

                case RequirementType.DaysStreak:
                    shouldUnlock = profile.Streak >= achievement.RequirementValue;
                    break;

                case RequirementType.TotalXP:
                    shouldUnlock = profile.TotalXP >= achievement.RequirementValue;
                    break;

                case RequirementType.LevelReached:
                    shouldUnlock = profile.CurrentLevel >= achievement.RequirementValue;
                    break;
            }

            if (shouldUnlock)
            {
                await UnlockAchievementAsync(achievement);
                newlyUnlocked.Add(achievement);
            }
        }

        return newlyUnlocked;
    }

    /// <summary>
    /// Unlocks a specific achievement
    /// </summary>
    public async Task UnlockAchievementAsync(Achievement achievement)
    {
        achievement.IsUnlocked = true;
        achievement.UnlockedAt = DateTime.Now;

        await _dbManager.UnlockAchievementAsync(achievement.Id);
        await _xpManager.AwardXPAsync(
            achievement.XPReward, 
            XPSource.Achievement, 
            $"Unlocked: {achievement.Name}"
        );

        // Update profile achievement count
        var profile = await _xpManager.GetOrCreateUserProfileAsync();
        profile.TotalAchievementsUnlocked++;
        await _dbManager.UpdateUserProfileAsync(profile);
    }

    /// <summary>
    /// Calculates current progress for an achievement
    /// </summary>
    private async Task<int> CalculateProgressAsync(Achievement achievement)
    {
        var profile = await _xpManager.GetOrCreateUserProfileAsync();

        return achievement.RequirementType switch
        {
            RequirementType.LessonsCompleted => profile.TotalLessonsCompleted,
            RequirementType.TestsCompleted => profile.TotalTestsCompleted,
            RequirementType.DaysStreak => profile.Streak,
            RequirementType.TotalXP => profile.TotalXP,
            RequirementType.LevelReached => profile.CurrentLevel,
            RequirementType.TotalPracticeTime => profile.TotalPracticeTimeMinutes,
            _ => 0
        };
    }
}
