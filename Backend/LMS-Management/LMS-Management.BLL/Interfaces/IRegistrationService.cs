using LMS_Management.BLL.DTOs;

namespace LMS_Management.BLL.Interfaces;

public interface IRegistrationService
{
    Task<RegistrationDto> RegisterAsync(string classId, RegisterStudentDto dto);
    Task<(bool success, string message)> CancelAsync(string registrationId);
}
