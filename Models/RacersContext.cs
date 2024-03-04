using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sprint1HEM.Models;

public partial class RacersContext : DbContext
{
    public RacersContext()
    {
    }

    public RacersContext(DbContextOptions<RacersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<OrderList> OrderLists { get; set; }

    /*  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
          => optionsBuilder.UseSqlServer("Server=LAPTOP-JDGHM7O1;Database=Racers;Trusted_Connection=True;TrustServerCertificate=True;");*/

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("Racers");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD797A9776A65");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__CustomerId__3B75D760");

            entity.HasOne(d => d.Item).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__ItemId__3C69FB99");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8F942AB8F");

            entity.ToTable("Customer");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Item__727E838BEB49576F");

            entity.ToTable("Item");

            entity.Property(e => e.ItemCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ItemDetails)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Itemname)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrderList>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__OrderLis__C3905BAF432B673C");

            entity.ToTable("OrderList");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.OrderLists)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__OrderList__Custo__3F466844");

            entity.HasOne(d => d.Item).WithMany(p => p.OrderLists)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__OrderList__ItemI__403A8C7D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
