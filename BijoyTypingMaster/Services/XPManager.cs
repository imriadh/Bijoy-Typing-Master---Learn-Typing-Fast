using BijoyTypingMaster.Models;

namespace BijoyTypingMaster.Services;

/// <summary>
/// Manages XP (Experience Points) and level progression system
/// </summary>
public class XPManager
{
    private readonly DatabaseManager _dbManager;

    public XPManager(DatabaseManager dbManager)
    {
        _dbManager = dbManager;
    }

    /// <summary>
    /// Awards XP to the user and checks for level up
    /// </summary>
    /// <returns>Tuple: (newLevel, leveledUp)</returns>
    public async Task<(int newLevel, bool leveledUp)> AwardXPAsync(int amount, string source, string description)
    {
        if (amount <= 0) return (1, false);

        var profile = await GetOrCreateUserProfileAsync();
        int oldLevel = profile.CurrentLevel;
        
        // Add XP
        profile.TotalXP += amount;
        profile.LastActive = DateTime.Now;

        // Calculate new level
        profile.CurrentLevel = CalculateLevel(profile.TotalXP);
        bool leveledUp = profile.CurrentLevel > oldLevel;

        // Save profile
        await _dbManager.UpdateUserProfileAsync(profile);

        // Log XP history
        var xpHistory = new XPHistory
        {
            UserId = profile.UserId,
            Date = DateTime.Now,
            Amount = amount,
            Source = source,
            Description = description
        };
        await _dbManager.AddXPHistoryAsync(xpHistory);

        return (profile.CurrentLevel, leveledUp);
    }

    /// <summary>
    /// Calculates level based on total XP
    /// Formula: 100 * N + 50 * N^2
    /// </summary>
    public int CalculateLevel(int totalXP)
    {
        if (totalXP < 100) return 1;

        // Iterate through levels until we find where the XP fits
        for (int level = 1; level <= 50; level++)
        {
            int xpForNextLevel = 100 * (level + 1) + 50 * (level + 1) * (level + 1);
            if (totalXP < xpForNextLevel)
            {
                return level;
            }
        }

        return 50; // Max level
    }

    /// <summary>
    /// Gets or creates user profile
    /// </summary>
    public async Task<UserProfile> GetOrCreateUserProfileAsync()
    {
        var profile = await _dbManager.GetUserProfileAsync();
        if (profile == null)
        {
            profile = new UserProfile
            {
                UserId = 1,
                TotalXP = 0,
                CurrentLevel = 1,
                JoinDate = DateTime.Now,
                LastActive = DateTime.Now,
                Streak = 0
            };
            await _dbManager.CreateUserProfileAsync(profile);
        }
        return profile;
    }

    /// <summary>
    /// Updates streak based on last active date
    /// </summary>
    public async Task<int> UpdateStreakAsync()
    {
        var profile = await GetOrCreateUserProfileAsync();
        var today = DateTime.Today;
        var lastActive = profile.LastActive.Date;

        if (lastActive == today)
        {
            // Already practiced today
            return profile.Streak;
        }
        else if (lastActive == today.AddDays(-1))
        {
            // Practiced yesterday - continue streak
            profile.Streak++;
            
            // Award streak bonus XP
            if (profile.Streak == 7)
            {
                await AwardXPAsync(XPRewards.StreakBonus7Days, XPSource.StreakBonus, "7-day streak!");
            }
            else if (profile.Streak == 30)
            {
                await AwardXPAsync(XPRewards.StreakBonus30Days, XPSource.StreakBonus, "30-day streak!");
            }
            else if (profile.Streak > 1)
            {
                // Daily streak bonus
                int bonus = XPRewards.StreakBonusPerDay * profile.Streak;
                await AwardXPAsync(bonus, XPSource.StreakBonus, $"{profile.Streak}-day streak!");
            }
        }
        else
        {
            // Streak broken
            profile.Streak = 1;
        }

        profile.LastActive = today;
        await _dbManager.UpdateUserProfileAsync(profile);
        return profile.Streak;
    }

    /// <summary>
    /// Gets XP history for a date range
    /// </summary>
    public async Task<List<XPHistory>> GetXPHistoryAsync(int days = 30)
    {
        var startDate = DateTime.Now.AddDays(-days);
        return await _dbManager.GetXPHistoryAsync(startDate);
    }

    /// <summary>
    /// Gets XP breakdown by source
    /// </summary>
    public async Task<Dictionary<string, int>> GetXPBreakdownAsync()
    {
        var history = await _dbManager.GetXPHistoryAsync(DateTime.MinValue);
        return history
            .GroupBy(x => x.Source)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));
    }

    /// <summary>
    /// Awards XP for completing a lesson
    /// </summary>
    public async Task<(int newLevel, bool leveledUp)> AwardLessonXPAsync(int lessonNumber)
    {
        return await AwardXPAsync(
            XPRewards.LessonComplete, 
            XPSource.LessonComplete, 
            $"Completed Lesson {lessonNumber}"
        );
    }

    /// <summary>
    /// Awards XP for completing a speed test
    /// </summary>
    public async Task<(int newLevel, bool leveledUp)> AwardSpeedTestXPAsync(int wpm, int accuracy)
    {
        // Base XP + performance bonus
        int bonus = (wpm / 10) + (accuracy / 10);
        int totalXP = XPRewards.SpeedTest + bonus;
        
        return await AwardXPAsync(
            totalXP, 
            XPSource.SpeedTest, 
            $"Speed Test: {wpm} WPM, {accuracy}% accuracy"
        );
    }

    /// <summary>
    /// Awards XP for custom practice session
    /// </summary>
    public async Task<(int newLevel, bool leveledUp)> AwardCustomPracticeXPAsync(string title)
    {
        return await AwardXPAsync(
            XPRewards.CustomPractice, 
            XPSource.CustomPractice, 
            $"Custom Practice: {title}"
        );
    }

    /// <summary>
    /// Gets level-up rewards for a specific level
    /// </summary>
    public string GetLevelUpReward(int level)
    {
        return level switch
        {
            5 => "🎨 Unlocked: Custom Themes",
            10 => "🎮 Unlocked: Mini-Games",
            15 => "📊 Unlocked: Advanced Statistics",
            20 => "⭐ Achievement: Master Typer",
            30 => "🥇 Unlocked: Gold Theme",
            40 => "💎 Unlocked: Premium Features",
            50 => "🏆 Achievement: Typing Legend (Max Level!)",
            _ => level % 5 == 0 ? "🎁 Milestone Reward!" : "🌟 Level Up!"
        };
    }
}
