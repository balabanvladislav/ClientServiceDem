using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB;

public partial class DbContext(DbContextOptions<DbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clients_pkey");

            entity.ToTable("clients");
            
            entity.HasIndex(e => e.Email, "idx_clients_email");
            entity.HasIndex(e => e.Email, "clients_email_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .HasColumnName("surname");

            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
        });


        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");

            entity.Property(e => e.SKU)
                .HasMaxLength(100)
                .HasColumnName("sku");

            entity.Property(e => e.Price)
                .HasColumnType("numeric(18,2)")
                .HasColumnName("price");

            entity.Property(e => e.Stock)
                .HasColumnName("stock");

            entity.HasMany(e => e.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .HasConstraintName("fk_orderitems_product_id")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");

            entity.Property(e => e.ClientId)
                .HasColumnName("client_id");

            entity.Property(e => e.OrderDate)
                .HasColumnName("order_date");

            entity.Property(e => e.Total)
                .HasColumnType("numeric(18,2)")
                .HasColumnName("total");

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(e => e.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(e => e.ClientId)
                .HasConstraintName("fk_orders_client_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .HasConstraintName("fk_orderitems_order_id")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderitems_pkey");

            entity.ToTable("order_items");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");

            entity.Property(e => e.OrderId)
                .HasColumnName("order_id");

            entity.Property(e => e.ProductId)
                .HasColumnName("product_id");

            entity.Property(e => e.Quantity)
                .HasColumnName("quantity");

            entity.Property(e => e.UnitPrice)
                .HasColumnType("numeric(18,2)")
                .HasColumnName("unit_price");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}