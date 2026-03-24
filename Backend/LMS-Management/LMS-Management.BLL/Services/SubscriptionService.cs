using LMS_Management.BLL.DTOs;
using LMS_Management.BLL.Interfaces;
using LMS_Management.DAL.Data;
using LMS_Management.DAL.Entities;
using MongoDB.Driver;

namespace LMS_Management.BLL.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly MongoDbContext _db;

    public SubscriptionService(MongoDbContext db) => _db = db;

    public async Task<SubscriptionDto> CreateAsync(CreateSubscriptionDto dto)
    {
        var sub = new Subscription
        {
            StudentId = dto.StudentId, PackageName = dto.PackageName,
            StartDate = dto.StartDate, EndDate = dto.EndDate,
            TotalSessions = dto.TotalSessions, UsedSessions = 0
        };
        await _db.Subscriptions.InsertOneAsync(sub);

        var student = await _db.Students.Find(s => s.Id == sub.StudentId).FirstOrDefaultAsync();
        return MapToDto(sub, student?.Name ?? string.Empty);
    }

    public async Task<SubscriptionDto?> GetByIdAsync(string id)
    {
        var sub = await _db.Subscriptions.Find(s => s.Id == id).FirstOrDefaultAsync();
        if (sub is null) return null;

        var student = await _db.Students.Find(s => s.Id == sub.StudentId).FirstOrDefaultAsync();
        return MapToDto(sub, student?.Name ?? string.Empty);
    }

    public async Task<SubscriptionDto?> UseSessionAsync(string id)
    {
        var sub = await _db.Subscriptions.Find(s => s.Id == id).FirstOrDefaultAsync();
        if (sub is null) return null;

        if (sub.UsedSessions >= sub.TotalSessions)
            throw new InvalidOperationException("No remaining sessions.");
        if (sub.EndDate < DateTime.UtcNow)
            throw new InvalidOperationException("Subscription has expired.");

        var update = Builders<Subscription>.Update.Inc(s => s.UsedSessions, 1);
        await _db.Subscriptions.UpdateOneAsync(s => s.Id == id, update);
        sub.UsedSessions++;

        var student = await _db.Students.Find(s => s.Id == sub.StudentId).FirstOrDefaultAsync();
        return MapToDto(sub, student?.Name ?? string.Empty);
    }

    public async Task<IEnumerable<SubscriptionDto>> GetByStudentAsync(string studentId)
    {
        var subs = await _db.Subscriptions.Find(s => s.StudentId == studentId).ToListAsync();
        var student = await _db.Students.Find(s => s.Id == studentId).FirstOrDefaultAsync();
        return subs.Select(s => MapToDto(s, student?.Name ?? string.Empty));
    }

    private static SubscriptionDto MapToDto(Subscription s, string studentName) => new()
    {
        Id = s.Id, StudentId = s.StudentId, StudentName = studentName,
        PackageName = s.PackageName, StartDate = s.StartDate, EndDate = s.EndDate,
        TotalSessions = s.TotalSessions, UsedSessions = s.UsedSessions
    };
}
