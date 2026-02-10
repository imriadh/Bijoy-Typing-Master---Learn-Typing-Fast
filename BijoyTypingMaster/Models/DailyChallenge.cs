namespace BijoyTypingMaster.Models;

/// <summary>
/// Represents a daily typing challenge
/// </summary>
public class DailyChallenge
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public ChallengeType ChallengeType { get; set; }
    public string TargetText { get; set; } = string.Empty;
    public int TargetWPM { get; set; }
    public int TargetAccuracy { get; set; }
    public int TimeLimit { get; set; } // in seconds
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int AchievedWPM { get; set; }
    public int AchievedAccuracy { get; set; }
    public int XPEarned { get; set; }

    /// <summary>
    /// Gets challenge description
    /// </summary>
    public string GetDescription()
    {
        return ChallengeType switch
        {
            ChallengeType.SpeedChallenge => $"Type at {TargetWPM} WPM or faster",
            ChallengeType.AccuracyChallenge => $"Maintain {TargetAccuracy}% accuracy or higher",
            ChallengeType.ComboChallenge => $"Achieve {TargetWPM} WPM AND {TargetAccuracy}% accuracy",
            ChallengeType.EnduranceChallenge => $"Type for {TimeLimit / 60} minutes without stopping",
            _ => "Complete the typing challenge"
        };
    }

    /// <summary>
    /// Gets difficulty level
    /// </summary>
    public string GetDifficultyLevel()
    {
        return ChallengeType switch
        {
            ChallengeType.SpeedChallenge => TargetWPM >= 60 ? "Hard" : TargetWPM >= 40 ? "Medium" : "Easy",
            ChallengeType.AccuracyChallenge => TargetAccuracy >= 98 ? "Hard" : TargetAccuracy >= 95 ? "Medium" : "Easy",
            ChallengeType.ComboChallenge => "Hard",
            ChallengeType.EnduranceChallenge => TimeLimit >= 180 ? "Hard" : "Medium",
            _ => "Normal"
        };
    }

    /// <summary>
    /// Calculates XP reward based on performance
    /// </summary>
    public int CalculateXPReward()
    {
        int baseXP = 100;
        int bonus = 0;

        switch (ChallengeType)
        {
            case ChallengeType.SpeedChallenge:
                if (AchievedWPM >= TargetWPM)
                {
                    bonus = (AchievedWPM - TargetWPM) * 2; // 2 XP per WPM over target
                }
                break;

            case ChallengeType.AccuracyChallenge:
                if (AchievedAccuracy >= TargetAccuracy)
                {
                    bonus = (AchievedAccuracy - TargetAccuracy) * 10; // 10 XP per % over target
                }
                break;

            case ChallengeType.ComboChallenge:
                if (AchievedWPM >= TargetWPM && AchievedAccuracy >= TargetAccuracy)
                {
                    baseXP = 200; // Higher base for combo
                    bonus = 50; // Perfect completion bonus
                }
                break;

            case ChallengeType.EnduranceChallenge:
                baseXP = 150; // Higher base for endurance
                bonus = AchievedAccuracy - 90; // Bonus for high accuracy in long session
                break;
        }

        return baseXP + Math.Max(0, bonus);
    }

    /// <summary>
    /// Checks if challenge requirements are met
    /// </summary>
    public bool IsRequirementMet()
    {
        return ChallengeType switch
        {
            ChallengeType.SpeedChallenge => AchievedWPM >= TargetWPM,
            ChallengeType.AccuracyChallenge => AchievedAccuracy >= TargetAccuracy,
            ChallengeType.ComboChallenge => AchievedWPM >= TargetWPM && AchievedAccuracy >= TargetAccuracy,
            ChallengeType.EnduranceChallenge => IsCompleted, // Just need to complete time
            _ => false
        };
    }
}

/// <summary>
/// Types of daily challenges
/// </summary>
public enum ChallengeType
{
    SpeedChallenge,      // Reach target WPM
    AccuracyChallenge,   // Maintain target accuracy
    ComboChallenge,      // Achieve both WPM and accuracy targets
    EnduranceChallenge   // Type for extended duration
}
