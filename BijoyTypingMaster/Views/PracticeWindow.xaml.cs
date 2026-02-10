using BijoyTypingMaster.Models;
using BijoyTypingMaster.Services;

namespace BijoyTypingMaster.Views;

public partial class PracticeWindow : ContentPage
{
    private readonly TypingEngine _typingEngine;
    private readonly DatabaseManager _dbManager;
    private readonly SettingsManager? _settingsManager;
    private readonly string _layoutType;
    private Lesson? _currentLesson;
    private bool _isSessionActive = false;

    // Constructor for dependency injection
    public PracticeWindow(string layoutType, DatabaseManager dbManager, SettingsManager settingsManager)
    {
        InitializeComponent();
        
        _layoutType = layoutType;
        _typingEngine = new TypingEngine();
        _dbManager = dbManager;
        _settingsManager = settingsManager;
        
        LayoutLabel.Text = layoutType;
        _typingEngine.SetLayout(layoutType);
        
        // Load finger guide setting
        if (_settingsManager != null)
        {
            FingerGuide.IsVisible = _settingsManager.CurrentSettings.ShowFingerGuide;
        }
        
        LoadRandomLesson();
    }

    // Legacy constructor for backward compatibility
    public PracticeWindow(string layoutType)
    {
        InitializeComponent();
        
        _layoutType = layoutType;
        _typingEngine = new TypingEngine();
        _dbManager = new DatabaseManager();
        
        LayoutLabel.Text = layoutType;
        _typingEngine.SetLayout(layoutType);
        
        LoadRandomLesson();
    }

    private void LoadRandomLesson()
    {
        var lessons = _dbManager.GetLessonsByType(_layoutType);
        
        if (lessons.Count > 0)
        {
            var random = new Random();
            _currentLesson = lessons[random.Next(lessons.Count)];
            TargetTextLabel.Text = _currentLesson.TextContent;
            
            // Update title
            Title = $"Practice: {_currentLesson.Title}";
        }
        else
        {
            TargetTextLabel.Text = "No lessons available for this layout.";
        }
    }

    private void OnStartClicked(object sender, EventArgs e)
    {
        if (_currentLesson == null) return;

        _isSessionActive = true;
        _typingEngine.StartSession(_currentLesson.TextContent);
        
        // Enable/disable buttons
        StartButton.IsEnabled = false;
        ResetButton.IsEnabled = true;
        FinishButton.IsEnabled = true;
        
        // Focus on hidden entry to capture keyboard
        HiddenEntry.Focus();
        
        // Clear display
        UserInputLabel.Text = "";
        UpdateStats();
        
        // Initialize finger guide with first character
        UpdateFingerGuide();
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        _isSessionActive = false;
        _typingEngine.Reset();
        
        UserInputLabel.Text = "";
        HiddenEntry.Text = "";
        
        // Reset buttons
        StartButton.IsEnabled = true;
        ResetButton.IsEnabled = false;
        FinishButton.IsEnabled = false;
        
        // Clear finger guide
        FingerGuide.ClearHighlight();
        
        UpdateStats();
    }

    private async void OnFinishClicked(object sender, EventArgs e)
    {
        if (!_isSessionActive) return;

        var (wpm, accuracy) = _typingEngine.EndSession();
        _isSessionActive = false;

        // Save to database
        if (_currentLesson != null)
        {
            var progress = new UserProgress
            {
                Date = DateTime.Now,
                WPM = wpm,
                Accuracy = accuracy,
                LessonId = _currentLesson.Id
            };

            _dbManager.SaveProgress(progress);
        }

        // Show results
        await DisplayAlert("Session Complete!", 
            $"WPM: {wpm:F2}\nAccuracy: {accuracy:F2}%\n\nGreat job!", 
            "OK");

        // Reset for next session
        OnResetClicked(sender, e);
        LoadRandomLesson();
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isSessionActive) return;

        // Get the last character typed
        if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.Length > e.OldTextValue.Length)
        {
            string newChar = e.NewTextValue.Substring(e.OldTextValue.Length);
            
            // Process through typing engine
            string processedChar = _typingEngine.ProcessKeyPress(newChar);
            
            // Update display
            UserInputLabel.Text = _typingEngine.CurrentInput;
            UpdateStats();
            UpdateTargetTextDisplay();
            
            // Update finger guide for next character
            UpdateFingerGuide();
            
            // Check if session complete
            if (_typingEngine.IsSessionComplete())
            {
                FingerGuide.ClearHighlight();
                OnFinishClicked(sender, e);
            }
        }
        else if (e.NewTextValue.Length < e.OldTextValue.Length)
        {
            // Handle backspace
            _typingEngine.ProcessKeyPress("Backspace");
            UserInputLabel.Text = _typingEngine.CurrentInput;
            UpdateStats();
            UpdateTargetTextDisplay();
            UpdateFingerGuide();
        }
    }

    private void UpdateFingerGuide()
    {
        if (_currentLesson == null || !FingerGuide.IsVisible) return;

        // Get the next expected character
        int currentPosition = _typingEngine.CurrentInput.Length;
        if (currentPosition < _currentLesson.TextContent.Length)
        {
            string nextChar = _currentLesson.TextContent[currentPosition].ToString();
            FingerGuide.HighlightKey(nextChar);
        }
        else
        {
            FingerGuide.ClearHighlight();
        }
    }

    private void UpdateStats()
    {
        WpmLabel.Text = _typingEngine.WPM.ToString("F2");
        AccuracyLabel.Text = _typingEngine.Accuracy.ToString("F2") + "%";
        ProgressLabel.Text = _typingEngine.GetProgress().ToString("F0") + "%";
    }

    private void UpdateTargetTextDisplay()
    {
        // This is a simplified version - in a real app, you'd use FormattedString
        // to color individual characters
        
        // For now, just keep it simple
        if (_currentLesson != null)
        {
            TargetTextLabel.Text = _currentLesson.TextContent;
        }
    }

    private void OnTextTapped(object sender, EventArgs e)
    {
        // Focus on hidden entry when user taps the text area
        HiddenEntry.Focus();
    }

    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        // Entry is focused - ready to capture input
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        // Optionally re-focus if session is active
        if (_isSessionActive)
        {
            HiddenEntry.Focus();
        }
    }
}
