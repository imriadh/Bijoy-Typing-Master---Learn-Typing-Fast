namespace BijoyTypingMaster.Models;

public class UserProgress
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double WPM { get; set; } // Words Per Minute
    public double Accuracy { get; set; } // Percentage (0-100)
    public int LessonId { get; set; }

    public UserProgress() 
    {
        Date = DateTime.Now;
    }

    public UserProgress(int id, DateTime date, double wpm, double accuracy, int lessonId)
    {
        Id = id;
        Date = date;
        WPM = wpm;
        Accuracy = accuracy;
        LessonId = lessonId;
    }

    public override string ToString()
    {
        return $"Date: {Date:yyyy-MM-dd}, WPM: {WPM:F2}, Accuracy: {Accuracy:F2}%, Lesson: {LessonId}";
    }
}
