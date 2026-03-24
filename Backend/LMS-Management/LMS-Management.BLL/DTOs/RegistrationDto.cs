namespace LMS_Management.BLL.DTOs;

public class RegisterStudentDto
{
    public string StudentId { get; set; } = string.Empty;
    public string SubscriptionId { get; set; } = string.Empty;
    public DateTime? NextSessionAt { get; set; }
}

public class RegistrationDto
{
    public string Id { get; set; } = string.Empty;
    public string ClassId { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public DateTime? NextSessionAt { get; set; }
}
