using System.Text.Json;
using BijoyTypingMaster.Models;

namespace BijoyTypingMaster.Services;

/// <summary>
/// Manages application settings and preferences
/// </summary>
public class SettingsManager
{
    private readonly string _settingsPath;
    private AppSettings _currentSettings;

    public AppSettings CurrentSettings => _currentSettings;

    public SettingsManager()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appDataPath, "BijoyTypingMaster");
        
        if (!Directory.Exists(appFolder))
        {
            Directory.CreateDirectory(appFolder);
        }
        
        _settingsPath = Path.Combine(appFolder, "settings.json");
        _currentSettings = LoadSettings();
    }

    /// <summary>
    /// Load settings from file or create default
    /// </summary>
    public AppSettings LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                string json = File.ReadAllText(_settingsPath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json);
                return settings ?? new AppSettings();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading settings: {ex.Message}");
        }

        return new AppSettings();
    }

    /// <summary>
    /// Save settings to file
    /// </summary>
    public void SaveSettings(AppSettings settings)
    {
        try
        {
            _currentSettings = settings;
            
            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true 
            };
            
            string json = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(_settingsPath, json);
            
            Console.WriteLine("Settings saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving settings: {ex.Message}");
        }
    }

    /// <summary>
    /// Update a specific setting
    /// </summary>
    public void UpdateSetting<T>(Action<AppSettings> updateAction)
    {
        updateAction(_currentSettings);
        SaveSettings(_currentSettings);
    }

    /// <summary>
    /// Reset to default settings
    /// </summary>
    public void ResetToDefaults()
    {
        _currentSettings = new AppSettings();
        SaveSettings(_currentSettings);
    }

    /// <summary>
    /// Get font size for UI elements
    /// </summary>
    public int GetFontSize()
    {
        return _currentSettings.FontSize;
    }

    /// <summary>
    /// Set font size
    /// </summary>
    public void SetFontSize(int size)
    {
        _currentSettings.FontSize = Math.Clamp(size, 16, 64);
        SaveSettings(_currentSettings);
    }

    /// <summary>
    /// Toggle sound on/off
    /// </summary>
    public void ToggleSound()
    {
        _currentSettings.SoundEnabled = !_currentSettings.SoundEnabled;
        SaveSettings(_currentSettings);
    }

    /// <summary>
    /// Set theme (Light/Dark)
    /// </summary>
    public void SetTheme(string theme)
    {
        _currentSettings.Theme = theme;
        SaveSettings(_currentSettings);
    }
}
