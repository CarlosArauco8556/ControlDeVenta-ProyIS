using ControlDeVenta_Proy.src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ControlDeVenta_Proy.src.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<InvoiceState> InvoiceStates { get; set; } = null!;
        public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public DbSet<SaleItem> SaleItems { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<Supply> Supplies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.SaleItems)
                .WithOne(si => si.Product)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Supplies)
                .WithOne(s => s.Product)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Supplies)
                .WithOne(s => s.Supplier)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Products)
                .WithMany(p => p.Suppliers)
                .UsingEntity(j => j.ToTable("SupplierProducts"));

            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.SaleItems)
                .WithOne(si => si.Invoice)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.User)
                .WithMany(u => u.Invoices)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.InvoiceState)
                .WithMany(is_ => is_.Invoices)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.PaymentMethod)
                .WithMany(pm => pm.Invoices)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supply>()
                .HasKey(s => s.Id);
                
            modelBuilder.Entity<Supply>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Supplies)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supply>()
                .HasOne(s => s.Supplier)
                .WithMany(s => s.Supplies)
                .HasForeignKey(s => s.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}