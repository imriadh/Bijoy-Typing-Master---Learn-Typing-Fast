namespace BijoyTypingMaster.Models;

/// <summary>
/// Represents user profile with XP, level, and overall statistics
/// </summary>
public class UserProfile
{
    public int Id { get; set; }
    public int UserId { get; set; } = 1; // Single user application
    public int TotalXP { get; set; }
    public int CurrentLevel { get; set; } = 1;
    public DateTime JoinDate { get; set; }
    public DateTime LastActive { get; set; }
    public int Streak { get; set; } // Consecutive days practiced
    public int TotalLessonsCompleted { get; set; }
    public int TotalTestsCompleted { get; set; }
    public int TotalAchievementsUnlocked { get; set; }
    public int TotalPracticeTimeMinutes { get; set; }

    /// <summary>
    /// Calculates XP required for next level
    /// Formula: 100 * N + 50 * N^2
    /// </summary>
    public int GetXPForNextLevel()
    {
        int nextLevel = CurrentLevel + 1;
        return 100 * nextLevel + 50 * nextLevel * nextLevel;
    }

    /// <summary>
    /// Calculates XP required for current level
    /// </summary>
    public int GetXPForCurrentLevel()
    {
        if (CurrentLevel == 1) return 0;
        return 100 * CurrentLevel + 50 * CurrentLevel * CurrentLevel;
    }

    /// <summary>
    /// Gets progress percentage to next level (0-100)
    /// </summary>
    public double GetLevelProgress()
    {
        int currentLevelXP = GetXPForCurrentLevel();
        int nextLevelXP = GetXPForNextLevel();
        int xpInCurrentLevel = TotalXP - currentLevelXP;
        int xpNeededForLevel = nextLevelXP - currentLevelXP;
        
        if (xpNeededForLevel == 0) return 100;
        return Math.Min(100, (xpInCurrentLevel / (double)xpNeededForLevel) * 100);
    }

    /// <summary>
    /// Gets formatted level progress string (e.g., "450/500 XP")
    /// </summary>
    public string GetLevelProgressString()
    {
        int currentLevelXP = GetXPForCurrentLevel();
        int nextLevelXP = GetXPForNextLevel();
        int xpInCurrentLevel = TotalXP - currentLevelXP;
        int xpNeededForLevel = nextLevelXP - currentLevelXP;
        
        return $"{xpInCurrentLevel}/{xpNeededForLevel} XP";
    }
}
