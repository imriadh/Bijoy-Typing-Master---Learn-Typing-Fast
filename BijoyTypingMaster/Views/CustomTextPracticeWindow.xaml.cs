using BijoyTypingMaster.Models;
using BijoyTypingMaster.Services;
using System.Diagnostics;

namespace BijoyTypingMaster.Views;

public partial class CustomTextPracticeWindow : ContentPage
{
    private readonly CustomPracticeSession _session;
    private readonly CustomTextManager _customTextManager;
    private readonly XPManager _xpManager;
    
    private bool _isRunning = false;
    private Stopwatch _stopwatch = new();
    private System.Timers.Timer _timer = new();
    private int _totalCharsTyped = 0;
    private int _correctChars = 0;
    private int _errorCount = 0;

    public CustomTextPracticeWindow(
        CustomPracticeSession session,
        CustomTextManager customTextManager,
        XPManager xpManager)
    {
        InitializeComponent();
        
        _session = session;
        _customTextManager = customTextManager;
        _xpManager = xpManager;

        InitializeUI();
        SetupTimer();
    }

    private void InitializeUI()
    {
        TitleLabel.Text = $"Practice: {_session.Title}";
        TargetTextLabel.Text = _session.CustomText;
    }

    private void SetupTimer()
    {
        _timer.Interval = 100; // Update every 100ms
        _timer.Elapsed += (s, e) => UpdateTimer();
    }

    private void OnStartClicked(object sender, EventArgs e)
    {
        _isRunning = true;
        _stopwatch.Start();
        _timer.Start();

        StartButton.IsEnabled = false;
        ResetButton.IsEnabled = true;
        UserInputEntry.IsEnabled = true;
        UserInputEntry.Focus();
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        StopPractice();
        ResetPractice();
    }

    private void StopPractice()
    {
        _isRunning = false;
        _stopwatch.Stop();
        _timer.Stop();
        UserInputEntry.IsEnabled = false;
    }

    private void ResetPractice()
    {
        _stopwatch.Reset();
        _totalCharsTyped = 0;
        _correctChars = 0;
        _errorCount = 0;

        UserInputEntry.Text = string.Empty;
        TimerLabel.Text = "00:00";
        WPMLabel.Text = "0";
        AccuracyLabel.Text = "100%";
        ProgressBar.Progress = 0;
        ProgressLabel.Text = "0% Complete";

        StartButton.IsEnabled = true;
        ResetButton.IsEnabled = false;
    }

    private void UpdateTimer()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var elapsed = _stopwatch.Elapsed;
            TimerLabel.Text = $"{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
        });
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isRunning) return;

        string userText = e.NewTextValue ?? string.Empty;
        string targetText = _session.CustomText;

        _totalCharsTyped = userText.Length;
        _correctChars = 0;
        _errorCount = 0;

        // Calculate correct characters
        for (int i = 0; i < Math.Min(userText.Length, targetText.Length); i++)
        {
            if (userText[i] == targetText[i])
            {
                _correctChars++;
            }
            else
            {
                _errorCount++;
            }
        }

        UpdateStats();

        // Check if completed
        if (userText == targetText)
        {
            CompletePractice();
        }
    }

    private void UpdateStats()
    {
        // Calculate WPM
        double minutes = _stopwatch.Elapsed.TotalMinutes;
        double words = _correctChars / 5.0;
        int wpm = minutes > 0 ? (int)(words / minutes) : 0;
        WPMLabel.Text = wpm.ToString();

        // Calculate Accuracy
        int accuracy = _totalCharsTyped > 0 
            ? (int)((_correctChars / (double)_totalCharsTyped) * 100) 
            : 100;
        AccuracyLabel.Text = $"{accuracy}%";

        // Update color
        AccuracyLabel.TextColor = accuracy >= 95 
            ? Color.FromArgb("#10b981") 
            : accuracy >= 90 
                ? Color.FromArgb("#fbbf24") 
                : Color.FromArgb("#ef4444");

        // Update progress
        double progress = _totalCharsTyped / (double)_session.CustomText.Length;
        ProgressBar.Progress = progress;
        ProgressLabel.Text = $"{(progress * 100):F0}% Complete";
    }

    private async void CompletePractice()
    {
        StopPractice();

        // Calculate final stats
        double minutes = _stopwatch.Elapsed.TotalMinutes;
        double words = _correctChars / 5.0;
        int finalWPM = minutes > 0 ? (int)(words / minutes) : 0;
        int finalAccuracy = _totalCharsTyped > 0 
            ? (int)((_correctChars / (double)_totalCharsTyped) * 100) 
            : 0;

        // Update database stats
        await _customTextManager.UpdateStatsAsync(_session.Id, finalWPM, finalAccuracy);

        // Award XP
        var (newLevel, leveledUp) = await _xpManager.AwardCustomPracticeXPAsync(_session.Title);

        // Show completion message
        string message = $"✅ Practice Complete!\n\n" +
                        $"Time: {_stopwatch.Elapsed.Minutes:D2}:{_stopwatch.Elapsed.Seconds:D2}\n" +
                        $"WPM: {finalWPM}\n" +
                        $"Accuracy: {finalAccuracy}%\n\n" +
                        $"⭐ You earned {XPRewards.CustomPractice} XP!";

        if (leveledUp)
        {
            message += $"\n\n🎊 LEVEL UP! You're now Level {newLevel}!";
        }

        bool retry = await DisplayAlert(
            "Practice Complete!", 
            message, 
            "Practice Again", 
            "Exit"
        );

        if (retry)
        {
            ResetPractice();
        }
        else
        {
            await Navigation.PopAsync();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer?.Dispose();
    }
}
