using BijoyTypingMaster.Models;

namespace BijoyTypingMaster.Services;

/// <summary>
/// Handles speed test logic and timing
/// </summary>
public class SpeedTestEngine
{
    private DateTime _startTime;
    private DateTime _endTime;
    private string _testText = string.Empty;
    private string _typedText = string.Empty;
    private int _duration; // in seconds
    private bool _isRunning = false;
    private bool _isComplete = false;
    private readonly IKeyboardLayout _layout;

    public string TestText => _testText;
    public string TypedText => _typedText;
    public bool IsRunning => _isRunning;
    public bool IsComplete => _isComplete;
    public int RemainingSeconds => Math.Max(0, _duration - ElapsedSeconds);
    public int ElapsedSeconds => _isRunning ? (int)(DateTime.Now - _startTime).TotalSeconds : 0;

    public SpeedTestEngine(IKeyboardLayout layout)
    {
        _layout = layout;
    }

    /// <summary>
    /// Start a new speed test
    /// </summary>
    public void StartTest(string testText, int durationSeconds)
    {
        _testText = testText;
        _duration = durationSeconds;
        _typedText = string.Empty;
        _startTime = DateTime.Now;
        _isRunning = true;
        _isComplete = false;
    }

    /// <summary>
    /// Process a keystroke
    /// </summary>
    public void ProcessKey(string key)
    {
        if (!_isRunning || _isComplete) return;

        // Process the key through the layout
        string output = _layout.ProcessKey(key);
        
        if (!string.IsNullOrEmpty(output))
        {
            _typedText += output;
        }

        // Check if time is up or text is complete
        if (ElapsedSeconds >= _duration || _typedText.Length >= _testText.Length)
        {
            CompleteTest();
        }
    }

    /// <summary>
    /// Process backspace
    /// </summary>
    public void ProcessBackspace()
    {
        if (!_isRunning || _isComplete || _typedText.Length == 0) return;

        _typedText = _typedText[..^1];
    }

    /// <summary>
    /// Complete the test
    /// </summary>
    public void CompleteTest()
    {
        if (!_isRunning) return;

        _endTime = DateTime.Now;
        _isRunning = false;
        _isComplete = true;
    }

    /// <summary>
    /// Generate speed test result
    /// </summary>
    public SpeedTestResult GetResult()
    {
        if (!_isComplete)
        {
            CompleteTest();
        }

        int correctChars = 0;
        int totalChars = _typedText.Length;
        int errors = 0;

        // Compare typed text with test text
        int compareLength = Math.Min(_typedText.Length, _testText.Length);
        for (int i = 0; i < compareLength; i++)
        {
            if (_typedText[i] == _testText[i])
            {
                correctChars++;
            }
            else
            {
                errors++;
            }
        }

        // Calculate metrics
        double minutes = (_endTime - _startTime).TotalMinutes;
        double wpm = (totalChars / 5.0) / minutes; // Standard WPM calculation
        double netWPM = ((totalChars - errors) / 5.0) / minutes;
        double accuracy = totalChars > 0 ? (correctChars * 100.0) / totalChars : 0;

        return new SpeedTestResult
        {
            Date = DateTime.Now,
            Duration = _duration,
            WPM = Math.Round(wpm, 2),
            NetWPM = Math.Round(netWPM, 2),
            Accuracy = Math.Round(accuracy, 2),
            TotalCharacters = totalChars,
            CorrectCharacters = correctChars,
            ErrorCount = errors,
            TestText = _testText
        };
    }

    /// <summary>
    /// Get current progress percentage
    /// </summary>
    public double GetProgress()
    {
        if (_testText.Length == 0) return 0;
        return (_typedText.Length * 100.0) / _testText.Length;
    }

    /// <summary>
    /// Reset the test
    /// </summary>
    public void Reset()
    {
        _testText = string.Empty;
        _typedText = string.Empty;
        _isRunning = false;
        _isComplete = false;
        _duration = 0;
    }

    /// <summary>
    /// Generate random test text in Bangla
    /// </summary>
    public static string GenerateRandomTestText(int wordCount = 50)
    {
        var words = new[]
        {
            "আমি", "তুমি", "সে", "আমরা", "তোমরা", "তারা",
            "করা", "যাওয়া", "আসা", "খাওয়া", "পড়া", "লেখা",
            "দেখা", "শোনা", "বলা", "চলা", "থাকা", "হওয়া",
            "মানুষ", "ঘর", "বাড়ি", "খাবার", "পানি", "কাজ",
            "সময়", "আজ", "কাল", "গতকাল", "পরশু", "দিন",
            "রাত", "সকাল", "দুপুর", "সন্ধ্যা", "ভালো", "খারাপ",
            "বড়", "ছোট", "নতুন", "পুরাতন", "সুন্দর", "কঠিন",
            "সহজ", "দ্রুত", "ধীর", "উপরে", "নিচে", "ভেতরে",
            "বাইরে", "এখানে", "সেখানে", "যেখানে", "কোথায়", "কখন",
            "কেন", "কিভাবে", "কিসের", "কার", "কাকে", "কাছে",
            "দূরে", "সাথে", "ছাড়া", "জন্য", "মতো", "হতে",
            "দেশ", "শহর", "গ্রাম", "বাজার", "স্কুল", "কলেজ",
            "বিশ্ববিদ্যালয়", "হাসপাতাল", "অফিস", "রাস্তা", "নদী", "পাহাড়",
            "সমুদ্র", "আকাশ", "সূর্য", "চাঁদ", "তারা", "বৃষ্টি"
        };

        var random = new Random();
        var result = new List<string>();

        for (int i = 0; i < wordCount; i++)
        {
            result.Add(words[random.Next(words.Length)]);
        }

        return string.Join(" ", result) + "।";
    }
}
