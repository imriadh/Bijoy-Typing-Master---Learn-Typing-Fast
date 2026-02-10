namespace BijoyTypingMaster.Models;

/// <summary>
/// Application settings and preferences
/// </summary>
public class AppSettings
{
    public int FontSize { get; set; } = 32;
    public string Theme { get; set; } = "Light"; // Light, Dark
    public bool SoundEnabled { get; set; } = true;
    public bool ShowFingerGuide { get; set; } = true;
    public bool ShowVirtualKeyboard { get; set; } = true;
    public string PreferredLayout { get; set; } = "Bijoy";
    public int SpeedTestDuration { get; set; } = 60; // seconds
    public string UserName { get; set; } = "User";
    
    // Display preferences
    public bool ShowWpmInRealTime { get; set; } = true;
    public bool ShowAccuracyInRealTime { get; set; } = true;
    public bool HighlightErrors { get; set; } = true;
    
    // Practice preferences
    public bool AutoAdvanceLesson { get; set; } = false;
    public int MinAccuracyToPass { get; set; } = 85; // percentage
    public int MinWpmToPass { get; set; } = 20;
}
