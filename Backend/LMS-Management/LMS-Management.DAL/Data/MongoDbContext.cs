using LMS_Management.DAL.Entities;
using MongoDB.Driver;

namespace LMS_Management.DAL.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoClient client, string databaseName)
    {
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Parent> Parents => _database.GetCollection<Parent>("parents");
    public IMongoCollection<Student> Students => _database.GetCollection<Student>("students");
    public IMongoCollection<Class> Classes => _database.GetCollection<Class>("classes");
    public IMongoCollection<ClassRegistration> ClassRegistrations => _database.GetCollection<ClassRegistration>("classRegistrations");
    public IMongoCollection<Subscription> Subscriptions => _database.GetCollection<Subscription>("subscriptions");
}
