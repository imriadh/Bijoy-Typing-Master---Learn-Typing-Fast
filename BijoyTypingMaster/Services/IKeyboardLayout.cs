namespace BijoyTypingMaster.Services;

/// <summary>
/// Interface for keyboard layout implementations (Bijoy and Unicode)
/// </summary>
public interface IKeyboardLayout
{
    /// <summary>
    /// Convert physical key to Bengali character based on the layout
    /// </summary>
    string ProcessKey(string key, string buffer);

    /// <summary>
    /// Layout name (Bijoy or Unicode)
    /// </summary>
    string LayoutName { get; }

    /// <summary>
    /// Check if the key requires special handling (like conjuncts)
    /// </summary>
    bool RequiresBuffer(string key);
}
