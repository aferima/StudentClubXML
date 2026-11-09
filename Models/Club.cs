namespace StudentClubsXml.Models;

public class Club
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Faculty { get; set; } = "";
    public string Department { get; set; } = "";
    public string? Type { get; set; }
    public string? Course { get; set; }
    public string? Topic { get; set; }
    public string Day { get; set; } = "";
    public string Time { get; set; } = "";
    public string Starosta { get; set; } = "";
    public string Leader { get; set; } = "";

    public string TypeDisplay => string.IsNullOrWhiteSpace(Type) ? "—" : Type;
    public string CourseDisplay => string.IsNullOrWhiteSpace(Course) ? "—" : Course;
    public string TopicDisplay => string.IsNullOrWhiteSpace(Topic) ? "—" : Topic;
}
