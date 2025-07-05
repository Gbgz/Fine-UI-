using Microsoft.EntityFrameworkCore;

namespace FineUICore.QuickStart
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
    }
}