using Microsoft.EntityFrameworkCore;

namespace Shepherd.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}
        
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}