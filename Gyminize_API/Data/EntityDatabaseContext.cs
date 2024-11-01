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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Customer_health)
                .WithOne(ch => ch.Customer)
                .HasForeignKey<Customer_health>(ch => ch.customer_id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
