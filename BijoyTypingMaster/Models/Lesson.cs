namespace BijoyTypingMaster.Models;

public class Lesson
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Difficulty { get; set; } = "Beginner"; // Beginner, Intermediate, Advanced
    public string TextContent { get; set; } = string.Empty;
    public string Type { get; set; } = "Bijoy"; // Bijoy or Unicode

    public Lesson() { }

    public Lesson(int id, string title, string difficulty, string textContent, string type)
    {
        Id = id;
        Title = title;
        Difficulty = difficulty;
        TextContent = textContent;
        Type = type;
    }
}
