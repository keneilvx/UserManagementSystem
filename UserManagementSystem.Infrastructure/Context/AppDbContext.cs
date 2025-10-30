using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using UserManagementSystem.Domain.DTOs;
using UserManagementSystem.Domain.Entities;

namespace UserManagementSystem.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

  
        }
        public DbSet<User> Users { get; set; }
    }
}
