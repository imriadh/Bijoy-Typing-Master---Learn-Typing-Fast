namespace BijoyTypingMaster.Models;

/// <summary>
/// Categories for structured lesson progression
/// </summary>
public enum LessonCategory
{
    HomeRow,        // Basic home row keys
    TopRow,         // Top row keys
    BottomRow,      // Bottom row keys
    Numbers,        // Number keys
    Punctuation,    // Punctuation marks
    Juktakkhor,     // Bengali conjuncts
    CommonWords,    // Frequently used words
    Phrases,        // Short phrases
    Sentences,      // Complete sentences
    Paragraphs      // Full paragraphs
}

/// <summary>
/// Extended lesson model with category and progression
/// </summary>
public class LessonInfo
{
    public int LessonNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public LessonCategory Category { get; set; }
    public string Difficulty { get; set; } = "Beginner";
    public string TextContent { get; set; } = string.Empty;
    public string Type { get; set; } = "Bijoy";
    public int EstimatedMinutes { get; set; } = 5;
    public string FocusKeys { get; set; } = string.Empty; // Keys to focus on
}
