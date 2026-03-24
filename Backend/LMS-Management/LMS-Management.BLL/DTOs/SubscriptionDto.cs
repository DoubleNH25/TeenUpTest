namespace LMS_Management.BLL.DTOs;

public class CreateSubscriptionDto
{
    public string StudentId { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalSessions { get; set; }
}

public class SubscriptionDto
{
    public string Id { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string PackageName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalSessions { get; set; }
    public int UsedSessions { get; set; }
    public int RemainingSessions => TotalSessions - UsedSessions;
    public bool IsActive => DateTime.UtcNow <= EndDate && RemainingSessions > 0;
}
