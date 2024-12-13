using ControlDeVenta_Proy.src.Models;
using ControlDeVenta_Proy.src.Models.Purchase;
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
        public DbSet<InvoiceCode> InvoiceCodes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SaleItem>()
                .HasKey(pc => new { pc.InvoiceId, pc.ProductId });

            modelBuilder.Entity<SaleItem>()
                .HasOne(pc => pc.Invoice)
                .WithMany(c => c.SaleItems)
                .HasForeignKey(pc => pc.InvoiceId);

            modelBuilder.Entity<SaleItem>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.SaleItems)
                .HasForeignKey(pc => pc.ProductId); 

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

            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.SaleItems)
                .WithOne(s => s.Invoice) 
                .HasForeignKey(s => s.InvoiceId); 
        }
    }
}