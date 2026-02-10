using System.Text;

namespace BijoyTypingMaster.Services;

/// <summary>
/// Bijoy keyboard layout with complex conjunct (Juktakkhor) handling
/// Based on the Bijoy PDF rules
/// </summary>
public class BijoyLayout : IKeyboardLayout
{
    public string LayoutName => "Bijoy";

    // Basic key mappings for Bijoy
    private readonly Dictionary<string, string> _basicKeyMap = new()
    {
        // Vowels
        { "a", "া" }, { "A", "অ" },
        { "i", "ি" }, { "I", "ই" },
        { "u", "ু" }, { "U", "উ" },
        { "e", "ে" }, { "E", "এ" },
        { "o", "ো" }, { "O", "ও" },
        { "y", "ৈ" }, { "Y", "ঐ" },
        { "ow", "ৌ" }, { "OI", "ঔ" },
        { "x", "ং" }, { ":", "ঃ" },
        { "^", "ঁ" },

        // Consonants (SutonnyMJ font mapping)
        { "k", "ক" }, { "K", "খ" },
        { "g", "গ" }, { "G", "ঘ" },
        { "q", "ঙ" },
        { "Q", "চ" }, { "OI", "ছ" },
        { "j", "জ" }, { "J", "ঝ" },
        { "NG", "ঞ" },
        { "T", "ট" }, { "Tn", "ঠ" },
        { "D", "ড" }, { "Dn", "ঢ" },
        { "Y", "ণ" },
        { "t", "ত" }, { "tn", "থ" },
        { "d", "দ" }, { "dn", "ধ" },
        { "n", "ন" },
        { "p", "প" }, { "f", "ফ" },
        { "b", "ব" }, { "v", "ভ" },
        { "m", "ম" },
        { "z", "য" }, { "Z", "য়" },
        { "r", "র" }, { "R", "ড়" },
        { "l", "ল" },
        { "S", "শ" }, { "sh", "ষ" },
        { "s", "স" },
        { "h", "হ" },
        { "H", "্" }, // Hasanta (for conjuncts)

        // Special conjunct marker
        { "&", "্" }, // Used for creating conjuncts (e.g., ক& + ক = ক্ক)

        // Numbers
        { "0", "০" }, { "1", "১" }, { "2", "২" },
        { "3", "৩" }, { "4", "৪" }, { "5", "৫" },
        { "6", "৬" }, { "7", "৭" }, { "8", "৮" }, { "9", "৯" }
    };

    // Common conjuncts (Juktakkhor) - pre-defined combinations
    private readonly Dictionary<string, string> _conjunctMap = new()
    {
        // ক series
        { "ক্ক", "ক্ক" }, { "ক্ত", "ক্ত" }, { "ক্র", "ক্র" },
        { "ক্ল", "ক্ল" }, { "ক্ষ", "ক্ষ" }, { "ক্ম", "ক্ম" },
        
        // গ series
        { "গ্ন", "গ্ন" }, { "গ্ল", "গ্ল" }, { "গ্র", "গ্র" },
        
        // ঙ series
        { "ঙ্ক", "ঙ্ক" }, { "ঙ্গ", "ঙ্গ" },
        
        // চ series
        { "চ্চ", "চ্চ" }, { "চ্ছ", "চ্ছ" }, { "চ্য", "চ্য" },
        
        // জ series
        { "জ্জ", "জ্জ" }, { "জ্ঝ", "জ্ঝ" }, { "জ্ঞ", "জ্ঞ" },
        
        // ট series
        { "ট্ট", "ট্ট" }, { "ট্র", "ট্র" },
        
        // ড series
        { "ড্ড", "ড্ড" }, { "ড্র", "ড্র" },
        
        // ণ series
        { "ণ্ড", "ণ্ড" }, { "ণ্ঢ", "ণ্ঢ" },
        
        // ত series
        { "ত্ত", "ত্ত" }, { "ত্থ", "ত্থ" }, { "ত্ন", "ত্ন" },
        { "ত্ম", "ত্ম" }, { "ত্র", "ত্র" },
        
        // দ series
        { "দ্দ", "দ্দ" }, { "দ্ধ", "দ্ধ" }, { "দ্ভ", "দ্ভ" },
        { "দ্ম", "দ্ম" }, { "দ্র", "দ্র" },
        
        // ন series
        { "ন্ট", "ন্ট" }, { "ন্ঠ", "ন্ঠ" }, { "ন্ড", "ন্ড" },
        { "ন্ত", "ন্ত" }, { "ন্থ", "ন্থ" }, { "ন্দ", "ন্দ" },
        { "ন্ধ", "ন্ধ" }, { "ন্ন", "ন্ন" }, { "ন্ম", "ন্ম" },
        
        // প series
        { "প্প", "প্প" }, { "প্ট", "প্ট" }, { "প্ত", "প্ত" },
        { "প্ন", "প্ন" }, { "প্র", "প্র" }, { "প্ল", "প্ল" },
        
        // ব series
        { "ব্জ", "ব্জ" }, { "ব্দ", "ব্দ" }, { "ব্ধ", "ব্ধ" },
        { "ব্ব", "ব্ব" }, { "ব্র", "ব্র" }, { "ব্ল", "ব্ল" },
        
        // ম series
        { "ম্প", "ম্প" }, { "ম্ফ", "ম্ফ" }, { "ম্ব", "ম্ব" },
        { "ম্ভ", "ম্ভ" }, { "ম্ম", "ম্ম" }, { "ম্ল", "ম্ল" },
        
        // ল series
        { "ল্ক", "ল্ক" }, { "ল্গ", "ল্গ" }, { "ল্প", "ল্প" },
        { "ল্ফ", "ল্ফ" }, { "ল্ব", "ল্ব" }, { "ল্ম", "ল্ম" },
        { "ল্ল", "ল্ল" },
        
        // শ series
        { "শ্চ", "শ্চ" }, { "শ্ছ", "শ্ছ" }, { "শ্ন", "শ্ন" },
        { "শ্ম", "শ্ম" }, { "শ্ল", "শ্ল" },
        
        // ষ series
        { "ষ্ক", "ষ্ক" }, { "ষ্ট", "ষ্ট" }, { "ষ্ঠ", "ষ্ঠ" },
        { "ষ্ণ", "ষ্ণ" }, { "ষ্প", "ষ্প" }, { "ষ্ম", "ষ্ম" },
        
        // স series
        { "স্ক", "স্ক" }, { "স্খ", "স্খ" }, { "স্ট", "স্ট" },
        { "স্ত", "স্ত" }, { "স্থ", "স্থ" }, { "স্ন", "স্ন" },
        { "স্প", "স্প" }, { "স্ফ", "স্ফ" }, { "স্ম", "স্ম" },
        { "স্ল", "স্ল" },
        
        // হ series
        { "হ্ণ", "হ্ণ" }, { "হ্ন", "হ্ন" }, { "হ্ম", "হ্ম" },
        { "হ্ল", "হ্ল" }
    };

    public string ProcessKey(string key, string buffer)
    {
        // Handle conjunct formation with & symbol
        // Example: k (ক) + & + k (ক) = ক্ক
        if (key == "&" || buffer.EndsWith("্"))
        {
            return "্"; // Return hasanta for conjunct building
        }

        // Check if buffer contains a pending conjunct
        if (buffer.Length > 0 && buffer.Contains("্"))
        {
            // Try to form a conjunct
            string potentialConjunct = buffer + ProcessBasicKey(key);
            
            if (_conjunctMap.ContainsKey(potentialConjunct))
            {
                return _conjunctMap[potentialConjunct];
            }
            
            // If no predefined conjunct, return hasanta + character
            return "্" + ProcessBasicKey(key);
        }

        // Handle # key for special vowel combinations
        if (key == "#")
        {
            // # can be used for র্য, র্্য combinations
            return "্র";
        }

        return ProcessBasicKey(key);
    }

    private string ProcessBasicKey(string key)
    {
        if (_basicKeyMap.ContainsKey(key))
        {
            return _basicKeyMap[key];
        }

        // Return the key as-is if not mapped
        return key;
    }

    public bool RequiresBuffer(string key)
    {
        // These keys require buffer handling for conjuncts
        return key == "&" || key == "#" || key == "H";
    }

    /// <summary>
    /// Create a conjunct from two characters
    /// Example: CreateConjunct("ক", "ক") returns "ক্ক"
    /// </summary>
    public string CreateConjunct(string char1, string char2)
    {
        string conjunct = char1 + "্" + char2;
        
        if (_conjunctMap.ContainsKey(conjunct))
        {
            return _conjunctMap[conjunct];
        }
        
        // Return with hasanta if not in predefined map
        return conjunct;
    }
}
