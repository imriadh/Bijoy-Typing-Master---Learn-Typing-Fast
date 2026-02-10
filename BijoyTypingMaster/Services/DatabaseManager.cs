using System.Data.SQLite;
using BijoyTypingMaster.Models;

namespace BijoyTypingMaster.Services;

public class DatabaseManager
{
    private readonly string _dbPath;
    private readonly string _connectionString;

    public DatabaseManager()
    {
        // Database will be created in the app's local data folder
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _dbPath = Path.Combine(appDataPath, "typing_master.db");
        _connectionString = $"Data Source={_dbPath};Version=3;";
    }

    public void InitializeDatabase()
    {
        try
        {
            // Create database file if it doesn't exist
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
                Console.WriteLine($"Database created at: {_dbPath}");
            }

            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            // Create Lessons table
            string createLessonsTable = @"
                CREATE TABLE IF NOT EXISTS Lessons (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Difficulty TEXT NOT NULL,
                    TextContent TEXT NOT NULL,
                    Type TEXT NOT NULL
                );";

            using (var command = new SQLiteCommand(createLessonsTable, connection))
            {
                command.ExecuteNonQuery();
            }

            // Create UserProgress table
            string createProgressTable = @"
                CREATE TABLE IF NOT EXISTS UserProgress (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    WPM REAL NOT NULL,
                    Accuracy REAL NOT NULL,
                    LessonId INTEGER NOT NULL,
                    FOREIGN KEY(LessonId) REFERENCES Lessons(Id)
                );";

            using (var command = new SQLiteCommand(createProgressTable, connection))
            {
                command.ExecuteNonQuery();
            }

            // Create SpeedTestResults table
            string createSpeedTestTable = @"
                CREATE TABLE IF NOT EXISTS SpeedTestResults (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    Duration INTEGER NOT NULL,
                    WPM REAL NOT NULL,
                    NetWPM REAL NOT NULL,
                    Accuracy REAL NOT NULL,
                    TotalCharacters INTEGER NOT NULL,
                    CorrectCharacters INTEGER NOT NULL,
                    ErrorCount INTEGER NOT NULL,
                    TestText TEXT NOT NULL
                );";

            using (var command = new SQLiteCommand(createSpeedTestTable, connection))
            {
                command.ExecuteNonQuery();
            }

            // Create UserProfile table
            string createUserProfileTable = @"
                CREATE TABLE IF NOT EXISTS UserProfile (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER DEFAULT 1,
                    TotalXP INTEGER DEFAULT 0,
                    CurrentLevel INTEGER DEFAULT 1,
                    JoinDate TEXT NOT NULL,
                    LastActive TEXT,
                    Streak INTEGER DEFAULT 0,
                    TotalLessonsCompleted INTEGER DEFAULT 0,
                    TotalTestsCompleted INTEGER DEFAULT 0,
                    TotalAchievementsUnlocked INTEGER DEFAULT 0,
                    TotalPracticeTimeMinutes INTEGER DEFAULT 0
                );";

            using (var command = new SQLiteCommand(createUserProfileTable, connection))
            {
                command.ExecuteNonQuery();
            }

            // Create XPHistory table
            string createXPHistoryTable = @"
                CREATE TABLE IF NOT EXISTS XPHistory (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER DEFAULT 1,
                    Date TEXT NOT NULL,
                    Amount INTEGER NOT NULL,
                    Source TEXT NOT NULL,
                    Description TEXT
                );";

            using (var command = new SQLiteCommand(createXPHistoryTable, connection))
            {
                command.ExecuteNonQuery();
            }

            // Create DailyChallenges table
            string createDailyChallengesTable = @"
                CREATE TABLE IF NOT EXISTS DailyChallenges (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    ChallengeType TEXT NOT NULL,
                    TargetText TEXT NOT NULL,
                    TargetWPM INTEGER NOT NULL,
                    TargetAccuracy INTEGER NOT NULL,
                    TimeLimit INTEGER NOT NULL,
                    IsCompleted INTEGER DEFAULT 0,
                    CompletedAt TEXT,
                    AchievedWPM INTEGER DEFAULT 0,
                    AchievedAccuracy INTEGER DEFAULT 0,
                    XPEarned INTEGER DEFAULT 0
                );";

            using (var command = new SQLiteCommand(createDailyChallengesTable, connection))
            {
                command.ExecuteNonQuery();
            }

            // Create UserAchievements table
            string createUserAchievementsTable = @"
                CREATE TABLE IF NOT EXISTS UserAchievements (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AchievementId INTEGER NOT NULL,
                    UnlockedAt TEXT NOT NULL,
                    Progress INTEGER DEFAULT 0
                );";

            using (var command = new SQLiteCommand(createUserAchievementsTable, connection))
            {
                command.ExecuteNonQuery();
            }

            // Create CustomTexts table
            string createCustomTextsTable = @"
                CREATE TABLE IF NOT EXISTS CustomTexts (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Text TEXT NOT NULL,
                    CreatedAt TEXT NOT NULL,
                    LastPracticed TEXT,
                    TimesCompleted INTEGER DEFAULT 0,
                    BestWPM INTEGER DEFAULT 0,
                    BestAccuracy INTEGER DEFAULT 0
                );";

            using (var command = new SQLiteCommand(createCustomTextsTable, connection))
            {
                command.ExecuteNonQuery();
            }

            // Create BookmarkedLessons table
            string createBookmarkedLessonsTable = @"
                CREATE TABLE IF NOT EXISTS BookmarkedLessons (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    LessonNumber INTEGER NOT NULL,
                    BookmarkedAt TEXT NOT NULL,
                    Notes TEXT
                );";

            using (var command = new SQLiteCommand(createBookmarkedLessonsTable, connection))
            {
                command.ExecuteNonQuery();
            }

            // Insert sample lessons if table is empty
            InsertSampleLessons(connection);

            Console.WriteLine("Database initialized successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database initialization error: {ex.Message}");
        }
    }

    private void InsertSampleLessons(SQLiteConnection connection)
    {
        // Check if lessons already exist
        string checkQuery = "SELECT COUNT(*) FROM Lessons";
        using var checkCommand = new SQLiteCommand(checkQuery, connection);
        long count = (long)checkCommand.ExecuteScalar()!;

        if (count > 0) return; // Lessons already exist

        // Insert sample Bijoy lessons
        var sampleLessons = new[]
        {
            new Lesson(0, "Bijoy Basics - Vowels", "Beginner", "অ আ ই ঈ উ ঊ এ ঐ ও ঔ", "Bijoy"),
            new Lesson(0, "Bijoy Basics - Consonants", "Beginner", "ক খ গ ঘ ঙ চ ছ জ ঝ ঞ", "Bijoy"),
            new Lesson(0, "Bijoy Conjuncts", "Intermediate", "ক্ক ক্ত ক্র গ্ন ঙ্গ", "Bijoy"),
            new Lesson(0, "Unicode Basics - Vowels", "Beginner", "অ আ ই ঈ উ ঊ এ ঐ ও ঔ", "Unicode"),
            new Lesson(0, "Unicode Basics - Consonants", "Beginner", "ক খ গ ঘ ঙ চ ছ জ ঝ ঞ", "Unicode")
        };

        foreach (var lesson in sampleLessons)
        {
            string insertQuery = @"
                INSERT INTO Lessons (Title, Difficulty, TextContent, Type)
                VALUES (@Title, @Difficulty, @TextContent, @Type)";

            using var command = new SQLiteCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@Title", lesson.Title);
            command.Parameters.AddWithValue("@Difficulty", lesson.Difficulty);
            command.Parameters.AddWithValue("@TextContent", lesson.TextContent);
            command.Parameters.AddWithValue("@Type", lesson.Type);
            command.ExecuteNonQuery();
        }
    }

    // Get all lessons by type
    public List<Lesson> GetLessonsByType(string type)
    {
        var lessons = new List<Lesson>();

        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string query = "SELECT * FROM Lessons WHERE Type = @Type ORDER BY Id";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Type", type);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            lessons.Add(new Lesson
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Difficulty = reader.GetString(2),
                TextContent = reader.GetString(3),
                Type = reader.GetString(4)
            });
        }

        return lessons;
    }

    // Get lesson by ID
    public Lesson? GetLessonById(int id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string query = "SELECT * FROM Lessons WHERE Id = @Id";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Lesson
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Difficulty = reader.GetString(2),
                TextContent = reader.GetString(3),
                Type = reader.GetString(4)
            };
        }

        return null;
    }

    // Save user progress
    public void SaveProgress(UserProgress progress)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string query = @"
            INSERT INTO UserProgress (Date, WPM, Accuracy, LessonId)
            VALUES (@Date, @WPM, @Accuracy, @LessonId)";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Date", progress.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@WPM", progress.WPM);
        command.Parameters.AddWithValue("@Accuracy", progress.Accuracy);
        command.Parameters.AddWithValue("@LessonId", progress.LessonId);

        command.ExecuteNonQuery();
    }

    // Get user progress history
    public List<UserProgress> GetProgressHistory(int limit = 50)
    {
        var progressList = new List<UserProgress>();

        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string query = "SELECT * FROM UserProgress ORDER BY Date DESC LIMIT @Limit";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            progressList.Add(new UserProgress
            {
                Id = reader.GetInt32(0),
                Date = DateTime.Parse(reader.GetString(1)),
                WPM = reader.GetDouble(2),
                Accuracy = reader.GetDouble(3),
                LessonId = reader.GetInt32(4)
            });
        }

        return progressList;
    }

    // Save speed test result
    public void SaveSpeedTestResult(SpeedTestResult result)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string query = @"
            INSERT INTO SpeedTestResults (Date, Duration, WPM, NetWPM, Accuracy, 
                                         TotalCharacters, CorrectCharacters, ErrorCount, TestText)
            VALUES (@Date, @Duration, @WPM, @NetWPM, @Accuracy, 
                    @TotalCharacters, @CorrectCharacters, @ErrorCount, @TestText)";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Date", result.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@Duration", result.Duration);
        command.Parameters.AddWithValue("@WPM", result.WPM);
        command.Parameters.AddWithValue("@NetWPM", result.NetWPM);
        command.Parameters.AddWithValue("@Accuracy", result.Accuracy);
        command.Parameters.AddWithValue("@TotalCharacters", result.TotalCharacters);
        command.Parameters.AddWithValue("@CorrectCharacters", result.CorrectCharacters);
        command.Parameters.AddWithValue("@ErrorCount", result.ErrorCount);
        command.Parameters.AddWithValue("@TestText", result.TestText);

        command.ExecuteNonQuery();
    }

    // Get speed test history
    public List<SpeedTestResult> GetSpeedTestHistory(int limit = 50)
    {
        var results = new List<SpeedTestResult>();

        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string query = "SELECT * FROM SpeedTestResults ORDER BY Date DESC LIMIT @Limit";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            results.Add(new SpeedTestResult
            {
                Id = reader.GetInt32(0),
                Date = DateTime.Parse(reader.GetString(1)),
                Duration = reader.GetInt32(2),
                WPM = reader.GetDouble(3),
                NetWPM = reader.GetDouble(4),
                Accuracy = reader.GetDouble(5),
                TotalCharacters = reader.GetInt32(6),
                CorrectCharacters = reader.GetInt32(7),
                ErrorCount = reader.GetInt32(8),
                TestText = reader.GetString(9)
            });
        }

        return results;
    }

    // Get best speed test result
    public SpeedTestResult? GetBestSpeedTestResult()
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string query = "SELECT * FROM SpeedTestResults ORDER BY WPM DESC, Accuracy DESC LIMIT 1";
        using var command = new SQLiteCommand(query, connection);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new SpeedTestResult
            {
                Id = reader.GetInt32(0),
                Date = DateTime.Parse(reader.GetString(1)),
                Duration = reader.GetInt32(2),
                WPM = reader.GetDouble(3),
                NetWPM = reader.GetDouble(4),
                Accuracy = reader.GetDouble(5),
                TotalCharacters = reader.GetInt32(6),
                CorrectCharacters = reader.GetInt32(7),
                ErrorCount = reader.GetInt32(8),
                TestText = reader.GetString(9)
            };
        }

        return null;
    }

    // Get average WPM and accuracy
    public (double avgWPM, double avgAccuracy) GetAverageStats(int days = 7)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        var cutoffDate = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");
        
        string query = @"
            SELECT AVG(WPM), AVG(Accuracy) 
            FROM UserProgress 
            WHERE Date >= @CutoffDate";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@CutoffDate", cutoffDate);

        using var reader = command.ExecuteReader();
        if (reader.Read() && !reader.IsDBNull(0))
        {
            return (reader.GetDouble(0), reader.GetDouble(1));
        }

        return (0, 0);
    }

    // Get progress over time for charts
    public List<(DateTime date, double wpm, double accuracy)> GetProgressOverTime(int days = 30)
    {
        var results = new List<(DateTime, double, double)>();

        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        var cutoffDate = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");

        string query = @"
            SELECT Date, AVG(WPM), AVG(Accuracy) 
            FROM UserProgress 
            WHERE Date >= @CutoffDate
            GROUP BY DATE(Date)
            ORDER BY Date ASC";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@CutoffDate", cutoffDate);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var date = DateTime.Parse(reader.GetString(0));
            var wpm = reader.GetDouble(1);
            var accuracy = reader.GetDouble(2);
            results.Add((date, wpm, accuracy));
        }

        return results;
    }

    // ===== XP SYSTEM METHODS =====

    // Create new user profile
    public async Task CreateUserProfileAsync(UserProfile profile)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = @"
            INSERT INTO UserProfile (UserId, TotalXP, CurrentLevel, JoinDate, LastActive, Streak,
                                   TotalLessonsCompleted, TotalTestsCompleted, TotalAchievementsUnlocked, TotalPracticeTimeMinutes)
            VALUES (@UserId, @TotalXP, @CurrentLevel, @JoinDate, @LastActive, @Streak,
                    @TotalLessonsCompleted, @TotalTestsCompleted, @TotalAchievementsUnlocked, @TotalPracticeTimeMinutes)";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", profile.UserId);
        command.Parameters.AddWithValue("@TotalXP", profile.TotalXP);
        command.Parameters.AddWithValue("@CurrentLevel", profile.CurrentLevel);
        command.Parameters.AddWithValue("@JoinDate", profile.JoinDate.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@LastActive", profile.LastActive.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@Streak", profile.Streak);
        command.Parameters.AddWithValue("@TotalLessonsCompleted", profile.TotalLessonsCompleted);
        command.Parameters.AddWithValue("@TotalTestsCompleted", profile.TotalTestsCompleted);
        command.Parameters.AddWithValue("@TotalAchievementsUnlocked", profile.TotalAchievementsUnlocked);
        command.Parameters.AddWithValue("@TotalPracticeTimeMinutes", profile.TotalPracticeTimeMinutes);

        await command.ExecuteNonQueryAsync();
    }

    // Get user profile
    public async Task<UserProfile?> GetUserProfileAsync(int userId = 1)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT * FROM UserProfile WHERE UserId = @UserId LIMIT 1";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", userId);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new UserProfile
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                TotalXP = reader.GetInt32(2),
                CurrentLevel = reader.GetInt32(3),
                JoinDate = DateTime.Parse(reader.GetString(4)),
                LastActive = reader.IsDBNull(5) ? DateTime.Now : DateTime.Parse(reader.GetString(5)),
                Streak = reader.GetInt32(6),
                TotalLessonsCompleted = reader.GetInt32(7),
                TotalTestsCompleted = reader.GetInt32(8),
                TotalAchievementsUnlocked = reader.GetInt32(9),
                TotalPracticeTimeMinutes = reader.GetInt32(10)
            };
        }

        return null;
    }

    // Update user profile
    public async Task UpdateUserProfileAsync(UserProfile profile)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = @"
            UPDATE UserProfile 
            SET TotalXP = @TotalXP,
                CurrentLevel = @CurrentLevel,
                LastActive = @LastActive,
                Streak = @Streak,
                TotalLessonsCompleted = @TotalLessonsCompleted,
                TotalTestsCompleted = @TotalTestsCompleted,
                TotalAchievementsUnlocked = @TotalAchievementsUnlocked,
                TotalPracticeTimeMinutes = @TotalPracticeTimeMinutes
            WHERE UserId = @UserId";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@TotalXP", profile.TotalXP);
        command.Parameters.AddWithValue("@CurrentLevel", profile.CurrentLevel);
        command.Parameters.AddWithValue("@LastActive", profile.LastActive.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@Streak", profile.Streak);
        command.Parameters.AddWithValue("@TotalLessonsCompleted", profile.TotalLessonsCompleted);
        command.Parameters.AddWithValue("@TotalTestsCompleted", profile.TotalTestsCompleted);
        command.Parameters.AddWithValue("@TotalAchievementsUnlocked", profile.TotalAchievementsUnlocked);
        command.Parameters.AddWithValue("@TotalPracticeTimeMinutes", profile.TotalPracticeTimeMinutes);
        command.Parameters.AddWithValue("@UserId", profile.UserId);

        await command.ExecuteNonQueryAsync();
    }

    // Add XP history entry
    public async Task AddXPHistoryAsync(XPHistory history)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = @"
            INSERT INTO XPHistory (UserId, Date, Amount, Source, Description)
            VALUES (@UserId, @Date, @Amount, @Source, @Description)";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", history.UserId);
        command.Parameters.AddWithValue("@Date", history.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@Amount", history.Amount);
        command.Parameters.AddWithValue("@Source", history.Source);
        command.Parameters.AddWithValue("@Description", history.Description ?? string.Empty);

        await command.ExecuteNonQueryAsync();
    }

    // Get XP history
    public async Task<List<XPHistory>> GetXPHistoryAsync(DateTime startDate, int userId = 1)
    {
        var history = new List<XPHistory>();

        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = @"
            SELECT * FROM XPHistory 
            WHERE UserId = @UserId AND Date >= @StartDate
            ORDER BY Date DESC";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd HH:mm:ss"));

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            history.Add(new XPHistory
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                Date = DateTime.Parse(reader.GetString(2)),
                Amount = reader.GetInt32(3),
                Source = reader.GetString(4),
                Description = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
            });
        }

        return history;
    }

    // ===== DAILY CHALLENGES METHODS =====

    // Save daily challenge
    public async Task SaveDailyChallengeAsync(DailyChallenge challenge)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = @"
            INSERT INTO DailyChallenges (Date, ChallengeType, TargetText, TargetWPM, TargetAccuracy, TimeLimit,
                                       IsCompleted, CompletedAt, AchievedWPM, AchievedAccuracy, XPEarned)
            VALUES (@Date, @ChallengeType, @TargetText, @TargetWPM, @TargetAccuracy, @TimeLimit,
                    @IsCompleted, @CompletedAt, @AchievedWPM, @AchievedAccuracy, @XPEarned)";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Date", challenge.Date.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@ChallengeType", challenge.ChallengeType.ToString());
        command.Parameters.AddWithValue("@TargetText", challenge.TargetText);
        command.Parameters.AddWithValue("@TargetWPM", challenge.TargetWPM);
        command.Parameters.AddWithValue("@TargetAccuracy", challenge.TargetAccuracy);
        command.Parameters.AddWithValue("@TimeLimit", challenge.TimeLimit);
        command.Parameters.AddWithValue("@IsCompleted", challenge.IsCompleted ? 1 : 0);
        command.Parameters.AddWithValue("@CompletedAt", challenge.CompletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@AchievedWPM", challenge.AchievedWPM);
        command.Parameters.AddWithValue("@AchievedAccuracy", challenge.AchievedAccuracy);
        command.Parameters.AddWithValue("@XPEarned", challenge.XPEarned);

        await command.ExecuteNonQueryAsync();
    }

    // Get daily challenge by date
    public async Task<DailyChallenge?> GetDailyChallengeAsync(DateTime date)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT * FROM DailyChallenges WHERE Date = @Date LIMIT 1";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new DailyChallenge
            {
                Id = reader.GetInt32(0),
                Date = DateTime.Parse(reader.GetString(1)),
                ChallengeType = Enum.Parse<ChallengeType>(reader.GetString(2)),
                TargetText = reader.GetString(3),
                TargetWPM = reader.GetInt32(4),
                TargetAccuracy = reader.GetInt32(5),
                TimeLimit = reader.GetInt32(6),
                IsCompleted = reader.GetInt32(7) == 1,
                CompletedAt = reader.IsDBNull(8) ? null : DateTime.Parse(reader.GetString(8)),
                AchievedWPM = reader.GetInt32(9),
                AchievedAccuracy = reader.GetInt32(10),
                XPEarned = reader.GetInt32(11)
            };
        }

        return null;
    }

    // Update daily challenge
    public async Task UpdateDailyChallengeAsync(DailyChallenge challenge)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = @"
            UPDATE DailyChallenges
            SET IsCompleted = @IsCompleted,
                CompletedAt = @CompletedAt,
                AchievedWPM = @AchievedWPM,
                AchievedAccuracy = @AchievedAccuracy,
                XPEarned = @XPEarned
            WHERE Id = @Id";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@IsCompleted", challenge.IsCompleted ? 1 : 0);
        command.Parameters.AddWithValue("@CompletedAt", challenge.CompletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@AchievedWPM", challenge.AchievedWPM);
        command.Parameters.AddWithValue("@AchievedAccuracy", challenge.AchievedAccuracy);
        command.Parameters.AddWithValue("@XPEarned", challenge.XPEarned);
        command.Parameters.AddWithValue("@Id", challenge.Id);

        await command.ExecuteNonQueryAsync();
    }

    // Get daily challenge history
    public async Task<List<DailyChallenge>> GetDailyChallengeHistoryAsync(int days)
    {
        var challenges = new List<DailyChallenge>();

        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        var startDate = DateTime.Today.AddDays(-days);
        string query = @"
            SELECT * FROM DailyChallenges
            WHERE Date >= @StartDate
            ORDER BY Date DESC";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            challenges.Add(new DailyChallenge
            {
                Id = reader.GetInt32(0),
                Date = DateTime.Parse(reader.GetString(1)),
                ChallengeType = Enum.Parse<ChallengeType>(reader.GetString(2)),
                TargetText = reader.GetString(3),
                TargetWPM = reader.GetInt32(4),
                TargetAccuracy = reader.GetInt32(5),
                TimeLimit = reader.GetInt32(6),
                IsCompleted = reader.GetInt32(7) == 1,
                CompletedAt = reader.IsDBNull(8) ? null : DateTime.Parse(reader.GetString(8)),
                AchievedWPM = reader.GetInt32(9),
                AchievedAccuracy = reader.GetInt32(10),
                XPEarned = reader.GetInt32(11)
            });
        }

        return challenges;
    }

    // ===== ACHIEVEMENTS METHODS =====

    // Unlock achievement for user
    public async Task UnlockAchievementAsync(int achievementId)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = @"
            INSERT INTO UserAchievements (AchievementId, UnlockedAt, Progress)
            VALUES (@AchievementId, @UnlockedAt, 0)";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@AchievementId", achievementId);
        command.Parameters.AddWithValue("@UnlockedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        await command.ExecuteNonQueryAsync();
    }

    // Get user's unlocked achievements
    public async Task<List<Achievement>> GetUserAchievementsAsync()
    {
        var achievements = new List<Achievement>();

        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT * FROM UserAchievements ORDER BY UnlockedAt DESC";
        using var command = new SQLiteCommand(query, connection);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            achievements.Add(new Achievement
            {
                Id = reader.GetInt32(1), // AchievementId
                IsUnlocked = true,
                UnlockedAt = DateTime.Parse(reader.GetString(2)),
                Progress = reader.GetInt32(3)
            });
        }

        return achievements;
    }

    // ===== CUSTOM TEXT PRACTICE METHODS =====

    // Save custom text
    public async Task<int> SaveCustomTextAsync(string title, string text)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = @"
            INSERT INTO CustomTexts (Title, Text, CreatedAt)
            VALUES (@Title, @Text, @CreatedAt);
            SELECT last_insert_rowid();";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Title", title);
        command.Parameters.AddWithValue("@Text", text);
        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    // Get all custom texts
    public async Task<List<CustomPracticeSession>> GetCustomTextsAsync()
    {
        var texts = new List<CustomPracticeSession>();

        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT * FROM CustomTexts ORDER BY CreatedAt DESC";
        using var command = new SQLiteCommand(query, connection);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            texts.Add(new CustomPracticeSession
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                CustomText = reader.GetString(2),
                CreatedAt = DateTime.Parse(reader.GetString(3)),
                LastPracticed = reader.IsDBNull(4) ? null : DateTime.Parse(reader.GetString(4)),
                TimesCompleted = reader.GetInt32(5),
                BestWPM = reader.GetInt32(6),
                BestAccuracy = reader.GetInt32(7)
            });
        }

        return texts;
    }

    // Update custom text stats
    public async Task UpdateCustomTextStatsAsync(int id, int wpm, int accuracy)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = @"
            UPDATE CustomTexts
            SET LastPracticed = @LastPracticed,
                TimesCompleted = TimesCompleted + 1,
                BestWPM = CASE WHEN @WPM > BestWPM THEN @WPM ELSE BestWPM END,
                BestAccuracy = CASE WHEN @Accuracy > BestAccuracy THEN @Accuracy ELSE BestAccuracy END
            WHERE Id = @Id";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@LastPracticed", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@WPM", wpm);
        command.Parameters.AddWithValue("@Accuracy", accuracy);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }

    // Delete custom text
    public async Task DeleteCustomTextAsync(int id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = "DELETE FROM CustomTexts WHERE Id = @Id";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        await command.ExecuteNonQueryAsync();
    }

    // ===== LESSON BOOKMARKS METHODS =====

    // Bookmark a lesson
    public async Task BookmarkLessonAsync(int lessonNumber, string notes = "")
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = @"
            INSERT INTO BookmarkedLessons (LessonNumber, BookmarkedAt, Notes)
            VALUES (@LessonNumber, @BookmarkedAt, @Notes)";

        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@LessonNumber", lessonNumber);
        command.Parameters.AddWithValue("@BookmarkedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@Notes", notes);

        await command.ExecuteNonQueryAsync();
    }

    // Remove bookmark
    public async Task UnbookmarkLessonAsync(int lessonNumber)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = "DELETE FROM BookmarkedLessons WHERE LessonNumber = @LessonNumber";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@LessonNumber", lessonNumber);

        await command.ExecuteNonQueryAsync();
    }

    // Get bookmarked lessons
    public async Task<List<int>> GetBookmarkedLessonsAsync()
    {
        var bookmarks = new List<int>();

        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT LessonNumber FROM BookmarkedLessons ORDER BY BookmarkedAt DESC";
        using var command = new SQLiteCommand(query, connection);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            bookmarks.Add(reader.GetInt32(0));
        }

        return bookmarks;
    }

    // Check if lesson is bookmarked
    public async Task<bool> IsLessonBookmarkedAsync(int lessonNumber)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();

        string query = "SELECT COUNT(*) FROM BookmarkedLessons WHERE LessonNumber = @LessonNumber";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@LessonNumber", lessonNumber);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }
}
