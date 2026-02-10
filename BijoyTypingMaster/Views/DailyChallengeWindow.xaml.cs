using BijoyTypingMaster.Models;
using BijoyTypingMaster.Services;
using System.Collections.ObjectModel;

namespace BijoyTypingMaster.Views;

public partial class DailyChallengeWindow : ContentPage
{
    private readonly DailyChallengeManager _challengeManager;
    private readonly XPManager _xpManager;
    private DailyChallenge? _currentChallenge;
    private ObservableCollection<DailyChallenge> _history = new();

    public DailyChallengeWindow(DailyChallengeManager challengeManager, XPManager xpManager)
    {
        InitializeComponent();
        _challengeManager = challengeManager;
        _xpManager = xpManager;

        HistoryCollectionView.ItemsSource = _history;

        LoadChallenge();
    }

    private async void LoadChallenge()
    {
        try
        {
            // Get today's challenge
            _currentChallenge = await _challengeManager.GetTodayChallengeAsync();

            // Update UI
            DateLabel.Text = _currentChallenge.Date.ToString("MMMM dd, yyyy");
            ChallengeTypeLabel.Text = _currentChallenge.ChallengeType.ToString().Replace("Challenge", " Challenge");
            DescriptionLabel.Text = _currentChallenge.GetDescription();
            ChallengeTextLabel.Text = _currentChallenge.TargetText;
            
            // Targets
            TargetWPMLabel.Text = $"{_currentChallenge.TargetWPM} WPM";
            TargetAccuracyLabel.Text = $"{_currentChallenge.TargetAccuracy}%";
            TimeLimitLabel.Text = $"{_currentChallenge.TimeLimit}s";

            // Difficulty badge
            string difficulty = _currentChallenge.GetDifficultyLevel();
            DifficultyLabel.Text = difficulty;
            DifficultyBadge.BackgroundColor = difficulty switch
            {
                "Easy" => Color.FromArgb("#10b981"),
                "Medium" => Color.FromArgb("#fbbf24"),
                "Hard" => Color.FromArgb("#ef4444"),
                _ => Color.FromArgb("#6b7280")
            };

            // Reward range
            RewardLabel.Text = $"{(_currentChallenge.ChallengeType == ChallengeType.ComboChallenge ? "200" : "100")}-{(_currentChallenge.ChallengeType == ChallengeType.EnduranceChallenge ? "200" : "150")} XP";

            // Check if completed
            if (_currentChallenge.IsCompleted)
            {
                ShowCompletedState();
            }

            // Load streak
            var streak = await _challengeManager.GetChallengeStreakAsync();
            StreakLabel.Text = $"Current Streak: {streak} day{(streak != 1 ? "s" : "")}";

            // Load history
            var history = await _challengeManager.GetChallengeHistoryAsync(30);
            _history.Clear();
            foreach (var challenge in history.Take(10))
            {
                _history.Add(challenge);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load challenge: {ex.Message}", "OK");
        }
    }

    private void ShowCompletedState()
    {
        if (_currentChallenge == null) return;

        // Hide start section
        StartSection.IsVisible = false;

        // Show completed section
        CompletedSection.IsVisible = true;

        // Fill in results
        AchievedWPMLabel.Text = _currentChallenge.AchievedWPM.ToString();
        AchievedAccuracyLabel.Text = $"{_currentChallenge.AchievedAccuracy}%";
        XPEarnedLabel.Text = $"+{_currentChallenge.XPEarned} XP";

        // Completion message
        bool requirementMet = _currentChallenge.IsRequirementMet();
        if (requirementMet)
        {
            CompletionMessageLabel.Text = "🎉 Excellent work! You've met the challenge requirements!";
        }
        else
        {
            CompletionMessageLabel.Text = "👏 Good effort! Try again tomorrow to beat the challenge!";
        }
    }

    private async void OnStartChallengeClicked(object sender, EventArgs e)
    {
        if (_currentChallenge == null) return;

        try
        {
            // Navigate to typing practice with challenge text
            var challengePracticeWindow = new ChallengePracticeWindow(
                _currentChallenge, 
                _challengeManager, 
                _xpManager
            );

            await Navigation.PushAsync(challengePracticeWindow);

            // Refresh when returning
            challengePracticeWindow.Disappearing += async (s, args) =>
            {
                LoadChallenge();
                
                // Reload challenge after completion
                _currentChallenge = await _challengeManager.GetTodayChallengeAsync();
            };
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to start challenge: {ex.Message}", "OK");
        }
    }
}
