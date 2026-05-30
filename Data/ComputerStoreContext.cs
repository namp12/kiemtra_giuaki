using System;
using System.Collections.Generic;
using Kiemtragiuaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Data;

public partial class ComputerStoreContext : DbContext
{
    public ComputerStoreContext()
    {
    }

    public ComputerStoreContext(DbContextOptions<ComputerStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<InventoryTransaction> InventoryTransactions { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Specification> Specifications { get; set; }

    public virtual DbSet<ViewInStockProduct> ViewInStockProducts { get; set; }

    public virtual DbSet<ViewMonthlyRevenue> ViewMonthlyRevenues { get; set; }

    public virtual DbSet<ViewTopSellingProduct> ViewTopSellingProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=computer_store;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__admins__3213E83FF173D8C8");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Role).HasDefaultValue("editor");
            entity.Property(e => e.Status).HasDefaultValue("active");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__brands__3213E83FDD288992");

            entity.Property(e => e.Status).HasDefaultValue("active");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__carts__3213E83F87809720");

            entity.Property(e => e.AddedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts).HasConstraintName("FK__carts__customer___778AC167");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__carts__product_i__787EE5A0");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__categori__3213E83FCEFFF50E");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("active");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK__categorie__paren__3A81B327");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__contacts__3213E83F15A965ED");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("new");

            entity.HasOne(d => d.RepliedByNavigation).WithMany(p => p.Contacts).HasConstraintName("FK__contacts__replie__19DFD96B");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__coupons__3213E83F7754A37C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MinOrderValue).HasDefaultValue(0m);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.Status).HasDefaultValue("active");
            entity.Property(e => e.UsedCount).HasDefaultValue(0);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__customer__3213E83FBDA38BFA");

            entity.ToTable("customers", tb => tb.HasTrigger("trg_customers_updated"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EmailVerified).HasDefaultValue(false);
            entity.Property(e => e.Status).HasDefaultValue("active");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<InventoryTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__inventor__3213E83FDD3942F1");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Admin).WithMany(p => p.InventoryTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inventory__admin__0D7A0286");

            entity.HasOne(d => d.Product).WithMany(p => p.InventoryTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inventory__produ__0C85DE4D");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orders__3213E83F18E6048A");

            entity.Property(e => e.DiscountAmount).HasDefaultValue(0m);
            entity.Property(e => e.OrderStatus).HasDefaultValue("pending");
            entity.Property(e => e.OrderedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PaymentMethod).HasDefaultValue("cod");
            entity.Property(e => e.PaymentStatus).HasDefaultValue("pending");
            entity.Property(e => e.ShippingFee).HasDefaultValue(0m);
            entity.Property(e => e.TaxAmount).HasDefaultValue(0m);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__customer__6B24EA82");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__order_it__3213E83FB73EC275");

            entity.Property(e => e.DiscountPercent).HasDefaultValue(0);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasConstraintName("FK__order_ite__order__6FE99F9F");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__order_ite__produ__70DDC3D8");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__products__3213E83F6A8D749E");

            entity.ToTable("products", tb => tb.HasTrigger("trg_products_updated"));

            entity.Property(e => e.CostPrice).HasDefaultValue(0m);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DiscountPercent).HasDefaultValue(0);
            entity.Property(e => e.Featured).HasDefaultValue(false);
            entity.Property(e => e.Sold).HasDefaultValue(0);
            entity.Property(e => e.Status).HasDefaultValue("draft");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Views).HasDefaultValue(0);

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__products__brand___5070F446");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__products__catego__4F7CD00D");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__product___3213E83F908D411B");

            entity.Property(e => e.IsThumbnail).HasDefaultValue(false);
            entity.Property(e => e.SortOrder).HasDefaultValue(0);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages).HasConstraintName("FK__product_i__produ__59063A47");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__reviews__3213E83F92E8AB43");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.HelpfulCount).HasDefaultValue(0);
            entity.Property(e => e.Status).HasDefaultValue("pending");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__reviews__custome__01142BA1");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews).HasConstraintName("FK__reviews__product__00200768");
        });

        modelBuilder.Entity<Specification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__specific__3213E83F7226BC2C");

            entity.Property(e => e.SortOrder).HasDefaultValue(0);

            entity.HasOne(d => d.Product).WithMany(p => p.Specifications).HasConstraintName("FK__specifica__produ__5441852A");
        });

        modelBuilder.Entity<ViewInStockProduct>(entity =>
        {
            entity.ToView("view_in_stock_products");
        });

        modelBuilder.Entity<ViewMonthlyRevenue>(entity =>
        {
            entity.ToView("view_monthly_revenue");
        });

        modelBuilder.Entity<ViewTopSellingProduct>(entity =>
        {
            entity.ToView("view_top_selling_products");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
