using BijoyTypingMaster.Models;

namespace BijoyTypingMaster.Services;

/// <summary>
/// Manages daily typing challenges
/// </summary>
public class DailyChallengeManager
{
    private readonly DatabaseManager _dbManager;
    private readonly Random _random = new Random();

    // Sample Bangla texts for challenges
    private readonly string[] _challengeTexts = new[]
    {
        "আমি বাংলায় গান গাই। আমার এই দেশের মাটি যেমন সহজ সরল তেমনি তার ভাষা।",
        "একুশে ফেব্রুয়ারি আমাদের জাতীয় শহীদ দিবস। এই দিনে আমরা শহীদদের স্মরণ করি।",
        "বাংলাদেশ দক্ষিণ এশিয়ার একটি সার্বভৌম রাষ্ট্র। এর রাজধানী ঢাকা।",
        "রবীন্দ্রনাথ ঠাকুর বাংলা সাহিত্যের এক অন্যতম কবি। তিনি নোবেল পুরস্কার পেয়েছিলেন।",
        "শীতকাল বাংলাদেশের মানুষের কাছে অনেক প্রিয় একটি ঋতু। এই সময় পিঠা খাওয়ার ধুম পড়ে যায়।",
        "কম্পিউটারে টাইপিং শেখা আজকাল অত্যন্ত প্রয়োজনীয় একটি দক্ষতা। এটি সবার শেখা উচিত।",
        "বাংলা ভাষায় টাইপ করতে পারলে অনেক সুবিধা হয়। অফিসের কাজেও এটি দরকার।",
        "যত্ন সহকারে অনুশীলন করলে টাইপিং গতি অনেক বাড়ানো সম্ভব। নিয়মিত চর্চা করতে হবে।"
    };

    public DailyChallengeManager(DatabaseManager dbManager)
    {
        _dbManager = dbManager;
    }

    /// <summary>
    /// Generates today's daily challenge
    /// </summary>
    public async Task<DailyChallenge> GenerateTodayChallengeAsync()
    {
        var today = DateTime.Today;
        
        // Check if challenge already exists for today
        var existing = await _dbManager.GetDailyChallengeAsync(today);
        if (existing != null)
        {
            return existing;
        }

        // Generate new challenge
        var challengeType = (ChallengeType)_random.Next(0, 4);
        var challenge = new DailyChallenge
        {
            Date = today,
            ChallengeType = challengeType,
            TargetText = _challengeTexts[_random.Next(_challengeTexts.Length)],
            IsCompleted = false
        };

        // Set targets based on type
        switch (challengeType)
        {
            case ChallengeType.SpeedChallenge:
                challenge.TargetWPM = _random.Next(30, 70);
                challenge.TargetAccuracy = 90;
                challenge.TimeLimit = 60;
                break;

            case ChallengeType.AccuracyChallenge:
                challenge.TargetWPM = 20;
                challenge.TargetAccuracy = _random.Next(95, 100);
                challenge.TimeLimit = 120;
                break;

            case ChallengeType.ComboChallenge:
                challenge.TargetWPM = _random.Next(40, 60);
                challenge.TargetAccuracy = _random.Next(95, 98);
                challenge.TimeLimit = 90;
                break;

            case ChallengeType.EnduranceChallenge:
                challenge.TargetWPM = 25;
                challenge.TargetAccuracy = 92;
                challenge.TimeLimit = 180; // 3 minutes
                break;
        }

        // Save to database
        await _dbManager.SaveDailyChallengeAsync(challenge);
        return challenge;
    }

    /// <summary>
    /// Gets today's challenge
    /// </summary>
    public async Task<DailyChallenge> GetTodayChallengeAsync()
    {
        var today = DateTime.Today;
        var challenge = await _dbManager.GetDailyChallengeAsync(today);
        
        if (challenge == null)
        {
            challenge = await GenerateTodayChallengeAsync();
        }

        return challenge;
    }

    /// <summary>
    /// Completes today's challenge with results
    /// </summary>
    public async Task<(bool success, int xpEarned)> CompleteChallengeAsync(int wpm, int accuracy)
    {
        var challenge = await GetTodayChallengeAsync();

        if (challenge.IsCompleted)
        {
            return (false, 0); // Already completed
        }

        // Update challenge results
        challenge.IsCompleted = true;
        challenge.CompletedAt = DateTime.Now;
        challenge.AchievedWPM = wpm;
        challenge.AchievedAccuracy = accuracy;

        // Calculate XP
        bool requirementMet = challenge.IsRequirementMet();
        challenge.XPEarned = requirementMet ? challenge.CalculateXPReward() : 0;

        // Save challenge result
        await _dbManager.UpdateDailyChallengeAsync(challenge);

        return (requirementMet, challenge.XPEarned);
    }

    /// <summary>
    /// Gets challenge streak (consecutive days completed)
    /// </summary>
    public async Task<int> GetChallengeStreakAsync()
    {
        var history = await _dbManager.GetDailyChallengeHistoryAsync(365);
        
        int streak = 0;
        var checkDate = DateTime.Today;

        foreach (var challenge in history.OrderByDescending(c => c.Date))
        {
            if (challenge.Date.Date == checkDate.Date && challenge.IsCompleted)
            {
                streak++;
                checkDate = checkDate.AddDays(-1);
            }
            else if (challenge.Date.Date < checkDate.Date)
            {
                break; // Streak broken
            }
        }

        return streak;
    }

    /// <summary>
    /// Gets challenge history for specified days
    /// </summary>
    public async Task<List<DailyChallenge>> GetChallengeHistoryAsync(int days = 30)
    {
        return await _dbManager.GetDailyChallengeHistoryAsync(days);
    }

    /// <summary>
    /// Gets challenge statistics
    /// </summary>
    public async Task<(int total, int completed, int avgXP)> GetChallengeStatsAsync()
    {
        var history = await _dbManager.GetDailyChallengeHistoryAsync(365);
        
        int total = history.Count;
        int completed = history.Count(c => c.IsCompleted);
        int avgXP = completed > 0 ? (int)history.Where(c => c.IsCompleted).Average(c => c.XPEarned) : 0;

        return (total, completed, avgXP);
    }
}
