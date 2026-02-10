using BijoyTypingMaster.Services;

namespace BijoyTypingMaster.Views;

public partial class MainPage : ContentPage
{
    private readonly DatabaseManager _dbManager;
    private readonly SettingsManager _settingsManager;
    private readonly CertificateGenerator _certGenerator;

    public MainPage(
        DatabaseManager dbManager,
        SettingsManager settingsManager,
        CertificateGenerator certGenerator)
    {
        InitializeComponent();
        _dbManager = dbManager;
        _settingsManager = settingsManager;
        _certGenerator = certGenerator;
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
        await DisplayAlert("Progress", "Progress tracking feature coming soon!", "OK");
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
