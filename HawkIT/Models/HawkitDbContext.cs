using Microsoft.EntityFrameworkCore;

namespace HawkIT.Models
{
    public class HawkitDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Worker> Workers { get; set; }

        public HawkitDbContext(DbContextOptions<HawkitDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
