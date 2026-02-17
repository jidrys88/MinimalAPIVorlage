using DataModels;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace DBUmgebung
{
    public class AppDbContext : DbContext
    {
        // Konstruktor für Dependency Injection
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

       

        // DbSet für die Products-Tabelle
        public DbSet<Product> Products { get; set; }
    }
 

}
