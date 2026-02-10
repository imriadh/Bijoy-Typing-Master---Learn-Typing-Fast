using BijoyTypingMaster.Models;
using BijoyTypingMaster.Services;

namespace BijoyTypingMaster.Views;

public partial class SettingsWindow : ContentPage
{
    private readonly SettingsManager _settingsManager;
    private AppSettings _settings;

    public SettingsWindow(SettingsManager settingsManager)
    {
        InitializeComponent();
        _settingsManager = settingsManager;
        _settings = _settingsManager.CurrentSettings;
        
        LoadSettings();
    }

    /// <summary>
    /// Load current settings into UI controls
    /// </summary>
    private void LoadSettings()
    {
        // Display Settings
        FontSizeSlider.Value = _settings.FontSize;
        FontSizeLabel.Text = _settings.FontSize.ToString();
        ThemePicker.SelectedItem = _settings.Theme;
        ShowKeyboardCheckbox.IsChecked = _settings.ShowVirtualKeyboard;
        ShowFingerGuideCheckbox.IsChecked = _settings.ShowFingerGuide;

        // Practice Settings
        LayoutPicker.SelectedItem = _settings.PreferredLayout;
        SoundCheckbox.IsChecked = _settings.SoundEnabled;
        AutoAdvanceCheckbox.IsChecked = _settings.AutoAdvanceLesson;
        MinAccuracySlider.Value = _settings.MinAccuracyToPass;
        MinAccuracyLabel.Text = _settings.MinAccuracyToPass.ToString();
        MinWpmSlider.Value = _settings.MinWpmToPass;
        MinWpmLabel.Text = _settings.MinWpmToPass.ToString();

        // Speed Test Settings
        DurationPicker.SelectedItem = _settings.SpeedTestDuration.ToString();

        // User Profile
        UserNameEntry.Text = _settings.UserName;
    }

    // Event Handlers
    private void OnFontSizeChanged(object sender, ValueChangedEventArgs e)
    {
        int fontSize = (int)e.NewValue;
        FontSizeLabel.Text = fontSize.ToString();
        _settings.FontSize = fontSize;
    }

    private void OnThemeChanged(object sender, EventArgs e)
    {
        if (ThemePicker.SelectedItem is string theme)
        {
            _settings.Theme = theme;
        }
    }

    private void OnShowKeyboardChanged(object sender, CheckedChangedEventArgs e)
    {
        _settings.ShowVirtualKeyboard = e.Value;
    }

    private void OnShowFingerGuideChanged(object sender, CheckedChangedEventArgs e)
    {
        _settings.ShowFingerGuide = e.Value;
    }

    private void OnLayoutChanged(object sender, EventArgs e)
    {
        if (LayoutPicker.SelectedItem is string layout)
        {
            _settings.PreferredLayout = layout;
        }
    }

    private void OnSoundChanged(object sender, CheckedChangedEventArgs e)
    {
        _settings.SoundEnabled = e.Value;
    }

    private void OnAutoAdvanceChanged(object sender, CheckedChangedEventArgs e)
    {
        _settings.AutoAdvanceLesson = e.Value;
    }

    private void OnMinAccuracyChanged(object sender, ValueChangedEventArgs e)
    {
        int accuracy = (int)e.NewValue;
        MinAccuracyLabel.Text = accuracy.ToString();
        _settings.MinAccuracyToPass = accuracy;
    }

    private void OnMinWpmChanged(object sender, ValueChangedEventArgs e)
    {
        int wpm = (int)e.NewValue;
        MinWpmLabel.Text = wpm.ToString();
        _settings.MinWpmToPass = wpm;
    }

    private void OnDurationChanged(object sender, EventArgs e)
    {
        if (DurationPicker.SelectedItem is string duration && int.TryParse(duration, out int seconds))
        {
            _settings.SpeedTestDuration = seconds;
        }
    }

    private void OnUserNameChanged(object sender, TextChangedEventArgs e)
    {
        _settings.UserName = e.NewTextValue ?? string.Empty;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            _settingsManager.SaveSettings(_settings);
            await DisplayAlert("Success", "✅ Settings saved successfully!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to save settings: {ex.Message}", "OK");
        }
    }

    private async void OnResetClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "Reset Settings", 
            "Are you sure you want to reset all settings to defaults?", 
            "Yes", 
            "No"
        );

        if (confirm)
        {
            _settingsManager.ResetToDefaults();
            _settings = _settingsManager.CurrentSettings;
            LoadSettings();
            await DisplayAlert("Reset Complete", "✅ Settings have been reset to defaults!", "OK");
        }
    }
}
