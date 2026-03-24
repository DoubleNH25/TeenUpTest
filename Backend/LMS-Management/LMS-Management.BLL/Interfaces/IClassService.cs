using LMS_Management.BLL.DTOs;

namespace LMS_Management.BLL.Interfaces;

public interface IClassService
{
    Task<ClassDto> CreateAsync(CreateClassDto dto);
    Task<IEnumerable<ClassDto>> GetByDayAsync(string? day);
}
