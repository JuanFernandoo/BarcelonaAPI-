using BarcelonaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BarcelonaAPI.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<PaymentAddresses> PaymentAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentAddresses>().HasKey(a => a.PaymentId);
            modelBuilder.Entity<CartItems>().HasKey(ci => ci.CartItemId);
            modelBuilder.Entity<Categories>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<Orders>().HasKey(o => o.OrderId);
            modelBuilder.Entity<Products>().HasKey(p => p.ProductId);
            modelBuilder.Entity<Users>().HasKey(u => u.UserId);

            modelBuilder.Entity<Orders>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Products>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Products>()
                .HasOne(p => p.Categories)
                .WithMany()  
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
