using Microsoft.EntityFrameworkCore;
using TouristPlanner.Models;

namespace TouristPlanner.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Place> Places { get; set; }
    }
}