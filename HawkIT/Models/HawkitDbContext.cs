using Microsoft.EntityFrameworkCore;

namespace HawkIT.Models
{
    public class HawkitDbContext : DbContext
    {

        public HawkitDbContext(DbContextOptions<HawkitDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
