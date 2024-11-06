using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
namespace Gyminize_API.Data
{
    public class EntityDatabaseContext : DbContext
    {
        public EntityDatabaseContext(DbContextOptions<EntityDatabaseContext> options) : base(options)
        {
        }

        public DbSet<Customer> CustomerEntity { get; set; }
        public DbSet<Customer_health> CustomerHealthEntity { get; set; }

        public DbSet<Dailydiary> DailydiaryEntity { get; set; }

        public DbSet<Fooddetail> FooddetailEntity { get; set; }
        public DbSet<Food> FoodEntity { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Customer_health)
                .WithOne(ch => ch.Customer)
                .HasForeignKey<Customer_health>(ch => ch.customer_id);

            // Cấu hình mối quan hệ 1-n giữa Customer và Dailydiary
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Dailydiaries)  // Giả sử có thuộc tính Dailydiaries trong Customer
                .WithOne(d => d.Customer)      // Giả sử có thuộc tính Customer trong Dailydiary
                .HasForeignKey(d => d.customer_id);  // customer_id là khóa ngoại trong Dailydiary
            //Mối quan hệ giữa dailydiary và fooddetail
            modelBuilder.Entity<Dailydiary>()
                .HasMany(d => d.Fooddetails)
                .WithOne(f => f.Dailydiary)
                .HasForeignKey(f => f.dailydiary_id);
            //Mối quan hệ giữa food và foodetail
            modelBuilder.Entity<Food>()
                .HasMany(d=>d.Fooddetails)
                .WithOne(f => f.Food)
                .HasForeignKey(f => f.food_id);

            base.OnModelCreating(modelBuilder);

            
        }
    }
}
