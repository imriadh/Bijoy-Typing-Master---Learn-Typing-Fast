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
            // Left Hand - Pinky
            ["a"] = ("Left Pinky", Colors.Red),
            ["q"] = ("Left Pinky", Colors.Red),
            ["z"] = ("Left Pinky", Colors.Red),
            ["1"] = ("Left Pinky", Colors.Red),
            ["`"] = ("Left Pinky", Colors.Red),
            
            // Left Hand - Ring
            ["s"] = ("Left Ring", Colors.Orange),
            ["w"] = ("Left Ring", Colors.Orange),
            ["x"] = ("Left Ring", Colors.Orange),
            ["2"] = ("Left Ring", Colors.Orange),
            
            // Left Hand - Middle
            ["d"] = ("Left Middle", Colors.Yellow),
            ["e"] = ("Left Middle", Colors.Yellow),
            ["c"] = ("Left Middle", Colors.Yellow),
            ["3"] = ("Left Middle", Colors.Yellow),
            
            // Left Hand - Index
            ["f"] = ("Left Index", Colors.Green),
            ["g"] = ("Left Index", Colors.Green),
            ["r"] = ("Left Index", Colors.Green),
            ["t"] = ("Left Index", Colors.Green),
            ["v"] = ("Left Index", Colors.Green),
            ["b"] = ("Left Index", Colors.Green),
            ["4"] = ("Left Index", Colors.Green),
            ["5"] = ("Left Index", Colors.Green),
            
            // Right Hand - Index
            ["j"] = ("Right Index", Colors.Blue),
            ["h"] = ("Right Index", Colors.Blue),
            ["y"] = ("Right Index", Colors.Blue),
            ["u"] = ("Right Index", Colors.Blue),
            ["n"] = ("Right Index", Colors.Blue),
            ["m"] = ("Right Index", Colors.Blue),
            ["6"] = ("Right Index", Colors.Blue),
            ["7"] = ("Right Index", Colors.Blue),
            
            // Right Hand - Middle
            ["k"] = ("Right Middle", Colors.Indigo),
            ["i"] = ("Right Middle", Colors.Indigo),
            [","] = ("Right Middle", Colors.Indigo),
            ["8"] = ("Right Middle", Colors.Indigo),
            
            // Right Hand - Ring
            ["l"] = ("Right Ring", Colors.Purple),
            ["o"] = ("Right Ring", Colors.Purple),
            ["."] = ("Right Ring", Colors.Purple),
            ["9"] = ("Right Ring", Colors.Purple),
            
            // Right Hand - Pinky
            [";"] = ("Right Pinky", Colors.Pink),
            ["p"] = ("Right Pinky", Colors.Pink),
            ["0"] = ("Right Pinky", Colors.Pink),
            ["-"] = ("Right Pinky", Colors.Pink),
            ["="] = ("Right Pinky", Colors.Pink),
            ["["] = ("Right Pinky", Colors.Pink),
            ["]"] = ("Right Pinky", Colors.Pink),
            ["'"] = ("Right Pinky", Colors.Pink),
            ["/"] = ("Right Pinky", Colors.Pink),
            ["\\"] = ("Right Pinky", Colors.Pink),
            
            // Thumbs
            [" "] = ("Thumbs", Colors.Gray)
        };
    }

    /// <summary>
    /// Highlight the key being typed
    /// </summary>
    public void HighlightKey(string key)
    {
        if (string.IsNullOrEmpty(key)) return;

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

    /// <summary>
    /// Clear the highlight
    /// </summary>
    public void ClearHighlight()
    {
        CurrentKeyBorder.IsVisible = false;
    }
}
