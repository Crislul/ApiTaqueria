using APITaqueriaIguala.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace APITaqueriaIguala.Data
{
    public class ApplicationDbContext : IdentityDbContext<AspNetUsers>
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<BusinessLocation> BusinessLocations { get; set; }
        public DbSet<DireccionEnvio> DireccionesEnvio { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<AspNetUsers> Users { get; set; }

        // Relación entre usuarios y roles
        public DbSet<IdentityUserRole<string>> UserRoles { get; set; } // Aquí es donde se maneja la relación de roles

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aquí no configurarás AspNetUserRoles ya que se usa la implementación predeterminada

            // Configuración de nombres de tablas
            modelBuilder.Entity<BusinessLocation>().ToTable("BusinessLocations");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Categories>().ToTable("Categories");

            // Configuración de relaciones con eliminación en cascada
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne()
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}