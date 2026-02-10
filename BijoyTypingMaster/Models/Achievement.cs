namespace BijoyTypingMaster.Models;

/// <summary>
/// Represents an unlockable achievement/badge
/// </summary>
public class Achievement
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty; // Emoji icon
    public AchievementCategory Category { get; set; }
    public AchievementTier Tier { get; set; }
    public RequirementType RequirementType { get; set; }
    public int RequirementValue { get; set; }
    public int XPReward { get; set; }
    public bool IsUnlocked { get; set; }
    public DateTime? UnlockedAt { get; set; }
    public int Progress { get; set; } // Current progress towards requirement
    public int MaxProgress => RequirementValue; // Max progress (same as requirement)

    /// <summary>
    /// Gets progress percentage (0-100)
    /// </summary>
    public int GetProgressPercentage()
    {
        if (MaxProgress == 0) return 0;
        return Math.Min(100, (int)((Progress / (double)MaxProgress) * 100));
    }

    /// <summary>
    /// Gets color based on tier
    /// </summary>
    public string GetTierColor()
    {
        return Tier switch
        {
            AchievementTier.Bronze => "#cd7f32",
            AchievementTier.Silver => "#c0c0c0",
            AchievementTier.Gold => "#ffd700",
            AchievementTier.Platinum => "#e5e4e2",
            _ => "#6b7280"
        };
    }
}

/// <summary>
/// Achievement categories
/// </summary>
public enum AchievementCategory
{
    Speed,      // WPM-related
    Accuracy,   // Accuracy-related
    Consistency,// Streak/daily practice
    Practice,   // Lessons/tests completed
    Mastery,    // Overall skill level
    Special     // Unique achievements
}

/// <summary>
/// Achievement tiers (difficulty/rarity)
/// </summary>
public enum AchievementTier
{
    Bronze,
    Silver,
    Gold,
    Platinum
}

/// <summary>
/// Types of requirements for achievements
/// </summary>
public enum RequirementType
{
    SingleWPM,          // Achieve X WPM in a single session
    AverageWPM,         // Maintain X average WPM
    SingleAccuracy,     // Achieve X% accuracy in a single session
    AverageAccuracy,    // Maintain X% average accuracy
    LessonsCompleted,   // Complete X lessons
    TestsCompleted,     // Complete X speed tests
    DaysStreak,         // Practice for X consecutive days
    TotalPracticeTime,  // Total practice time in minutes
    TotalXP,            // Reach X total XP
    LevelReached,       // Reach level X
    ChallengesCompleted,// Complete X daily challenges
    PerfectTests        // Complete X tests with 100% accuracy
}
