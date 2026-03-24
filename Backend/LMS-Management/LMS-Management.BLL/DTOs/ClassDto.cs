namespace LMS_Management.BLL.DTOs;

public class CreateClassDto
{
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string DayOfWeek { get; set; } = string.Empty;
    public string TimeSlot { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public int MaxStudents { get; set; }
}

public class ClassDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string DayOfWeek { get; set; } = string.Empty;
    public string TimeSlot { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public int MaxStudents { get; set; }
    public int CurrentStudents { get; set; }
}
