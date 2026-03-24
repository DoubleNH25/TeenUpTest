using LMS_Management.BLL.DTOs;
using LMS_Management.BLL.Interfaces;
using LMS_Management.DAL.Data;
using LMS_Management.DAL.Entities;
using MongoDB.Driver;

namespace LMS_Management.BLL.Services;

public class RegistrationService : IRegistrationService
{
    private readonly MongoDbContext _db;

    public RegistrationService(MongoDbContext db) => _db = db;

    public async Task<RegistrationDto> RegisterAsync(string classId, RegisterStudentDto dto)
    {
        var cls = await _db.Classes.Find(c => c.Id == classId).FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException("Class not found.");

        var currentCount = await _db.ClassRegistrations.CountDocumentsAsync(r => r.ClassId == classId);
        if (currentCount >= cls.MaxStudents)
            throw new InvalidOperationException("Class is full.");

        var duplicate = await _db.ClassRegistrations
            .Find(r => r.ClassId == classId && r.StudentId == dto.StudentId)
            .FirstOrDefaultAsync();
        if (duplicate is not null)
            throw new InvalidOperationException("Student is already registered in this class.");

        var studentRegs = await _db.ClassRegistrations
            .Find(r => r.StudentId == dto.StudentId)
            .ToListAsync();

        var studentClassIds = studentRegs.Select(r => r.ClassId).ToList();
        if (studentClassIds.Count > 0)
        {
            var conflictFilter = Builders<Class>.Filter.And(
                Builders<Class>.Filter.In(c => c.Id, studentClassIds),
                Builders<Class>.Filter.Eq(c => c.DayOfWeek, cls.DayOfWeek),
                Builders<Class>.Filter.Eq(c => c.TimeSlot, cls.TimeSlot)
            );
            var conflict = await _db.Classes.Find(conflictFilter).FirstOrDefaultAsync();
            if (conflict is not null)
                throw new InvalidOperationException($"Student already has a class at {cls.TimeSlot} on {cls.DayOfWeek}.");
        }

        var subscription = await _db.Subscriptions
            .Find(s => s.Id == dto.SubscriptionId && s.StudentId == dto.StudentId)
            .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException("Subscription not found for this student.");

        if (subscription.EndDate < DateTime.UtcNow)
            throw new InvalidOperationException("Subscription has expired.");

        if (subscription.UsedSessions >= subscription.TotalSessions)
            throw new InvalidOperationException("No remaining sessions in subscription.");

        var registration = new ClassRegistration
        {
            ClassId = classId,
            StudentId = dto.StudentId,
            RegisteredAt = DateTime.UtcNow,
            NextSessionAt = dto.NextSessionAt
        };

        var subUpdate = Builders<Subscription>.Update.Inc(s => s.UsedSessions, 1);
        await _db.Subscriptions.UpdateOneAsync(s => s.Id == subscription.Id, subUpdate);
        await _db.ClassRegistrations.InsertOneAsync(registration);

        var student = await _db.Students.Find(s => s.Id == dto.StudentId).FirstOrDefaultAsync();
        return new RegistrationDto
        {
            Id = registration.Id,
            ClassId = classId,
            ClassName = cls.Name,
            StudentId = dto.StudentId,
            StudentName = student?.Name ?? string.Empty,
            RegisteredAt = registration.RegisteredAt,
            NextSessionAt = registration.NextSessionAt
        };
    }

    public async Task<(bool success, string message)> CancelAsync(string registrationId)
    {
        var registration = await _db.ClassRegistrations
            .Find(r => r.Id == registrationId)
            .FirstOrDefaultAsync();

        if (registration is null)
            return (false, "Registration not found.");

        bool refundSession = registration.NextSessionAt.HasValue
            && (registration.NextSessionAt.Value - DateTime.UtcNow).TotalHours > 24;

        if (refundSession)
        {
            var subFilter = Builders<Subscription>.Filter.And(
                Builders<Subscription>.Filter.Eq(s => s.StudentId, registration.StudentId),
                Builders<Subscription>.Filter.Gt(s => s.UsedSessions, 0)
            );
            var subUpdate = Builders<Subscription>.Update.Inc(s => s.UsedSessions, -1);
            await _db.Subscriptions.UpdateOneAsync(subFilter, subUpdate);
        }

        await _db.ClassRegistrations.DeleteOneAsync(r => r.Id == registrationId);

        var msg = refundSession
            ? "Registration cancelled and 1 session refunded."
            : "Registration cancelled. No refund (less than 24h before session).";

        return (true, msg);
    }
}
