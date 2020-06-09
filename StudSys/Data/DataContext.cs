using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudSys.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using StudSys.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace StudSys.Data
{
    public class DataContext : IdentityDbContext<UserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<TodoItemModel> TodoItems { get; set; }
        public DbSet<GroupModel> Groups { get; set; }
        public DbSet<SubjectModel> Subject { get; set; }
        public DbSet<SubjectLessonModel> SubjectLesson { get; set; }

        public DbSet<SubjectGroupModel> SubjectGroup { get; set; }
    }
}
