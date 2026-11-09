using CY_DM;
using Microsoft.EntityFrameworkCore;

namespace Gr_Api.Models
{
    public class GrContext : DbContext
    {
        public GrContext(DbContextOptions<GrContext> options)
      : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //   optionsBuilder .UseSqlServer("OurConnectionString");
        }
        public DbSet<GrProduct> GrProduct { get; set; } = default!;
        public DbSet<GrManufacturer> GrManufacturer { get; set; } = default!;
        public DbSet<GrCategory> GrCategory { get; set; } = default!;
        public DbSet<GrKeyValue> GrKeyValue { get; set; } = default!;
    }
}
