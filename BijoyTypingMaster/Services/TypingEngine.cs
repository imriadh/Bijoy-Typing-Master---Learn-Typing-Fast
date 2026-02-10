using System.Diagnostics;

namespace BijoyTypingMaster.Services;

/// <summary>
/// Core typing engine that handles input processing, WPM, and accuracy calculations
/// </summary>
public class TypingEngine
{
    private IKeyboardLayout _currentLayout;
    private string _inputBuffer = "";
    private readonly Stopwatch _timer;
    private int _correctCharacters = 0;
    private int _totalCharacters = 0;
    private string _targetText = "";
    private int _currentPosition = 0;

    public double WPM { get; private set; }
    public double Accuracy { get; private set; }
    public string CurrentInput { get; private set; } = "";
    public int CurrentPosition => _currentPosition;

    public TypingEngine()
    {
        _currentLayout = new BijoyLayout(); // Default layout
        _timer = new Stopwatch();
    }

    /// <summary>
    /// Set the keyboard layout (Bijoy or Unicode)
    /// </summary>
    public void SetLayout(string layoutType)
    {
        _currentLayout = layoutType.ToLower() == "unicode" 
            ? new UnicodeLayout() 
            : new BijoyLayout();
    }

    /// <summary>
    /// Start a new typing session with target text
    /// </summary>
    public void StartSession(string targetText)
    {
        _targetText = targetText;
        _currentPosition = 0;
        _correctCharacters = 0;
        _totalCharacters = 0;
        CurrentInput = "";
        _inputBuffer = "";
        WPM = 0;
        Accuracy = 100;
        _timer.Restart();
    }

    /// <summary>
    /// Process a key press and return the resulting character
    /// </summary>
    public string ProcessKeyPress(string key)
    {
        // Ignore special keys
        if (IsSpecialKey(key))
        {
            return "";
        }

        // Handle backspace
        if (key == "Backspace" || key == "Back")
        {
            if (CurrentInput.Length > 0 && _currentPosition > 0)
            {
                CurrentInput = CurrentInput.Substring(0, CurrentInput.Length - 1);
                _currentPosition--;
                _inputBuffer = "";
            }
            return "";
        }

        // Process the key through the layout
        string character = _currentLayout.ProcessKey(key, _inputBuffer);

        // Update buffer if needed
        if (_currentLayout.RequiresBuffer(key))
        {
            _inputBuffer += character;
            return "";
        }
        else
        {
            _inputBuffer = "";
        }

        // Add character to input
        if (!string.IsNullOrEmpty(character))
        {
            CurrentInput += character;
            _totalCharacters++;

            // Check if character matches target
            if (_currentPosition < _targetText.Length)
            {
                if (character == _targetText[_currentPosition].ToString())
                {
                    _correctCharacters++;
                }
                _currentPosition++;
            }

            // Calculate metrics
            CalculateMetrics();

            return character;
        }

        return "";
    }

    /// <summary>
    /// Calculate WPM and Accuracy in real-time
    /// </summary>
    private void CalculateMetrics()
    {
        // Calculate WPM (assuming average word length of 5 characters)
        double minutes = _timer.Elapsed.TotalMinutes;
        if (minutes > 0)
        {
            WPM = (_totalCharacters / 5.0) / minutes;
        }

        // Calculate Accuracy
        if (_totalCharacters > 0)
        {
            Accuracy = (_correctCharacters / (double)_totalCharacters) * 100.0;
        }
        else
        {
            Accuracy = 100;
        }
    }

    /// <summary>
    /// End the typing session and return final stats
    /// </summary>
    public (double wpm, double accuracy) EndSession()
    {
        _timer.Stop();
        CalculateMetrics();
        return (Math.Round(WPM, 2), Math.Round(Accuracy, 2));
    }

    /// <summary>
    /// Get the current character in the target text
    /// </summary>
    public string GetCurrentTargetChar()
    {
        if (_currentPosition < _targetText.Length)
        {
            return _targetText[_currentPosition].ToString();
        }
        return "";
    }

    /// <summary>
    /// Check if the session is complete
    /// </summary>
    public bool IsSessionComplete()
    {
        return _currentPosition >= _targetText.Length;
    }

    /// <summary>
    /// Get completion percentage
    /// </summary>
    public double GetProgress()
    {
        if (_targetText.Length == 0) return 0;
        return (_currentPosition / (double)_targetText.Length) * 100.0;
    }

    /// <summary>
    /// Check if a key is a special/control key
    /// </summary>
    private bool IsSpecialKey(string key)
    {
        var specialKeys = new[] { 
            "Shift", "Control", "Alt", "Tab", "Escape", "Enter", 
            "CapsLock", "F1", "F2", "F3", "F4", "F5", "F6", 
            "F7", "F8", "F9", "F10", "F11", "F12",
            "Left", "Right", "Up", "Down", "Home", "End", 
            "PageUp", "PageDown", "Insert", "Delete"
        };

        return specialKeys.Contains(key);
    }

    /// <summary>
    /// Reset the typing engine
    /// </summary>
    public void Reset()
    {
        _timer.Reset();
        _currentPosition = 0;
        _correctCharacters = 0;
        _totalCharacters = 0;
        CurrentInput = "";
        _inputBuffer = "";
        WPM = 0;
        Accuracy = 100;
    }
}
