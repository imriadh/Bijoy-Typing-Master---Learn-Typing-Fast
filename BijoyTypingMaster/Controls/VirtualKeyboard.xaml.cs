namespace BijoyTypingMaster.Controls;

public partial class VirtualKeyboard : ContentView
{
    private Dictionary<string, Button> _keyMap;
    private Color _normalColor;
    private Color _highlightColor;

    public VirtualKeyboard()
    {
        InitializeComponent();
        
        _normalColor = Colors.LightGray;
        _highlightColor = Colors.Yellow;
        
        InitializeKeyMap();
        ResetAllKeys();
    }

    private void InitializeKeyMap()
    {
        _keyMap = new Dictionary<string, Button>
        {
            // Numbers
            { "`", KeyTilde }, { "1", Key1 }, { "2", Key2 }, { "3", Key3 },
            { "4", Key4 }, { "5", Key5 }, { "6", Key6 }, { "7", Key7 },
            { "8", Key8 }, { "9", Key9 }, { "0", Key0 },
            { "-", KeyMinus }, { "=", KeyEqual },
            
            // Letters
            { "Q", KeyQ }, { "W", KeyW }, { "E", KeyE }, { "R", KeyR },
            { "T", KeyT }, { "Y", KeyY }, { "U", KeyU }, { "I", KeyI },
            { "O", KeyO }, { "P", KeyP },
            { "[", KeyBracketL }, { "]", KeyBracketR },
            
            { "A", KeyA }, { "S", KeyS }, { "D", KeyD }, { "F", KeyF },
            { "G", KeyG }, { "H", KeyH }, { "J", KeyJ }, { "K", KeyK },
            { "L", KeyL }, { ";", KeySemicolon }, { "'", KeyQuote },
            
            { "Z", KeyZ }, { "X", KeyX }, { "C", KeyC }, { "V", KeyV },
            { "B", KeyB }, { "N", KeyN }, { "M", KeyM },
            { ",", KeyComma }, { ".", KeyPeriod }, { "/", KeySlash },
            
            { " ", KeySpace }, { "Space", KeySpace }
        };
    }

    /// <summary>
    /// Highlight a key when pressed
    /// </summary>
    public void HighlightKey(string key)
    {
        // Reset all keys first
        ResetAllKeys();
        
        // Highlight the pressed key
        string upperKey = key.ToUpper();
        
        if (_keyMap.ContainsKey(upperKey))
        {
            _keyMap[upperKey].BackgroundColor = _highlightColor;
            
            // Reset after a short delay
            Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
            {
                _keyMap[upperKey].BackgroundColor = _normalColor;
                return false; // Stop the timer
            });
        }
    }

    /// <summary>
    /// Reset all keys to normal state
    /// </summary>
    private void ResetAllKeys()
    {
        foreach (var button in _keyMap.Values)
        {
            button.BackgroundColor = _normalColor;
        }
    }

    /// <summary>
    /// Set custom highlight color
    /// </summary>
    public void SetHighlightColor(Color color)
    {
        _highlightColor = color;
    }

    /// <summary>
    /// Set custom normal color
    /// </summary>
    public void SetNormalColor(Color color)
    {
        _normalColor = color;
        ResetAllKeys();
    }
}
