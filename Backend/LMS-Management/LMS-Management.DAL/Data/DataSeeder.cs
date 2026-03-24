using LMS_Management.DAL.Entities;
using MongoDB.Driver;

namespace LMS_Management.DAL.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(MongoDbContext db)
    {
        if (await db.Parents.CountDocumentsAsync(FilterDefinition<Parent>.Empty) > 0)
            return;

        var parent1 = new Parent { Name = "Nguyen Van An", Phone = "0901234567", Email = "an.nguyen@email.com" };
        var parent2 = new Parent { Name = "Tran Thi Bich", Phone = "0912345678", Email = "bich.tran@email.com" };
        await db.Parents.InsertManyAsync([parent1, parent2]);

        var student1 = new Student { Name = "Nguyen Minh Khoa", Dob = new DateTime(2015, 3, 10), Gender = "Male", CurrentGrade = "3", ParentId = parent1.Id };
        var student2 = new Student { Name = "Nguyen Thi Lan", Dob = new DateTime(2013, 7, 22), Gender = "Female", CurrentGrade = "5", ParentId = parent1.Id };
        var student3 = new Student { Name = "Tran Quoc Bao", Dob = new DateTime(2014, 11, 5), Gender = "Male", CurrentGrade = "4", ParentId = parent2.Id };
        await db.Students.InsertManyAsync([student1, student2, student3]);

        var class1 = new Class { Name = "Toan Co Ban A1", Subject = "Math", DayOfWeek = "Monday", TimeSlot = "08:00-09:30", TeacherName = "Le Van Hung", MaxStudents = 10 };
        var class2 = new Class { Name = "Tieng Anh Giao Tiep B1", Subject = "English", DayOfWeek = "Wednesday", TimeSlot = "14:00-15:30", TeacherName = "Pham Thi Mai", MaxStudents = 8 };
        var class3 = new Class { Name = "Vat Ly Nang Cao C2", Subject = "Physics", DayOfWeek = "Friday", TimeSlot = "09:00-10:30", TeacherName = "Hoang Duc Nam", MaxStudents = 12 };
        await db.Classes.InsertManyAsync([class1, class2, class3]);

        await db.Subscriptions.InsertManyAsync([
            new Subscription { StudentId = student1.Id, PackageName = "Basic 10", StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 6, 30), TotalSessions = 10, UsedSessions = 2 },
            new Subscription { StudentId = student2.Id, PackageName = "Standard 20", StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), TotalSessions = 20, UsedSessions = 5 },
            new Subscription { StudentId = student3.Id, PackageName = "Basic 10", StartDate = new DateTime(2026, 2, 1), EndDate = new DateTime(2026, 7, 31), TotalSessions = 10, UsedSessions = 0 }
        ]);
    }
}
