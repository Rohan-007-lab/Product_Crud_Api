using Microsoft.EntityFrameworkCore;
using ProductCrudApi.Domain.Entities;

namespace ProductCrudApi.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.ProductName)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(p => p.CreatedBy)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.ModifiedBy)
                      .HasMaxLength(100);
            });

            // Item configuration
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Quantity)
                      .IsRequired();

                // User configuration
                modelBuilder.Entity<User>(entity =>
                {
                    entity.HasKey(u => u.Id);

                    entity.Property(u => u.Username)
                          .IsRequired()
                          .HasMaxLength(100);

                    entity.HasIndex(u => u.Username)
                          .IsUnique();

                    entity.Property(u => u.PasswordHash)
                          .IsRequired();
                });

                // Item -> Product relationship (Foreign Key)
                entity.HasOne(i => i.Product)
                      .WithMany(p => p.Items)
                      .HasForeignKey(i => i.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}