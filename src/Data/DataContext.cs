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
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<InvoiceState> InvoiceStates { get; set; } = null!;
        public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public DbSet<SaleItem> SaleItems { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<Supply> Supplies { get; set; } = null!;

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
                .HasKey(pp => new { pp.SupplierId, pp.ProductId }); 

            modelBuilder.Entity<Supply>()
                .HasOne(pp => pp.Supplier) 
                .WithMany(p => p.Supplies)
                .HasForeignKey(pp => pp.SupplierId); 

            modelBuilder.Entity<Supply>()
                .HasOne(pp => pp.Product)
                .WithMany(p => p.Supplies)
                .HasForeignKey(pp => pp.ProductId);
        }

    }
}