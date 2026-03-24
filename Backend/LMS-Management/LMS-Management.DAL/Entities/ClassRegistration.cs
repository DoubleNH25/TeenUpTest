using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LMS_Management.DAL.Entities;

public class ClassRegistration
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonRepresentation(BsonType.ObjectId)]
    public string ClassId { get; set; } = string.Empty;

    [BsonRepresentation(BsonType.ObjectId)]
    public string StudentId { get; set; } = string.Empty;

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public DateTime? NextSessionAt { get; set; }
}
