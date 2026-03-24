using LMS_Management.BLL.DTOs;
using LMS_Management.BLL.Interfaces;
using LMS_Management.DAL.Data;
using LMS_Management.DAL.Entities;
using MongoDB.Driver;

namespace LMS_Management.BLL.Services;

public class ParentService : IParentService
{
    private readonly MongoDbContext _db;

    public ParentService(MongoDbContext db) => _db = db;

    public async Task<ParentDto> CreateAsync(CreateParentDto dto)
    {
        var parent = new Parent { Name = dto.Name, Phone = dto.Phone, Email = dto.Email };
        await _db.Parents.InsertOneAsync(parent);
        return MapToDto(parent);
    }

    public async Task<ParentDto?> GetByIdAsync(string id)
    {
        var parent = await _db.Parents.Find(p => p.Id == id).FirstOrDefaultAsync();
        return parent is null ? null : MapToDto(parent);
    }

    private static ParentDto MapToDto(Parent p) => new()
    {
        Id = p.Id, Name = p.Name, Phone = p.Phone, Email = p.Email
    };
}
