using LMS_Management.BLL.DTOs;

namespace LMS_Management.BLL.Interfaces;

public interface IParentService
{
    Task<ParentDto> CreateAsync(CreateParentDto dto);
    Task<ParentDto?> GetByIdAsync(string id);
    Task<IEnumerable<ParentDto>> GetAllAsync();
}
