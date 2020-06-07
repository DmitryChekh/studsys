using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudSys.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using StudSys.Models.DbModels;

namespace StudSys.Data
{
    public class DataContext :IdentityDbContext<UserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<TodoItemModel> TodoItems { get; set; }
        public DbSet<GroupModel> Groups { get; set; }
        public DbSet<SubjectModel> Subject { get; set; }
    }
}
