using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public DbSet<Supplier> Providers { get; set; } = null!;
        public DbSet<Supply> Categories { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<SaleItem> InvoiceDetails { get; set; } = null!;
        public DbSet<InvoiceState> InvoiceDetail { get; set; } = null!;
        public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Worker", NormalizedName = "WORKER" },
                new IdentityRole { Name = "Client", NormalizedName = "CLIENT" }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            modelBuilder.Entity<SaleItem>()
                .HasKey(x => new { x.InvoiceId, x.ProductId });
            
            modelBuilder.Entity<SaleItem>()
                .HasOne(x => x.Product)
                .WithMany(x => x.SaleItems)
                .HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<SaleItem>()
                .HasOne(x => x.Invoice)
                .WithMany(x => x.SaleItems)
                .HasForeignKey(x => x.InvoiceId);

            modelBuilder.Entity<Supply>()
                .HasKey(x => new { x.ProductId, x.SupplierId });
            
            modelBuilder.Entity<Supply>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Supplies)
                .HasForeignKey(x => x.ProductId);
            
            modelBuilder.Entity<Supply>()
                .HasOne(x => x.Supplier)
                .WithMany(x => x.Supplies)
                .HasForeignKey(x => x.SupplierId);
                
        }
    }
}