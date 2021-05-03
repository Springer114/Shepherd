using Microsoft.EntityFrameworkCore;

namespace Shepherd.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}
    }
}