using LMS_Management.BLL.DTOs;

namespace LMS_Management.BLL.Interfaces;

public interface ISubscriptionService
{
    Task<SubscriptionDto> CreateAsync(CreateSubscriptionDto dto);
    Task<SubscriptionDto?> GetByIdAsync(string id);
    Task<SubscriptionDto?> UseSessionAsync(string id);
}
