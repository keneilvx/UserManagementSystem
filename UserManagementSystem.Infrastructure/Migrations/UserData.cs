using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Infrastructure.Context;

namespace UserManagementSystem.Infrastructure.Migrations
{
    public static class UserData
    {
        public static void UserSeed(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(

                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "keneil40",
                        FirstName = "Keneil",
                        LastName = "Smith",
                        Email = "keneil.smith@examplemail.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                        CreatedAt = DateTime.UtcNow,
                        LastLoginAt = DateTimeOffset.UtcNow
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "john50",
                        FirstName = "John",
                        LastName = "Doe",
                        Email = "john.doe@examplemail.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("test123"),
                        CreatedAt = DateTime.UtcNow.AddDays(-2),
                        LastLoginAt = DateTimeOffset.UtcNow.AddHours(0.30)
                    }

                 );
                context.SaveChanges();

            }
        }
    }
}
