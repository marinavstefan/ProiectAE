using Microsoft.EntityFrameworkCore;
using ProiectAE.Models.Entities;

namespace ProiectAE
{

    public class ProiectContext : DbContext
    {
        public ProiectContext(DbContextOptions<ProiectContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
