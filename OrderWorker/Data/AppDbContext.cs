using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrderWorker.Models;

namespace PaymentService.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions opt):base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderItem>().HasKey(oi => new {oi.OrderId,oi.ItemId});
            builder.Entity<OrderItem>().HasOne(oi => oi.Order).WithMany(o=>o.OrderItems).HasForeignKey(oi=>oi.OrderId);
            builder.Entity<OrderItem>().HasOne(oi => oi.Item).WithMany().HasForeignKey(oi=>oi.ItemId);

        }
        public DbSet<Item> Items{ get; set; }
        public DbSet<Order> Orders{get; set; }
        public DbSet<OrderItem> orderItems{get; set; }

    }
}