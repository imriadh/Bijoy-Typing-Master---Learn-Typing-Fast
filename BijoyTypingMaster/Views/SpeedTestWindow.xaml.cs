using BijoyTypingMaster.Models;
using BijoyTypingMaster.Services;
using System.Timers;

namespace BijoyTypingMaster.Views;

public partial class SpeedTestWindow : ContentPage
{
    private readonly DatabaseManager _dbManager;
    private readonly SettingsManager _settingsManager;
    private readonly CertificateGenerator _certGenerator;
    private SpeedTestEngine? _engine;
    private System.Timers.Timer? _timer;
    private SpeedTestResult? _lastResult;

    public SpeedTestWindow(
        DatabaseManager dbManager, 
        SettingsManager settingsManager,
        CertificateGenerator certGenerator)
    {
        InitializeComponent();
        _dbManager = dbManager;
        _settingsManager = settingsManager;
        _certGenerator = certGenerator;
    }

    private async void OnStartClicked(object sender, EventArgs e)
    {
        try
        {
            // Get settings
            var settings = _settingsManager.CurrentSettings;
            int duration = settings.SpeedTestDuration;

            // Create layout
            IKeyboardLayout layout = settings.PreferredLayout == "Bijoy" 
                ? new BijoyLayout() 
                : new UnicodeLayout();

            // Initialize engine
            _engine = new SpeedTestEngine(layout);

            // Generate test text
            string testText = SpeedTestEngine.GenerateRandomTestText(50);
            
            // Start test
            _engine.StartTest(testText, duration);

            // Update UI
            TestTextLabel.Text = testText;
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            ResultsPanel.IsVisible = false;
            HiddenEntry.Text = string.Empty;
            HiddenEntry.Focus();

            // Start timer
            StartTimer();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to start test: {ex.Message}", "OK");
        }
    }

    private void StartTimer()
    {
        _timer = new System.Timers.Timer(100); // Update every 100ms
        _timer.Elapsed += OnTimerTick;
        _timer.Start();
    }

    private void OnTimerTick(object? sender, ElapsedEventArgs e)
    {
        if (_engine == null || !_engine.IsRunning) return;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Update timer
            TimerLabel.Text = _engine.RemainingSeconds.ToString();

            // Update progress
            ProgressLabel.Text = $"{_engine.GetProgress():F0}%";

            // Check if complete
            if (_engine.IsComplete || _engine.RemainingSeconds <= 0)
            {
                FinishTest();
            }
        });
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_engine == null || !_engine.IsRunning) return;

        // Get the last character typed
        if (e.NewTextValue?.Length > e.OldTextValue?.Length)
        {
            string lastChar = e.NewTextValue[^1..];
            _engine.ProcessKey(lastChar);
        }
        else if (e.NewTextValue?.Length < e.OldTextValue?.Length)
        {
            _engine.ProcessBackspace();
        }

        // Calculate real-time stats
        UpdateRealtimeStats();
    }

    private void UpdateRealtimeStats()
    {
        if (_engine == null) return;

        // Simple real-time WPM calculation
        double minutes = _engine.ElapsedSeconds / 60.0;
        if (minutes > 0)
        {
            double wpm = (_engine.TypedText.Length / 5.0) / minutes;
            WpmLabel.Text = $"{wpm:F0}";
        }

        // Simple accuracy calculation
        int correct = 0;
        int compareLength = Math.Min(_engine.TypedText.Length, _engine.TestText.Length);
        for (int i = 0; i < compareLength; i++)
        {
            if (_engine.TypedText[i] == _engine.TestText[i])
                correct++;
        }

        if (compareLength > 0)
        {
            double accuracy = (correct * 100.0) / compareLength;
            AccuracyLabel.Text = $"{accuracy:F0}%";
        }
    }

    private void OnStopClicked(object sender, EventArgs e)
    {
        FinishTest();
    }

    private async void FinishTest()
    {
        try
        {
            // Stop timer
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;

            if (_engine == null) return;

            // Get result
            _lastResult = _engine.GetResult();

            // Save to database
            _dbManager.SaveSpeedTestResult(_lastResult);

            // Show results
            ShowResults(_lastResult);

            // Update UI state
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to finish test: {ex.Message}", "OK");
        }
    }

    private void ShowResults(SpeedTestResult result)
    {
        // Show results panel
        ResultsPanel.IsVisible = true;

        // Set stars and rating
        StarsLabel.Text = new string('⭐', result.GetStars());
        RatingLabel.Text = result.GetRating();

        // Set detailed stats
        GrossWpmLabel.Text = $"{result.WPM:F2}";
        NetWpmLabel.Text = $"{result.NetWPM:F2}";
        FinalAccuracyLabel.Text = $"{result.Accuracy:F2}%";
        ErrorsLabel.Text = result.ErrorCount.ToString();

        // Update header stats
        WpmLabel.Text = $"{result.WPM:F0}";
        AccuracyLabel.Text = $"{result.Accuracy:F0}%";
        ProgressLabel.Text = "100%";
        TimerLabel.Text = "0";
    }

    private async void OnGenerateCertificateClicked(object sender, EventArgs e)
    {
        if (_lastResult == null)
        {
            await DisplayAlert("Error", "No test result available", "OK");
            return;
        }

        try
        {
            var settings = _settingsManager.CurrentSettings;
            string userName = string.IsNullOrWhiteSpace(settings.UserName) 
                ? "Typing Master User" 
                : settings.UserName;

            var certificate = _certGenerator.GenerateCertificate(userName, _lastResult);
            string certText = _certGenerator.ExportAsText(certificate);

            // Show certificate in dialog
            await DisplayAlert(
                "🏆 Certificate Generated", 
                certText, 
                "OK"
            );

            // In a real app, you would also save this as PDF or image
            // For now, just copy to clipboard
            await Clipboard.SetTextAsync(certText);
            await DisplayAlert(
                "Copied", 
                "Certificate text has been copied to clipboard!", 
                "OK"
            );
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to generate certificate: {ex.Message}", "OK");
        }
    }

    private void OnRetryClicked(object sender, EventArgs e)
    {
        // Reset UI
        ResultsPanel.IsVisible = false;
        WpmLabel.Text = "0";
        AccuracyLabel.Text = "100%";
        ProgressLabel.Text = "0%";
        TimerLabel.Text = _settingsManager.CurrentSettings.SpeedTestDuration.ToString();
        TestTextLabel.Text = "Click START to begin the speed test...";
        HiddenEntry.Text = string.Empty;

        // Reset engine
        _engine?.Reset();
        _engine = null;
        _lastResult = null;

        // Enable start button
        StartButton.IsEnabled = true;
        StopButton.IsEnabled = false;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        // Stop timer if running
        _timer?.Stop();
        _timer?.Dispose();

        await Shell.Current.GoToAsync("..");
    }
}
