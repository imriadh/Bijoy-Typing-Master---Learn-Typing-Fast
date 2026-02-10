namespace BijoyTypingMaster.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
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
}
