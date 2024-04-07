using CashRegister.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CashRegister.Infrastructure.Context
{
    public class CashRegisterDBContext : DbContext
    {
        public CashRegisterDBContext(DbContextOptions<CashRegisterDBContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBill> ProductBills { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<DeletedBills> DeletedBills { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<CreditCardType> CreditCardTypes { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
        .Property(p => p.Price)
        .HasPrecision(18, 2);

            modelBuilder.Entity<ProductBill>()
                .Property(pb => pb.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Bill>()
                .Property(b => b.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.CreditCard)
                .WithMany(cc => cc.Bills)
                .HasForeignKey(b => b.CreditCardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bills)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<ProductBill>()
                .HasOne(pb => pb.Bill)
                .WithMany(b => b.ProductBills)
                .HasForeignKey(pb => pb.BillId);

            modelBuilder.Entity<ProductBill>()
                .HasOne(pb => pb.Product)
                .WithMany(p => p.ProductBills)
                .HasForeignKey(pb => pb.ProductId);

            modelBuilder.Entity<CreditCard>()
                .HasOne(cc => cc.User)
                .WithMany(u => u.CreditCards)
                .HasForeignKey(cc => cc.UserId);

            modelBuilder.Entity<CreditCard>()
                .HasOne(cc => cc.CreditCardType)
                .WithMany(cct => cct.CreditCards)
                .HasForeignKey(cc => cc.TypeId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);


            modelBuilder.Entity<DeletedBills>()
                .HasOne(db => db.Bill)
                .WithMany() 
                .HasForeignKey(db => db.BillId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>()
                .HasIndex(b => b.Number)
                .IsUnique();
        }

    }
}
