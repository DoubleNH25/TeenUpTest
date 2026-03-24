using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LMS_Management.DAL.Entities;

public class Parent
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
