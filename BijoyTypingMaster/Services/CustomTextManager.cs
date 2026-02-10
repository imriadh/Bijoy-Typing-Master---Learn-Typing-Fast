using BijoyTypingMaster.Models;

namespace BijoyTypingMaster.Services;

/// <summary>
/// Manages custom text practice sessions
/// </summary>
public class CustomTextManager
{
    private readonly DatabaseManager _dbManager;

    public CustomTextManager(DatabaseManager dbManager)
    {
        _dbManager = dbManager;
    }

    /// <summary>
    /// Saves a new custom text
    /// </summary>
    public async Task<(bool success, string message, int id)> SaveCustomTextAsync(string title, string text)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(title))
        {
            return (false, "Title cannot be empty", 0);
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            return (false, "Text cannot be empty", 0);
        }

        if (text.Length < 50)
        {
            return (false, "Text must be at least 50 characters long", 0);
        }

        if (text.Length > 5000)
        {
            return (false, "Text cannot exceed 5000 characters", 0);
        }

        // Check if text contains Bangla characters
        if (!ContainsBanglaCharacters(text))
        {
            return (false, "Text must contain Bangla characters", 0);
        }

        try
        {
            int id = await _dbManager.SaveCustomTextAsync(title, text);
            return (true, "Custom text saved successfully!", id);
        }
        catch (Exception ex)
        {
            return (false, $"Failed to save: {ex.Message}", 0);
        }
    }

    /// <summary>
    /// Gets all saved custom texts
    /// </summary>
    public async Task<List<CustomPracticeSession>> GetSavedTextsAsync()
    {
        return await _dbManager.GetCustomTextsAsync();
    }

    /// <summary>
    /// Updates stats after completing a practice session
    /// </summary>
    public async Task UpdateStatsAsync(int id, int wpm, int accuracy)
    {
        await _dbManager.UpdateCustomTextStatsAsync(id, wpm, accuracy);
    }

    /// <summary>
    /// Deletes a custom text
    /// </summary>
    public async Task<bool> DeleteTextAsync(int id)
    {
        try
        {
            await _dbManager.DeleteCustomTextAsync(id);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Imports text from a file
    /// </summary>
    public async Task<(bool success, string message, string text)> ImportFromFileAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return (false, "File not found", string.Empty);
            }

            string extension = Path.GetExtension(filePath).ToLower();
            if (extension != ".txt")
            {
                return (false, "Only .txt files are supported", string.Empty);
            }

            string text = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrWhiteSpace(text))
            {
                return (false, "File is empty", string.Empty);
            }

            if (text.Length < 50)
            {
                return (false, "Text must be at least 50 characters", string.Empty);
            }

            if (text.Length > 5000)
            {
                text = text.Substring(0, 5000); // Truncate
            }

            return (true, "File imported successfully", text);
        }
        catch (Exception ex)
        {
            return (false, $"Import failed: {ex.Message}", string.Empty);
        }
    }

    /// <summary>
    /// Checks if text contains Bangla characters
    /// </summary>
    private bool ContainsBanglaCharacters(string text)
    {
        // Bangla Unicode range: \u0980 to \u09FF
        return text.Any(c => c >= '\u0980' && c <= '\u09FF');
    }

    /// <summary>
    /// Validates text for practice
    /// </summary>
    public (bool valid, string message) ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return (false, "Text cannot be empty");

        if (text.Length < 50)
            return (false, $"Text too short. Need at least 50 characters ({text.Length}/50)");

        if (text.Length > 5000)
            return (false, $"Text too long. Maximum 5000 characters ({text.Length}/5000)");

        if (!ContainsBanglaCharacters(text))
            return (false, "Text must contain Bangla characters");

        return (true, "Text is valid");
    }
}
