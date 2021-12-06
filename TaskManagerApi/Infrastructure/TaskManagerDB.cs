using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Models;

namespace TaskManagerApi.Infrastructure
{
    public class TaskManagerDB : IdentityDbContext<User>
    {
        public TaskManagerDB(DbContextOptions<TaskManagerDB> options) : base(options)
        {

        }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Domain.Models.Task> Tasks { get; set; }
        public DbSet<UserToTask> UserToTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity(typeof(Status)).HasData(
                 new Status { Id = 1, Name = "In progress" },
                 new Status { Id = 2, Name = "Completed" },
                 new Status { Id = 3, Name = "Planned" },
                 new Status { Id = 4, Name = "Review" }
                 );
        }
    }
}
