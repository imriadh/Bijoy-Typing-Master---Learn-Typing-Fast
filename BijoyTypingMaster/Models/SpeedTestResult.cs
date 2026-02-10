namespace BijoyTypingMaster.Models;

/// <summary>
/// Speed test result with detailed metrics
/// </summary>
public class SpeedTestResult
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Duration { get; set; } // seconds
    public double WPM { get; set; }
    public double NetWPM { get; set; } // WPM adjusted for errors
    public double Accuracy { get; set; }
    public int TotalCharacters { get; set; }
    public int CorrectCharacters { get; set; }
    public int ErrorCount { get; set; }
    public string TestType { get; set; } = "Speed Test"; // Speed Test, Custom, Challenge
    
    public SpeedTestResult()
    {
        Date = DateTime.Now;
    }
    
    /// <summary>
    /// Get performance rating based on WPM and accuracy
    /// </summary>
    public string GetRating()
    {
        if (Accuracy < 80) return "Needs Practice";
        if (WPM < 20) return "Beginner";
        if (WPM < 40) return "Intermediate";
        if (WPM < 60) return "Advanced";
        if (WPM < 80) return "Expert";
        return "Master";
    }
    
    /// <summary>
    /// Get star rating (1-5 stars)
    /// </summary>
    public int GetStars()
    {
        double score = (WPM * Accuracy) / 100.0;
        if (score < 20) return 1;
        if (score < 40) return 2;
        if (score < 60) return 3;
        if (score < 80) return 4;
        return 5;
    }
}
