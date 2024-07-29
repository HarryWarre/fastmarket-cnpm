using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace fastwebsite.Entities;

public partial class FastdbContext : DbContext
{
    public FastdbContext()
    {
    }

    public FastdbContext(DbContextOptions<FastdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<SalesStaff> SalesStaffs { get; set; }

    public virtual DbSet<TypePayment> TypePayments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // This part is optional if you are using DI
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=fastdb;Integrated Security=True;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__Banner__BD58D47345C6E805");

            entity.ToTable("Banner");

            entity.Property(e => e.BannerId).HasColumnName("bannerId");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.Link)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("link");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__415B03B872A4369C");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("cartId");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalPrice");

            entity.HasOne(d => d.Account).WithMany(p => p.Carts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Cart__accountId__32E0915F");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__283983B6C06CF46B");

            entity.ToTable("CartItem");

            entity.Property(e => e.CartItemId).HasColumnName("cartItemId");
            entity.Property(e => e.CartId).HasColumnName("cartId");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK__CartItem__cartId__35BCFE0A");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__CartItem__produc__36B12243");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CateId).HasName("PK__Category__A88B4DE4B4E7ADA9");

            entity.ToTable("Category");

            entity.Property(e => e.CateId).HasColumnName("cateId");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("PK__Coupon__A2AF8D544B4B05C8");

            entity.ToTable("Coupon");

            entity.HasIndex(e => e.Code, "UQ__Coupon__357D4CF9F7B66F8A").IsUnique();

            entity.Property(e => e.CouponId).HasColumnName("couponId");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("date")
                .HasColumnName("expirationDate");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Customer__F267251E14686187");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "UQ__Customer__AB6E616413DAAF6F").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__0809335DB6C40AD1");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.CodeCoupon)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("codeCoupon");
            entity.Property(e => e.OrderDate)
                .HasColumnType("date")
                .HasColumnName("orderDate");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalPrice");
            entity.Property(e => e.TypePaymentId).HasColumnName("typePaymentId");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Orders__accountI__3B75D760");

            entity.HasOne(d => d.TypePayment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TypePaymentId)
                .HasConstraintName("FK__Orders__typePaym__3C69FB99");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__3724BD526629D889");

            entity.ToTable("OrderItem");

            entity.Property(e => e.OrderItemId).HasColumnName("orderItemId");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderItem__order__3F466844");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderItem__produ__403A8C7D");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__2D10D16A59B32396");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.CateId).HasColumnName("cateId");
            entity.Property(e => e.Des)
                .HasColumnType("text")
                .HasColumnName("des");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("productName");

            entity.HasOne(d => d.Cate).WithMany(p => p.Products)
                .HasForeignKey(d => d.CateId)
                .HasConstraintName("FK__Product__cateId__2C3393D0");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => new { e.AccountId, e.ProductId }).HasName("PK__Review__40B6280808497295");

            entity.ToTable("Review");

            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.Rates)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("rates");

            entity.HasOne(d => d.Account).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__accountI__2F10007B");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__productI__300424B4");
        });

        modelBuilder.Entity<SalesStaff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SalesSta__3213E83F358506F4");

            entity.ToTable("SalesStaff");

            entity.HasIndex(e => e.Email, "UQ__SalesSta__AB6E61644E7E1F5B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdminState).HasColumnName("adminState");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<TypePayment>(entity =>
        {
            entity.HasKey(e => e.TypePaymentId).HasName("PK__typePaym__2380E3F93F0268AF");

            entity.ToTable("typePayment");

            entity.Property(e => e.TypePaymentId).HasColumnName("typePaymentId");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("paymentMethod");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
