using BijoyTypingMaster.Models;

namespace BijoyTypingMaster.Controls;

public partial class XPBar : ContentView
{
    private double _maxProgressWidth = 0;

    public XPBar()
    {
        InitializeComponent();
        
        // Get max width for progress bar after layout
        this.SizeChanged += (s, e) =>
        {
            if (_maxProgressWidth == 0 && this.Width > 0)
            {
                // Calculate available width for progress bar
                _maxProgressWidth = this.Width - 200; // Approximate width minus level badge and streak
                if (_maxProgressWidth < 100) _maxProgressWidth = 200;
            }
        };
    }

    /// <summary>
    /// Updates the XP bar with current profile data
    /// </summary>
    public void UpdateProfile(UserProfile profile)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Update level
            LevelLabel.Text = profile.CurrentLevel.ToString();

            // Update total XP
            XPLabel.Text = $"{profile.TotalXP:N0} XP";

            // Update progress to next level
            int currentLevelXP = profile.GetXPForCurrentLevel();
            int nextLevelXP = profile.GetXPForNextLevel();
            int xpInCurrentLevel = profile.TotalXP - currentLevelXP;
            int xpNeededForLevel = nextLevelXP - currentLevelXP;

            ProgressLabel.Text = $"({ xpInCurrentLevel}/{xpNeededForLevel} to level {profile.CurrentLevel + 1})";

            // Update progress bar
            double percentage = profile.GetLevelProgress();
            PercentageLabel.Text = $"{percentage:F0}%";

            // Animate progress bar if width is available
            if (_maxProgressWidth > 0)
            {
                var targetWidth = _maxProgressWidth * (percentage / 100.0);
                ProgressFill.WidthRequest = targetWidth;
            }
            else
            {
                // Fallback: use percentage-based width
                ProgressFill.WidthRequest = 200 * (percentage / 100.0);
            }

            // Update streak
            StreakLabel.Text = profile.Streak.ToString();

            // Change streak color based on value
            var streakBorder = (Border)StreakLabel.Parent.Parent;
            if (profile.Streak >= 30)
            {
                streakBorder.BackgroundColor = Color.FromArgb("#ffd700"); // Gold
                streakBorder.Stroke = Color.FromArgb("#ffed4e");
            }
            else if (profile.Streak >= 7)
            {
                streakBorder.BackgroundColor = Color.FromArgb("#ff6b35"); // Orange
                streakBorder.Stroke = Color.FromArgb("#ff8c61");
            }
            else
            {
                streakBorder.BackgroundColor = Color.FromArgb("#64748b"); // Gray
                streakBorder.Stroke = Color.FromArgb("#94a3b8");
            }
        });
    }

    /// <summary>
    /// Animates XP gain (visual feedback)
    /// </summary>
    public async Task AnimateXPGainAsync(int xpGained)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            // Create floating XP label
            var floatingLabel = new Label
            {
                Text = $"+{xpGained} XP",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#4ade80"),
                Opacity = 0
            };

            // Add to parent grid
            if (this.Parent is Grid parentGrid)
            {
                parentGrid.Add(floatingLabel);
                
                // Animate up and fade out
                await Task.WhenAll(
                    floatingLabel.FadeTo(1, 200),
                    floatingLabel.TranslateTo(0, -50, 1000, Easing.CubicOut)
                );
                
                await floatingLabel.FadeTo(0, 300);
                
                parentGrid.Remove(floatingLabel);
            }
        });
    }

    /// <summary>
    /// Shows level-up animation
    /// </summary>
    public async Task AnimateLevelUpAsync(int newLevel, string reward)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            // Pulse animation on level badge
            await LevelLabel.Parent.Parent.ScaleTo(1.3, 200, Easing.BounceOut);
            await LevelLabel.Parent.Parent.ScaleTo(1.0, 200, Easing.BounceIn);
            
            // Show level-up message
            await Application.Current!.MainPage!.DisplayAlert(
                "🎉 LEVEL UP!",
                $"Congratulations! You've reached Level {newLevel}!\n\n{reward}",
                "Awesome!"
            );
        });
    }
}
