namespace LMS_Management.BLL.DTOs;

public class CreateStudentDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime Dob { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string CurrentGrade { get; set; } = string.Empty;
    public string ParentId { get; set; } = string.Empty;
}

public class StudentDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime Dob { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string CurrentGrade { get; set; } = string.Empty;
    public ParentDto? Parent { get; set; }
}
