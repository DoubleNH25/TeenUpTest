using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LMS_Management.DAL.Entities;

public class Class
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string DayOfWeek { get; set; } = string.Empty;
    public string TimeSlot { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public int MaxStudents { get; set; }
}
