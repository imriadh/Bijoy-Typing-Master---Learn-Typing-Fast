using BijoyTypingMaster.Services;

namespace BijoyTypingMaster.Views;

public partial class MainPage : ContentPage
{
    private readonly DatabaseManager _dbManager;
    private readonly SettingsManager _settingsManager;
    private readonly CertificateGenerator _certGenerator;
    private readonly XPManager _xpManager;

    public MainPage(
        DatabaseManager dbManager,
        SettingsManager settingsManager,
        CertificateGenerator certGenerator,
        XPManager xpManager)
    {
        InitializeComponent();
        _dbManager = dbManager;
        _settingsManager = settingsManager;
        _certGenerator = certGenerator;
        _xpManager = xpManager;

        // Load XP profile on page load
        LoadXPProfile();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Refresh XP bar when returning to main page
        await LoadXPProfile();
    }

    private async Task LoadXPProfile()
    {
        try
        {
            // Update streak on app open
            await _xpManager.UpdateStreakAsync();

            // Load and display profile
            var profile = await _xpManager.GetOrCreateUserProfileAsync();
            XPBarControl.UpdateProfile(profile);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading XP profile: {ex.Message}");
        }
    }

    private async void OnBijoyPracticeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PracticeWindow("Bijoy"));
    }

    private async void OnUnicodePracticeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PracticeWindow("Unicode"));
    }

    private async void OnViewProgressClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StatisticsWindow(_dbManager));
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaymentWindow());
    }

    private async void OnSpeedTestClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(
            new SpeedTestWindow(_dbManager, _settingsManager, _certGenerator)
        );
    }

    private async void OnSettingsPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsWindow(_settingsManager));
    }
}
