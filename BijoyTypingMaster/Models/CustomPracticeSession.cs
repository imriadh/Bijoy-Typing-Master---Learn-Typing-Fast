namespace BijoyTypingMaster.Models;

/// <summary>
/// Represents a custom text practice session
/// </summary>
public class CustomPracticeSession
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CustomText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastPracticed { get; set; }
    public int TimesCompleted { get; set; }
    public int BestWPM { get; set; }
    public int BestAccuracy { get; set; }

    /// <summary>
    /// Gets word count
    /// </summary>
    public int GetWordCount()
    {
        if (string.IsNullOrWhiteSpace(CustomText)) return 0;
        return CustomText.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    /// <summary>
    /// Gets character count
    /// </summary>
    public int GetCharacterCount()
    {
        return CustomText?.Length ?? 0;
    }

    /// <summary>
    /// Gets formatted stats string
    /// </summary>
    public string GetStatsString()
    {
        return TimesCompleted > 0
            ? $"Best: {BestWPM} WPM | {BestAccuracy}% | Completed {TimesCompleted}×"
            : "Not practiced yet";
    }
}
