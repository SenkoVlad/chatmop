using Microsoft.EntityFrameworkCore;
using senkovlad.chat.data.Models;

namespace senkovlad.chat.data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
