using BijoyTypingMaster.Services;

namespace BijoyTypingMaster;

public partial class App : Application
{
    private readonly LicenseManager _licenseManager;

    public App(LicenseManager licenseManager)
    {
        InitializeComponent();
        _licenseManager = licenseManager;

        // Check license on startup
        if (!_licenseManager.IsValid())
        {
            MainPage = new NavigationPage(new PaymentWindow());
        }
        else
        {
            MainPage = new AppShell();
        }
    }
}
