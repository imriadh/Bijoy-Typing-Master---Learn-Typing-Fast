using BijoyTypingMaster.Services;

namespace BijoyTypingMaster.Views;

public partial class PaymentWindow : ContentPage
{
    private readonly LicenseManager _licenseManager;
    private const string PAYMENT_FORM_URL = "https://forms.gle/PLACEHOLDER_LINK"; // Replace with your Google Form link

    public PaymentWindow()
    {
        InitializeComponent();
        
        _licenseManager = new LicenseManager();
        
        LoadLicenseInfo();
    }

    private void LoadLicenseInfo()
    {
        // Get machine ID
        string machineId = _licenseManager.GetMachineId();
        MachineIdLabel.Text = machineId;

        // Get trial status
        int remainingDays = _licenseManager.GetRemainingTrialDays();
        bool isPremium = _licenseManager.IsPremium();

        if (isPremium)
        {
            TrialStatusLabel.Text = "Premium User ✓";
            TrialDaysLabel.Text = "License: Active";
            StatusMessageLabel.Text = "Thank you for supporting Bijoy Typing Master!";
            
            // Hide purchase section if already premium
            BackButton.IsVisible = true;
        }
        else if (remainingDays > 0)
        {
            TrialStatusLabel.Text = "Trial Period";
            TrialDaysLabel.Text = $"Days Remaining: {remainingDays}";
            StatusMessageLabel.Text = "Upgrade to premium for lifetime access!";
            BackButton.IsVisible = true;
        }
        else
        {
            TrialStatusLabel.Text = "Trial Expired ⚠️";
            TrialDaysLabel.Text = "Days Remaining: 0";
            StatusMessageLabel.Text = "Purchase a license to continue using the app";
            BackButton.IsVisible = false;
        }
    }

    private async void OnCopyMachineIdClicked(object sender, EventArgs e)
    {
        string machineId = MachineIdLabel.Text;
        
        try
        {
            await Clipboard.SetTextAsync(machineId);
            await DisplayAlert("Copied!", "Machine ID copied to clipboard", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Could not copy: {ex.Message}", "OK");
        }
    }

    private async void OnBuyNowClicked(object sender, EventArgs e)
    {
        try
        {
            // Open the payment form URL in browser
            await Launcher.OpenAsync(new Uri(PAYMENT_FORM_URL));
            
            await DisplayAlert("Payment Instructions", 
                "1. Fill out the form with your Machine ID\n" +
                "2. Send 200 BDT via bKash\n" +
                "3. You'll receive your license key via email within 24 hours", 
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", 
                $"Could not open browser: {ex.Message}\n\n" +
                $"Please visit manually: {PAYMENT_FORM_URL}", 
                "OK");
        }
    }

    private async void OnActivateLicenseClicked(object sender, EventArgs e)
    {
        string licenseKey = LicenseKeyEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(licenseKey))
        {
            await DisplayAlert("Invalid Key", "Please enter a license key", "OK");
            return;
        }

        // Validate and activate
        bool success = _licenseManager.ActivateLicense(licenseKey);

        if (success)
        {
            await DisplayAlert("Success! 🎉", 
                "Your license has been activated!\nThank you for your purchase!", 
                "OK");
            
            LoadLicenseInfo();
            
            // Navigate back to main app
            Application.Current!.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Invalid License Key", 
                "The license key you entered is invalid or doesn't match your machine ID.\n\n" +
                "Please check:\n" +
                "• The key is entered correctly\n" +
                "• The key was generated for this machine ID\n" +
                "• There are no extra spaces", 
                "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (_licenseManager.IsValid())
        {
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Trial Expired", 
                "Your trial period has ended. Please purchase a license to continue.", 
                "OK");
        }
    }
}
