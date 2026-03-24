using LMS_Management.BLL.DTOs;
using LMS_Management.BLL.Interfaces;
using LMS_Management.DAL.Data;
using LMS_Management.DAL.Entities;
using MongoDB.Driver;

namespace LMS_Management.BLL.Services;

public class ClassService : IClassService
{
    private readonly MongoDbContext _db;

    public ClassService(MongoDbContext db) => _db = db;

    public async Task<ClassDto> CreateAsync(CreateClassDto dto)
    {
        var cls = new Class
        {
            Name = dto.Name, Subject = dto.Subject, DayOfWeek = dto.DayOfWeek,
            TimeSlot = dto.TimeSlot, TeacherName = dto.TeacherName, MaxStudents = dto.MaxStudents
        };
        await _db.Classes.InsertOneAsync(cls);
        return MapToDto(cls, 0);
    }

    public async Task<IEnumerable<ClassDto>> GetByDayAsync(string? day)
    {
        var filter = string.IsNullOrWhiteSpace(day)
            ? FilterDefinition<Class>.Empty
            : Builders<Class>.Filter.Regex(c => c.DayOfWeek, new MongoDB.Bson.BsonRegularExpression($"^{day}$", "i"));

        var classes = await _db.Classes.Find(filter).ToListAsync();

        var result = new List<ClassDto>();
        foreach (var cls in classes)
        {
            var count = await _db.ClassRegistrations.CountDocumentsAsync(r => r.ClassId == cls.Id);
            result.Add(MapToDto(cls, (int)count));
        }
        return result;
    }

    private static ClassDto MapToDto(Class c, int currentStudents) => new()
    {
        Id = c.Id, Name = c.Name, Subject = c.Subject, DayOfWeek = c.DayOfWeek,
        TimeSlot = c.TimeSlot, TeacherName = c.TeacherName, MaxStudents = c.MaxStudents,
        CurrentStudents = currentStudents
    };
}
