using BijoyTypingMaster.Models;
using BijoyTypingMaster.Services;
using System.Collections.ObjectModel;

namespace BijoyTypingMaster.Views;

public partial class StatisticsWindow : ContentPage
{
    private readonly DatabaseManager _dbManager;
    public ObservableCollection<SpeedTestResult> SpeedTests { get; set; }
    public ObservableCollection<UserProgress> PracticeSessions { get; set; }

    public StatisticsWindow(DatabaseManager dbManager)
    {
        InitializeComponent();
        _dbManager = dbManager;
        
        SpeedTests = new ObservableCollection<SpeedTestResult>();
        PracticeSessions = new ObservableCollection<UserProgress>();
        
        SpeedTestsCollection.ItemsSource = SpeedTests;
        PracticeSessionsCollection.ItemsSource = PracticeSessions;
        
        DateRangePicker.SelectedIndex = 0; // Default to 7 days
        LoadStatistics(7);
    }

    private void OnDateRangeChanged(object sender, EventArgs e)
    {
        int days = DateRangePicker.SelectedIndex switch
        {
            0 => 7,
            1 => 30,
            2 => 365, // All time (1 year max)
            _ => 7
        };
        
        LoadStatistics(days);
    }

    private void LoadStatistics(int days)
    {
        // Load summary stats
        var (avgWpm, avgAccuracy) = _dbManager.GetAverageStats(days);
        AvgWpmLabel.Text = avgWpm > 0 ? $"{avgWpm:F1}" : "0.0";
        AvgAccLabel.Text = avgAccuracy > 0 ? $"{avgAccuracy:F1}%" : "0.0%";
        
        // Load best speed test result
        var bestResult = _dbManager.GetBestSpeedTestResult();
        BestWpmLabel.Text = bestResult != null ? $"{bestResult.WPM:F1}" : "0.0";

        // Load progress data for trends
        var progressData = _dbManager.GetProgressOverTime(days);
        
        if (progressData.Count > 0)
        {
            // Calculate WPM trend
            var wpmValues = progressData.Select(p => p.wpm).ToList();
            double minWpm = wpmValues.Min();
            double maxWpm = wpmValues.Max();
            double currentWpm = wpmValues.Last();
            
            if (maxWpm > minWpm)
            {
                WpmProgressBar.Progress = (currentWpm - minWpm) / (maxWpm - minWpm);
                WpmTrendLabel.Text = $"Current: {currentWpm:F1} WPM (Range: {minWpm:F1} - {maxWpm:F1})";
            }
            else
            {
                WpmProgressBar.Progress = 0.5;
                WpmTrendLabel.Text = $"Stable at {currentWpm:F1} WPM";
            }

            // Calculate Accuracy trend
            var accValues = progressData.Select(p => p.accuracy).ToList();
            double avgAcc = accValues.Average();
            
            if (avgAcc >= 70 && avgAcc <= 100)
            {
                AccuracyProgressBar.Progress = (avgAcc - 70) / 30.0; // Map 70-100% to 0-1
                AccuracyTrendLabel.Text = $"Average: {avgAcc:F1}%";
            }
            else
            {
                AccuracyProgressBar.Progress = 0.0;
                AccuracyTrendLabel.Text = $"Average: {avgAcc:F1}%";
            }

            // Session count
            SessionCountLabel.Text = $"{progressData.Count} session{(progressData.Count != 1 ? "s" : "")}";
        }
        else
        {
            WpmProgressBar.Progress = 0.0;
            WpmTrendLabel.Text = "No data yet - start practicing!";
            AccuracyProgressBar.Progress = 0.0;
            AccuracyTrendLabel.Text = "No data yet - start practicing!";
            SessionCountLabel.Text = "0 sessions";
        }

        // Load speed test history
        var speedTests = _dbManager.GetSpeedTestHistory(15);
        SpeedTests.Clear();
        foreach (var test in speedTests)
        {
            // Add Rating property for display
            var testWithRating = new SpeedTestResult
            {
                Id = test.Id,
                Date = test.Date,
                Duration = test.Duration,
                WPM = test.WPM,
                NetWPM = test.NetWPM,
                Accuracy = test.Accuracy,
                TotalCharacters = test.TotalCharacters,
                CorrectCharacters = test.CorrectCharacters,
                ErrorCount = test.ErrorCount,
                TestText = test.TestText
            };
            SpeedTests.Add(testWithRating);
        }

        // Load practice session history
        var practiceSessions = _dbManager.GetProgressHistory(15);
        PracticeSessions.Clear();
        foreach (var session in practiceSessions)
        {
            PracticeSessions.Add(session);
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
