using Microsoft.EntityFrameworkCore;

namespace Shepherd.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}
        
        public DbSet<User> Users { get; set; }
        public DbSet<Pen> Pens { get; set; }
        public DbSet<Team> Teams { get; set; }
    }
}