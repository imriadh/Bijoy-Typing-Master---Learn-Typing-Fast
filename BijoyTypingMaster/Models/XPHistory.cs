namespace BijoyTypingMaster.Models;

/// <summary>
/// Represents a single XP transaction/award
/// </summary>
public class XPHistory
{
    public int Id { get; set; }
    public int UserId { get; set; } = 1;
    public DateTime Date { get; set; }
    public int Amount { get; set; }
    public string Source { get; set; } = string.Empty; // e.g., "Lesson", "Challenge", "Achievement"
    public string Description { get; set; } = string.Empty; // e.g., "Completed Lesson 5"
}

/// <summary>
/// XP source constants
/// </summary>
public static class XPSource
{
    public const string LessonComplete = "Lesson";
    public const string SpeedTest = "Speed Test";
    public const string DailyChallenge = "Daily Challenge";
    public const string Achievement = "Achievement";
    public const string MiniGame = "Mini Game";
    public const string CustomPractice = "Custom Practice";
    public const string StreakBonus = "Streak Bonus";
}

/// <summary>
/// XP reward amounts
/// </summary>
public static class XPRewards
{
    public const int LessonComplete = 25;
    public const int SpeedTest = 30;
    public const int DailyChallengeBase = 50;
    public const int CustomPractice = 15;
    public const int MiniGameBase = 10;
    public const int StreakBonus7Days = 100;
    public const int StreakBonus30Days = 500;
    public const int StreakBonusPerDay = 10; // Additional XP per consecutive day
}
