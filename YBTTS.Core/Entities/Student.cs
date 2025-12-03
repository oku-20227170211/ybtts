namespace YBTTS.Core.Entities;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Points { get; set; } = 0;
    public int Level { get; set; } = 1;
    public ICollection<Request> Requests { get; set; } = new List<Request>();
}
