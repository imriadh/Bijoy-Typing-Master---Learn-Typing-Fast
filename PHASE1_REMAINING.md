# Phase 1 - Remaining Features Implementation Guide

## 🎯 Overview
This guide provides step-by-step instructions to complete the last 2 features of Phase 1.

---

## Feature 5: Finger Position Guide Overlay

### 📋 Requirements
- Visual hand diagram showing proper finger placement
- Color-coded keys (each finger assigned a color)
- Highlight the current key being typed
- Toggle visibility from settings
- Semi-transparent overlay that doesn't obstruct practice

### 🎨 Design Approach

#### Color Scheme (Standard Touch Typing):
```
Left Pinky:    Red       → Keys: `, 1, q, a, z
Left Ring:     Orange    → Keys: 2, w, s, x
Left Middle:   Yellow    → Keys: 3, e, d, c
Left Index:    Green     → Keys: 4, 5, r, t, f, g, v, b
Right Index:   Blue      → Keys: 6, 7, y, u, h, j, n, m
Right Middle:  Indigo    → Keys: 8, i, k, ,
Right Ring:    Purple    → Keys: 9, o, l, .
Right Pinky:   Pink      → Keys: 0, -, =, p, [, ], ;, ', /, \
Thumbs:        Gray      → Space bar
```

### 📁 File Structure
```
Controls/
└── FingerGuideOverlay.xaml      (UI)
└── FingerGuideOverlay.xaml.cs   (Logic)

Resources/
└── Images/
    ├── hand_left.png            (Optional: Hand image)
    └── hand_right.png           (Optional: Hand image)
```

### 💻 Implementation Steps

#### Step 1: Create FingerGuideOverlay.xaml
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="BijoyTypingMaster.Controls.FingerGuideOverlay">
    
    <Border Background="#AA000000"  <!-- Semi-transparent -->
            Padding="20">
        <VerticalStackLayout Spacing="10" HorizontalOptions="Center">
            
            <!-- Title -->
            <Label Text="✋ Finger Position Guide" 
                   FontSize="18" 
                   FontAttributes="Bold"
                   HorizontalOptions="Center"/>
            
            <!-- Left Hand -->
            <Border StrokeThickness="2" 
                    Stroke="White"
                    Padding="15">
                <VerticalStackLayout Spacing="5">
                    <Label Text="Left Hand" 
                           FontSize="14" 
                           HorizontalOptions="Center"/>
                    
                    <!-- Finger labels with colors -->
                    <Grid ColumnDefinitions="*,*,*,*" RowDefinitions="Auto,Auto" ColumnSpacing="10" RowSpacing="5">
                        <Label Grid.Row="0" Grid.Column="0" Text="Pinky" BackgroundColor="Red" Padding="5" HorizontalOptions="Fill" HorizontalTextAlignment="Center"/>
                        <Label Grid.Row="1" Grid.Column="0" Text="a, q, z, 1" FontSize="10" HorizontalOptions="Center"/>
                        
                        <Label Grid.Row="0" Grid.Column="1" Text="Ring" BackgroundColor="Orange" Padding="5" HorizontalOptions="Fill" HorizontalTextAlignment="Center"/>
                        <Label Grid.Row="1" Grid.Column="1" Text="s, w, x, 2" FontSize="10" HorizontalOptions="Center"/>
                        
                        <Label Grid.Row="0" Grid.Column="2" Text="Middle" BackgroundColor="Yellow" TextColor="Black" Padding="5" HorizontalOptions="Fill" HorizontalTextAlignment="Center"/>
                        <Label Grid.Row="1" Grid.Column="2" Text="d, e, c, 3" FontSize="10" HorizontalOptions="Center"/>
                        
                        <Label Grid.Row="0" Grid.Column="3" Text="Index" BackgroundColor="Green" Padding="5" HorizontalOptions="Fill" HorizontalTextAlignment="Center"/>
                        <Label Grid.Row="1" Grid.Column="3" Text="f, g, r, t, v, b, 4, 5" FontSize="10" HorizontalOptions="Center"/>
                    </Grid>
                </VerticalStackLayout>
            </Border>
            
            <!-- Right Hand -->
            <Border StrokeThickness="2" 
                    Stroke="White"
                    Padding="15">
                <VerticalStackLayout Spacing="5">
                    <Label Text="Right Hand" 
                           FontSize="14" 
                           HorizontalOptions="Center"/>
                    
                    <Grid ColumnDefinitions="*,*,*,*" RowDefinitions="Auto,Auto" ColumnSpacing="10" RowSpacing="5">
                        <Label Grid.Row="0" Grid.Column="0" Text="Index" BackgroundColor="Blue" Padding="5" HorizontalOptions="Fill" HorizontalTextAlignment="Center"/>
                        <Label Grid.Row="1" Grid.Column="0" Text="j, h, y, u, n, m, 6, 7" FontSize="10" HorizontalOptions="Center"/>
                        
                        <Label Grid.Row="0" Grid.Column="1" Text="Middle" BackgroundColor="Indigo" Padding="5" HorizontalOptions="Fill" HorizontalTextAlignment="Center"/>
                        <Label Grid.Row="1" Grid.Column="1" Text="k, i, 8" FontSize="10" HorizontalOptions="Center"/>
                        
                        <Label Grid.Row="0" Grid.Column="2" Text="Ring" BackgroundColor="Purple" Padding="5" HorizontalOptions="Fill" HorizontalTextAlignment="Center"/>
                        <Label Grid.Row="1" Grid.Column="2" Text="l, o, 9" FontSize="10" HorizontalOptions="Center"/>
                        
                        <Label Grid.Row="0" Grid.Column="3" Text="Pinky" BackgroundColor="Pink" TextColor="Black" Padding="5" HorizontalOptions="Fill" HorizontalTextAlignment="Center"/>
                        <Label Grid.Row="1" Grid.Column="3" Text=";, p, 0, -, =" FontSize="10" HorizontalOptions="Center"/>
                    </Grid>
                </VerticalStackLayout>
            </Border>
            
            <!-- Current Key Highlight -->
            <Border x:Name="CurrentKeyBorder"
                    StrokeThickness="3" 
                    Stroke="Yellow"
                    Background="#44FFFF00"
                    Padding="20"
                    IsVisible="False">
                <Label x:Name="CurrentKeyLabel" 
                       Text="" 
                       FontSize="24" 
                       FontAttributes="Bold"
                       HorizontalOptions="Center"/>
            </Border>
            
        </VerticalStackLayout>
    </Border>
    
</ContentView>
```

#### Step 2: Create FingerGuideOverlay.xaml.cs
```csharp
using System.Collections.Generic;

namespace BijoyTypingMaster.Controls;

public partial class FingerGuideOverlay : ContentView
{
    private readonly Dictionary<string, (string finger, Color color)> _fingerMap;

    public FingerGuideOverlay()
    {
        InitializeComponent();
        _fingerMap = InitializeFingerMap();
    }

    private Dictionary<string, (string, Color)> InitializeFingerMap()
    {
        return new Dictionary<string, (string, Color)>
        {
            // Left Hand
            ["a"] = ("Left Pinky", Colors.Red),
            ["q"] = ("Left Pinky", Colors.Red),
            ["z"] = ("Left Pinky", Colors.Red),
            ["1"] = ("Left Pinky", Colors.Red),
            
            ["s"] = ("Left Ring", Colors.Orange),
            ["w"] = ("Left Ring", Colors.Orange),
            ["x"] = ("Left Ring", Colors.Orange),
            ["2"] = ("Left Ring", Colors.Orange),
            
            ["d"] = ("Left Middle", Colors.Yellow),
            ["e"] = ("Left Middle", Colors.Yellow),
            ["c"] = ("Left Middle", Colors.Yellow),
            ["3"] = ("Left Middle", Colors.Yellow),
            
            ["f"] = ("Left Index", Colors.Green),
            ["g"] = ("Left Index", Colors.Green),
            ["r"] = ("Left Index", Colors.Green),
            ["t"] = ("Left Index", Colors.Green),
            ["v"] = ("Left Index", Colors.Green),
            ["b"] = ("Left Index", Colors.Green),
            ["4"] = ("Left Index", Colors.Green),
            ["5"] = ("Left Index", Colors.Green),
            
            // Right Hand
            ["j"] = ("Right Index", Colors.Blue),
            ["h"] = ("Right Index", Colors.Blue),
            ["y"] = ("Right Index", Colors.Blue),
            ["u"] = ("Right Index", Colors.Blue),
            ["n"] = ("Right Index", Colors.Blue),
            ["m"] = ("Right Index", Colors.Blue),
            ["6"] = ("Right Index", Colors.Blue),
            ["7"] = ("Right Index", Colors.Blue),
            
            ["k"] = ("Right Middle", Colors.Indigo),
            ["i"] = ("Right Middle", Colors.Indigo),
            [","] = ("Right Middle", Colors.Indigo),
            ["8"] = ("Right Middle", Colors.Indigo),
            
            ["l"] = ("Right Ring", Colors.Purple),
            ["o"] = ("Right Ring", Colors.Purple),
            ["."] = ("Right Ring", Colors.Purple),
            ["9"] = ("Right Ring", Colors.Purple),
            
            [";"] = ("Right Pinky", Colors.Pink),
            ["p"] = ("Right Pinky", Colors.Pink),
            ["0"] = ("Right Pinky", Colors.Pink),
            ["-"] = ("Right Pinky", Colors.Pink),
            ["="] = ("Right Pinky", Colors.Pink),
            
            [" "] = ("Thumbs", Colors.Gray)
        };
    }

    public void HighlightKey(string key)
    {
        if (_fingerMap.TryGetValue(key.ToLower(), out var fingerInfo))
        {
            CurrentKeyLabel.Text = $"Press '{key}' with {fingerInfo.finger}";
            CurrentKeyBorder.Stroke = new SolidColorBrush(fingerInfo.color);
            CurrentKeyBorder.IsVisible = true;
        }
        else
        {
            CurrentKeyBorder.IsVisible = false;
        }
    }

    public void ClearHighlight()
    {
        CurrentKeyBorder.IsVisible = false;
    }
}
```

#### Step 3: Integrate into PracticeWindow.xaml
Add the overlay to PracticeWindow:

```xml
<!-- Add this inside the Grid, above the main content -->
<controls:FingerGuideOverlay x:Name="FingerGuide"
                             IsVisible="{Binding ShowFingerGuide}"
                             VerticalOptions="Start"
                             ZIndex="10"/>
```

#### Step 4: Update PracticeWindow.xaml.cs
```csharp
// In constructor, load setting
var settings = App.ServiceProvider.GetService<SettingsManager>();
FingerGuide.IsVisible = settings.CurrentSettings.ShowFingerGuide;

// In OnTextChanged(), highlight current key
if (!string.IsNullOrEmpty(expectedKey))
{
    FingerGuide.HighlightKey(expectedKey);
}
```

---

## Feature 6: Statistics Dashboard

### 📋 Requirements
- WPM over time (line chart)
- Accuracy trends (line chart)
- Practice sessions per day (bar chart)
- Speed test history (table)
- Summary statistics (cards)
- Date range filters (7 days, 30 days, all time)

### 📦 Required NuGet Package
```bash
dotnet add package LiveChartsCore.SkiaSharpView.Maui
```

### 📁 File Structure
```
Views/
├── StatisticsWindow.xaml      (UI with charts)
└── StatisticsWindow.xaml.cs   (Chart data binding)
```

### 💻 Implementation Steps

#### Step 1: Install LiveCharts2
```bash
cd /workspaces/Bijoy-Typing-Master---Learn-Typing-Fast/BijoyTypingMaster
dotnet add package LiveChartsCore.SkiaSharpView.Maui --version 2.0.0-rc2
```

#### Step 2: Register LiveCharts in MauiProgram.cs
```csharp
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

// In CreateMauiApp():
builder
    .UseMauiApp<App>()
    .UseSkiaSharp()  // Add this
    .ConfigureFonts(fonts => { ... });

LiveCharts.Configure(config =>
    config
        .AddSkiaSharp()
        .AddDefaultMappers()
);
```

#### Step 3: Create StatisticsWindow.xaml
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:lvc="clr-namespace:LiveChartsCore.SkiaSharpView.Maui;assembly=LiveChartsCore.SkiaSharpView.Maui"
             x:Class="BijoyTypingMaster.Views.StatisticsWindow"
             Title="📊 Statistics">
    
    <ScrollView>
        <VerticalStackLayout Padding="30" Spacing="20">
            
            <!-- Header -->
            <Label Text="📊 Your Progress Statistics" 
                   FontSize="32" 
                   FontAttributes="Bold" 
                   HorizontalOptions="Center"/>

            <!-- Date Filter -->
            <Picker x:Name="DateRangePicker"
                    Title="Select Time Range"
                    SelectedIndexChanged="OnDateRangeChanged">
                <Picker.Items>
                    <x:String>Last 7 Days</x:String>
                    <x:String>Last 30 Days</x:String>
                    <x:String>All Time</x:String>
                </Picker.Items>
            </Picker>

            <!-- Summary Cards -->
            <Grid ColumnDefinitions="*,*,*" ColumnSpacing="15">
                <Border Grid.Column="0" StrokeThickness="2" Padding="15">
                    <VerticalStackLayout Spacing="5">
                        <Label Text="Average WPM" FontSize="12" Opacity="0.7"/>
                        <Label x:Name="AvgWpmLabel" Text="0" FontSize="24" FontAttributes="Bold"/>
                    </VerticalStackLayout>
                </Border>
                
                <Border Grid.Column="1" StrokeThickness="2" Padding="15">
                    <VerticalStackLayout Spacing="5">
                        <Label Text="Avg Accuracy" FontSize="12" Opacity="0.7"/>
                        <Label x:Name="AvgAccLabel" Text="0%" FontSize="24" FontAttributes="Bold"/>
                    </VerticalStackLayout>
                </Border>
                
                <Border Grid.Column="2" StrokeThickness="2" Padding="15">
                    <VerticalStackLayout Spacing="5">
                        <Label Text="Best WPM" FontSize="12" Opacity="0.7"/>
                        <Label x:Name="BestWpmLabel" Text="0" FontSize="24" FontAttributes="Bold"/>
                    </VerticalStackLayout>
                </Border>
            </Grid>

            <!-- WPM Chart -->
            <Border StrokeThickness="2" Padding="15">
                <VerticalStackLayout Spacing="10">
                    <Label Text="📈 WPM Over Time" FontSize="18" FontAttributes="Bold"/>
                    <lvc:CartesianChart x:Name="WpmChart" 
                                        HeightRequest="250"
                                        Series="{Binding WpmSeries}"/>
                </VerticalStackLayout>
            </Border>

            <!-- Accuracy Chart -->
            <Border StrokeThickness="2" Padding="15">
                <VerticalStackLayout Spacing="10">
                    <Label Text="✅ Accuracy Trends" FontSize="18" FontAttributes="Bold"/>
                    <lvc:CartesianChart x:Name="AccuracyChart" 
                                        HeightRequest="250"
                                        Series="{Binding AccuracySeries}"/>
                </VerticalStackLayout>
            </Border>

            <!-- Speed Test History Table -->
            <Border StrokeThickness="2" Padding="15">
                <VerticalStackLayout Spacing="10">
                    <Label Text="⚡ Recent Speed Tests" FontSize="18" FontAttributes="Bold"/>
                    <CollectionView x:Name="SpeedTestsCollection"
                                    ItemsSource="{Binding SpeedTests}"
                                    HeightRequest="200">
                        <CollectionView.ItemTemplate>
                            <DataTemplate>
                                <Grid ColumnDefinitions="*,*,*,*" Padding="5">
                                    <Label Grid.Column="0" Text="{Binding Date, StringFormat='{0:MM/dd}'}" />
                                    <Label Grid.Column="1" Text="{Binding WPM, StringFormat='{0:F1} WPM'}" />
                                    <Label Grid.Column="2" Text="{Binding Accuracy, StringFormat='{0:F1}%'}" />
                                    <Label Grid.Column="3" Text="{Binding Rating}" />
                                </Grid>
                            </DataTemplate>
                        </CollectionView.ItemTemplate>
                    </CollectionView>
                </VerticalStackLayout>
            </Border>

        </VerticalStackLayout>
    </ScrollView>
    
</ContentPage>
```

#### Step 4: Create StatisticsWindow.xaml.cs
```csharp
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using BijoyTypingMaster.Services;
using System.Collections.ObjectModel;

namespace BijoyTypingMaster.Views;

public partial class StatisticsWindow : ContentPage
{
    private readonly DatabaseManager _dbManager;
    public ObservableCollection<ISeries> WpmSeries { get; set; }
    public ObservableCollection<ISeries> AccuracySeries { get; set; }
    public ObservableCollection<SpeedTestResult> SpeedTests { get; set; }

    public StatisticsWindow(DatabaseManager dbManager)
    {
        InitializeComponent();
        _dbManager = dbManager;
        
        WpmSeries = new ObservableCollection<ISeries>();
        AccuracySeries = new ObservableCollection<ISeries>();
        SpeedTests = new ObservableCollection<SpeedTestResult>();
        
        BindingContext = this;
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
        AvgWpmLabel.Text = $"{avgWpm:F1}";
        AvgAccLabel.Text = $"{avgAccuracy:F1}%";
        
        var bestResult = _dbManager.GetBestSpeedTestResult();
        BestWpmLabel.Text = bestResult != null ? $"{bestResult.WPM:F1}" : "0";

        // Load chart data
        var progressData = _dbManager.GetProgressOverTime(days);
        
        var wpmValues = new List<double>();
        var accuracyValues = new List<double>();
        foreach (var item in progressData)
        {
            wpmValues.Add(item.wpm);
            accuracyValues.Add(item.accuracy);
        }

        WpmSeries.Clear();
        WpmSeries.Add(new LineSeries<double>
        {
            Values = wpmValues,
            Name = "WPM"
        });

        AccuracySeries.Clear();
        AccuracySeries.Add(new LineSeries<double>
        {
            Values = accuracyValues,
            Name = "Accuracy"
        });

        // Load speed test history
        var speedTests = _dbManager.GetSpeedTestHistory(10);
        SpeedTests.Clear();
        foreach (var test in speedTests)
        {
            SpeedTests.Add(test);
        }
    }
}
```

#### Step 5: Register in MauiProgram.cs
```csharp
builder.Services.AddTransient<StatisticsWindow>();
```

#### Step 6: Add Navigation Button in MainPage
```csharp
// In MainPage.xaml.cs
private async void OnViewProgressClicked(object sender, EventArgs e)
{
    var statsWindow = App.ServiceProvider.GetService<StatisticsWindow>();
    await Navigation.PushAsync(statsWindow);
}
```

---

## 🧪 Testing Plan

### Finger Guide Testing:
1. Navigate to Practice window
2. Verify finger guide is visible (if enabled in settings)
3. Type keys and verify correct finger/color highlights
4. Toggle setting and verify visibility changes

### Statistics Dashboard Testing:
1. Add some practice sessions and speed tests
2. Open Statistics window
3. Verify charts display correctly
4. Change date range filter
5. Verify data updates correctly
6. Check summary cards show accurate numbers

---

## 📉 Estimated Time

- **Finger Guide:** 2-3 hours
  - UI design: 1 hour
  - Key mapping: 30 minutes
  - Integration: 30 minutes
  - Testing: 1 hour

- **Statistics Dashboard:** 4-5 hours
  - LiveCharts setup: 1 hour
  - UI layout: 1 hour
  - Data binding: 1 hour
  - Chart configuration: 1 hour
  - Testing: 1 hour

**Total:** 6-8 hours

---

## ✅ Completion Checklist

### Finger Guide:
- [ ] FingerGuideOverlay.xaml created
- [ ] FingerGuideOverlay.xaml.cs created
- [ ] Finger mapping dictionary complete
- [ ] Integrated into PracticeWindow
- [ ] Settings toggle works
- [ ] Color highlighting works
- [ ] Tested on Windows

### Statistics Dashboard:
- [ ] LiveCharts package installed
- [ ] StatisticsWindow.xaml created
- [ ] StatisticsWindow.xaml.cs created
- [ ] WPM chart displays
- [ ] Accuracy chart displays
- [ ] Summary cards show data
- [ ] Date range filter works
- [ ] Speed test table populates
- [ ] Registered in DI
- [ ] Navigation added
- [ ] Tested on Windows

---

**Once both features are complete, Phase 1 will be 100% finished! 🎉**
