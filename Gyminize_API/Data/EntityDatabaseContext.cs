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
         public DbSet<Plan> PlanEntity { get; set; }
        public DbSet<Plandetail> PlanDetailEntity { get; set; }
        public DbSet<Typeworkout> TypeworkoutEntity { get; set; }
        public DbSet<Workoutdetail> WorkoutDetailEntity { get; set; }
        public DbSet<Exercise> ExerciseEntity { get; set; }
        public DbSet<Exercisedetail> ExerciseDetailEntity { get; set; }

        public DbSet<Orders> OrdersEntity
        {
          get; set;
        }
        public DbSet<Orderdetail> OrderdetailEntity
        {
            get; set;
        }
        public DbSet<Product> ProductEntity
        {
            get; set;
        }
        public DbSet<Category> CategoryEntity
        {
            get; set;
        }
        public DbSet<Payment> PaymentEntity
        {
            get; set;
        }
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
                .HasMany(d => d.Fooddetails)
                .WithOne(f => f.Food)
                .HasForeignKey(f => f.food_id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade); // Thiết lập cascade delete



            // Mối quan hệ giữa Plan và PlanDetail (1-n)
            modelBuilder.Entity<Plan>()
                .HasMany(p => p.Plandetail)
                .WithOne(pd => pd.Plan)
                .HasForeignKey(pd => pd.plan_id);

            // Mối quan hệ giữa Customer và PlanDetail (1-n)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Plandetails)
                .WithOne(pd => pd.Customer)
                .HasForeignKey(pd => pd.customer_id);

            // Mối quan hệ giữa Typeworkout và WorkoutDetail (1-n)
            modelBuilder.Entity<Typeworkout>()
                .HasMany(tw => tw.Workoutdetails)
                .WithOne(wd => wd.Typeworkout)
                .HasForeignKey(wd => wd.typeworkout_id);

            // Mối quan hệ giữa Plan và WorkoutDetail (1-n)
            modelBuilder.Entity<Plandetail>()
                .HasMany(p => p.Workoutdetails)
                .WithOne(wd => wd.Plandetail)
                .HasForeignKey(wd => wd.plandetail_id);

            // Mối quan hệ giữa Typeworkout và ExerciseDetail (1-n)
            modelBuilder.Entity<Typeworkout>()
                .HasMany(tw => tw.Exercisedetails)
                .WithOne(ed => ed.Typeworkout)
                .HasForeignKey(ed => ed.typeworkout_id);

            // Mối quan hệ giữa Exercise và ExerciseDetail (1-n)
            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Exercisedetails)
                .WithOne(ed => ed.Exercise)
                .HasForeignKey(ed => ed.exercise_id);

            modelBuilder.Entity<Orders>()
                .HasMany(o => o.Orderdetail)
                .WithOne(od => od.Orders)
                .HasForeignKey(od => od.orders_id);
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Orderdetail)
                .WithOne(od => od.Product)
                .HasForeignKey(od => od.product_id)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade); // Thiết lập cascade delete

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Product)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.category_id);

            modelBuilder.Entity<Orders>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Orders)
                .HasForeignKey<Payment>(p => p.orders_id);

            // Cấu hình mối quan hệ 1-n giữa Customer và Orders
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders) 
                .WithOne(d => d.Customer)     
                .HasForeignKey(d => d.customer_id);  

            base.OnModelCreating(modelBuilder);


        }
    }

}
