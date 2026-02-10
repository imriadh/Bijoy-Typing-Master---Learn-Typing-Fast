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
}
