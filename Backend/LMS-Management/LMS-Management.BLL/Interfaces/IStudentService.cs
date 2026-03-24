using LMS_Management.BLL.DTOs;

namespace LMS_Management.BLL.Interfaces;

public interface IStudentService
{
    Task<StudentDto> CreateAsync(CreateStudentDto dto);
    Task<StudentDto?> GetByIdAsync(string id);
}
