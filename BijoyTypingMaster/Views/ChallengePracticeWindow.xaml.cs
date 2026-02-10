using BijoyTypingMaster.Models;
using BijoyTypingMaster.Services;
using System.Diagnostics;

namespace BijoyTypingMaster.Views;

public partial class ChallengePracticeWindow : ContentPage
{
    private readonly DailyChallenge _challenge;
    private readonly DailyChallengeManager _challengeManager;
    private readonly XPManager _xpManager;
    
    private bool _isRunning = false;
    private Stopwatch _stopwatch = new();
    private System.Timers.Timer _timer = new();
    private int _timeRemaining;
    private int _totalCharsTyped = 0;
    private int _correctChars = 0;
    private int _errorCount = 0;

    public ChallengePracticeWindow(
        DailyChallenge challenge, 
        DailyChallengeManager challengeManager,
        XPManager xpManager)
    {
        InitializeComponent();
        
        _challenge = challenge;
        _challengeManager = challengeManager;
        _xpManager = xpManager;
        _timeRemaining = challenge.TimeLimit;

        InitializeUI();
        SetupTimer();
    }

    private void InitializeUI()
    {
        TargetTextLabel.Text = _challenge.TargetText;
        TimerLabel.Text = _timeRemaining.ToString();
        
        TargetDisplayLabel.Text = _challenge.ChallengeType switch
        {
            ChallengeType.SpeedChallenge => $"{_challenge.TargetWPM} WPM",
            ChallengeType.AccuracyChallenge => $"{_challenge.TargetAccuracy}% Accuracy",
            ChallengeType.ComboChallenge => $"{_challenge.TargetWPM} WPM | {_challenge.TargetAccuracy}% Accuracy",
            ChallengeType.EnduranceChallenge => $"{_challenge.TimeLimit / 60} minutes",
            _ => ""
        };
    }

    private void SetupTimer()
    {
        _timer.Interval = 1000; // 1 second
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
        StopChallenge();
        ResetChallenge();
    }

    private void StopChallenge()
    {
        _isRunning = false;
        _stopwatch.Stop();
        _timer.Stop();
        UserInputEntry.IsEnabled = false;
    }

    private void ResetChallenge()
    {
        _stopwatch.Reset();
        _timeRemaining = _challenge.TimeLimit;
        _totalCharsTyped = 0;
        _correctChars = 0;
        _errorCount = 0;

        UserInputEntry.Text = string.Empty;
        TimerLabel.Text = _timeRemaining.ToString();
        WPMLabel.Text = "0";
        AccuracyLabel.Text = "100%";
        ProgressLabel.Text = "0%";
        FeedbackLabel.IsVisible = false;

        StartButton.IsEnabled = true;
        ResetButton.IsEnabled = false;
    }

    private void UpdateTimer()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _timeRemaining--;
            TimerLabel.Text = _timeRemaining.ToString();

            if (_timeRemaining <= 10)
            {
                TimerLabel.TextColor = Color.FromArgb("#ef4444"); // Red warning
            }

            if (_timeRemaining <= 0)
            {
                CompleteChallenge();
            }
        });
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isRunning) return;

        string userText = e.NewTextValue ?? string.Empty;
        string targetText = _challenge.TargetText;

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
        if (userText.Length >= targetText.Length)
        {
            CompleteChallenge();
        }
    }

    private void UpdateStats()
    {
        // Calculate WPM
        double minutes = _stopwatch.Elapsed.TotalMinutes;
        double words = _correctChars / 5.0; // Standard: 5 chars = 1 word
        int wpm = minutes > 0 ? (int)(words / minutes) : 0;
        WPMLabel.Text = wpm.ToString();

        // Calculate Accuracy
        int accuracy = _totalCharsTyped > 0 
            ? (int)((_correctChars / (double)_totalCharsTyped) * 100) 
            : 100;
        AccuracyLabel.Text = $"{accuracy}%";

        // Update accuracy color
        AccuracyLabel.TextColor = accuracy >= 95 
            ? Color.FromArgb("#10b981") 
            : accuracy >= 90 
                ? Color.FromArgb("#fbbf24") 
                : Color.FromArgb("#ef4444");

        // Calculate Progress
        int progress = (int)((_totalCharsTyped / (double)_challenge.TargetText.Length) * 100);
        progress = Math.Min(100, progress);
        ProgressLabel.Text = $"{progress}%";
    }

    private async void CompleteChallenge()
    {
        StopChallenge();

        // Calculate final stats
        double minutes = _stopwatch.Elapsed.TotalMinutes;
        double words = _correctChars / 5.0;
        int finalWPM = minutes > 0 ? (int)(words / minutes) : 0;
        int finalAccuracy = _totalCharsTyped > 0 
            ? (int)((_correctChars / (double)_totalCharsTyped) * 100) 
            : 0;

        // Complete challenge in manager
        var (success, xpEarned) = await _challengeManager.CompleteChallengeAsync(finalWPM, finalAccuracy);

        // Award XP through XP manager (for additional tracking)
        if (success && xpEarned > 0)
        {
            var (newLevel, leveledUp) = await _xpManager.AwardXPAsync(
                xpEarned, 
                XPSource.DailyChallenge, 
                $"Completed {_challenge.ChallengeType}"
            );

            // Show completion message
            string message = success
                ? $"🎉 Challenge Complete!\n\nWPM: {finalWPM}\nAccuracy: {finalAccuracy}%\n\n✨ You earned {xpEarned} XP!"
                : $"👏 Good effort!\n\nWPM: {finalWPM}\nAccuracy: {finalAccuracy}%\n\nYou didn't meet the target this time, but keep practicing!";

            if (leveledUp)
            {
                message += $"\n\n🎊 LEVEL UP! You're now Level {newLevel}!";
            }

            await DisplayAlert(
                success ? "Success!" : "Challenge Attempt", 
                message, 
                "OK"
            );

            // Return to previous page
            await Navigation.PopAsync();
        }
        else
        {
            // Show retry option
            bool retry = await DisplayAlert(
                "Time's Up!", 
                $"Final WPM: {finalWPM}\nAccuracy: {finalAccuracy}%\n\nWould you like to try again?",
                "Retry", 
                "Exit"
            );

            if (retry)
            {
                ResetChallenge();
            }
            else
            {
                await Navigation.PopAsync();
            }
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer?.Dispose();
    }
}
