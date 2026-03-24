using LMS_Management.BLL.DTOs;
using LMS_Management.BLL.Interfaces;
using LMS_Management.DAL.Data;
using LMS_Management.DAL.Entities;
using MongoDB.Driver;

namespace LMS_Management.BLL.Services;

public class StudentService : IStudentService
{
    private readonly MongoDbContext _db;

    public StudentService(MongoDbContext db) => _db = db;

    public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
    {
        var student = new Student
        {
            Name = dto.Name, Dob = dto.Dob, Gender = dto.Gender,
            CurrentGrade = dto.CurrentGrade, ParentId = dto.ParentId
        };
        await _db.Students.InsertOneAsync(student);

        var parent = await _db.Parents.Find(p => p.Id == student.ParentId).FirstOrDefaultAsync();
        return MapToDto(student, parent);
    }

    public async Task<StudentDto?> GetByIdAsync(string id)
    {
        var student = await _db.Students.Find(s => s.Id == id).FirstOrDefaultAsync();
        if (student is null) return null;

        var parent = await _db.Parents.Find(p => p.Id == student.ParentId).FirstOrDefaultAsync();
        return MapToDto(student, parent);
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        var students = await _db.Students.Find(_ => true).ToListAsync();
        var parentIds = students.Select(s => s.ParentId).Distinct().ToList();
        var parents = await _db.Parents.Find(p => parentIds.Contains(p.Id)).ToListAsync();
        var parentMap = parents.ToDictionary(p => p.Id);
        return students.Select(s => MapToDto(s, parentMap.GetValueOrDefault(s.ParentId)));
    }

    private static StudentDto MapToDto(Student s, Parent? parent) => new()
    {
        Id = s.Id, Name = s.Name, Dob = s.Dob, Gender = s.Gender, CurrentGrade = s.CurrentGrade,
        Parent = parent is null ? null : new ParentDto { Id = parent.Id, Name = parent.Name, Phone = parent.Phone, Email = parent.Email }
    };
}
