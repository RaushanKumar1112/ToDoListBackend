using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Providers
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tasks> Tasks { get; set; }

    }
}
