using BijoyTypingMaster.Models;
using BijoyTypingMaster.Services;
using System.Collections.ObjectModel;

namespace BijoyTypingMaster.Views;

public partial class AchievementsWindow : ContentPage
{
    private readonly AchievementManager _achievementManager;
    private readonly XPManager _xpManager;
    private ObservableCollection<AchievementViewModel> _achievements = new();
    private List<Achievement> _allAchievements = new();
    private AchievementCategory? _currentFilter = null;

    public AchievementsWindow(AchievementManager achievementManager, XPManager xpManager)
    {
        InitializeComponent();
        _achievementManager = achievementManager;
        _xpManager = xpManager;

        AchievementsCollectionView.ItemsSource = _achievements;

        LoadAchievements();
    }

    private async void LoadAchievements()
    {
        try
        {
            _allAchievements = await _achievementManager.GetAllAchievementsWithStatusAsync();

            // Update header stats
            int unlocked = _allAchievements.Count(a => a.IsUnlocked);
            int total = _allAchievements.Count;
            TotalAchievementsLabel.Text = $"{unlocked}/{total}";

            int totalXP = _allAchievements.Where(a => a.IsUnlocked).Sum(a => a.XPReward);
            TotalXPLabel.Text = $"{totalXP:N0} XP";

            int completion = total > 0 ? (unlocked * 100 / total) : 0;
            CompletionLabel.Text = $"{completion}%";

            // Display achievements
            ApplyFilter(_currentFilter);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load achievements: {ex.Message}", "OK");
        }
    }

    private void OnFilterClicked(object sender, EventArgs e)
    {
        if (sender is not Button button) return;

        // Reset all button colors
        AllFilterBtn.BackgroundColor = Color.FromArgb("#334155");
        SpeedFilterBtn.BackgroundColor = Color.FromArgb("#334155");
        AccuracyFilterBtn.BackgroundColor = Color.FromArgb("#334155");
        PracticeFilterBtn.BackgroundColor = Color.FromArgb("#334155");

        // Set active button
        button.BackgroundColor = Color.FromArgb("#6366f1");

        // Apply filter
        _currentFilter = button.StyleId switch
        {
            "Speed" => AchievementCategory.Speed,
            "Accuracy" => AchievementCategory.Accuracy,
            "Practice" => AchievementCategory.Practice,
            _ => null
        };

        ApplyFilter(_currentFilter);
    }

    private void ApplyFilter(AchievementCategory? category)
    {
        _achievements.Clear();

        var filtered = category == null
            ? _allAchievements
            : _allAchievements.Where(a => a.Category == category).ToList();

        foreach (var achievement in filtered)
        {
            _achievements.Add(new AchievementViewModel(achievement));
        }
    }
}

// ViewModel for data binding
public class AchievementViewModel
{
    private readonly Achievement _achievement;

    public AchievementViewModel(Achievement achievement)
    {
        _achievement = achievement;
    }

    public string Icon => _achievement.Icon;
    public string Name => _achievement.Name;
    public string Description => _achievement.Description;
    public string Tier => _achievement.Tier.ToString();
    public string TierColor => _achievement.GetTierColor();
    public bool IsUnlocked => _achievement.IsUnlocked;
    public int ProgressPercentage => _achievement.GetProgressPercentage();
    public string ProgressText => $"{_achievement.Progress}/{_achievement.MaxProgress}";
    public string XPRewardText => $"⭐ {_achievement.XPReward} XP";
    public string UnlockDateText => _achievement.UnlockedAt?.ToString("MMM dd, yyyy") ?? "";
}
