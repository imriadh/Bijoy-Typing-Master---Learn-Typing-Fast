namespace BijoyTypingMaster.Services;

/// <summary>
/// Simple Unicode Bengali keyboard layout
/// Direct key-to-character mapping
/// </summary>
public class UnicodeLayout : IKeyboardLayout
{
    public string LayoutName => "Unicode";

    private readonly Dictionary<string, string> _keyMap = new()
    {
        // Vowels
        { "a", "া" }, { "A", "অ" },
        { "i", "ি" }, { "I", "ঈ" },
        { "u", "ু" }, { "U", "ঊ" },
        { "e", "ে" }, { "E", "এ" },
        { "o", "ো" }, { "O", "ও" },
        { "x", "ং" }, { "X", "ঃ" },
        { "^", "ঁ" },

        // Consonants
        { "k", "ক" }, { "K", "খ" },
        { "g", "গ" }, { "G", "ঘ" },
        { "q", "ঙ" },
        { "c", "চ" }, { "C", "ছ" },
        { "j", "জ" }, { "J", "ঝ" },
        { "z", "ঞ" },
        { "T", "ট" }, { "Th", "ঠ" },
        { "D", "ড" }, { "Dh", "ঢ" },
        { "N", "ণ" },
        { "t", "ত" }, { "th", "থ" },
        { "d", "দ" }, { "dh", "ধ" },
        { "n", "ন" },
        { "p", "প" }, { "P", "ফ" },
        { "b", "ব" }, { "B", "ভ" },
        { "m", "ম" },
        { "Z", "য" }, { "y", "য়" },
        { "r", "র" }, { "R", "ড়" },
        { "l", "ল" },
        { "sh", "শ" }, { "Sh", "ষ" },
        { "s", "স" }, { "S", "স" },
        { "h", "হ" },
        { "H", "্" }, // Hasanta/Halant

        // Numbers (Bengali)
        { "0", "০" }, { "1", "১" }, { "2", "২" },
        { "3", "৩" }, { "4", "৪" }, { "5", "৫" },
        { "6", "৬" }, { "7", "৭" }, { "8", "৮" }, { "9", "৯" }
    };

    public string ProcessKey(string key, string buffer)
    {
        // Simple direct mapping for Unicode
        if (_keyMap.ContainsKey(key))
        {
            return _keyMap[key];
        }

        // Return the key as-is if not mapped (for spaces, punctuation, etc.)
        return key;
    }

    public bool RequiresBuffer(string key)
    {
        // Unicode doesn't need complex buffer handling
        return false;
    }
}
