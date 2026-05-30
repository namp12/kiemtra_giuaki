using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Models;

[Table("products")]
[Index("Slug", Name = "UQ__products__32DD1E4C87C5947E", IsUnique = true)]
[Index("Sku", Name = "UQ__products__DDDF4BE7FE32BEC8", IsUnique = true)]
[Index("CategoryId", "BrandId", Name = "idx_products_category_brand")]
public partial class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("sku")]
    [StringLength(50)]
    public string Sku { get; set; } = null!;

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("slug")]
    [StringLength(255)]
    public string Slug { get; set; } = null!;

    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("brand_id")]
    public int BrandId { get; set; }

    [Column("price", TypeName = "decimal(15, 2)")]
    public decimal Price { get; set; }

    [Column("cost_price", TypeName = "decimal(15, 2)")]
    public decimal? CostPrice { get; set; }

    [Column("discount_percent")]
    public int? DiscountPercent { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("sold")]
    public int? Sold { get; set; }

    [Column("main_image")]
    [StringLength(500)]
    public string? MainImage { get; set; }

    [Column("short_description")]
    public string? ShortDescription { get; set; }

    [Column("full_description")]
    public string? FullDescription { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("featured")]
    public bool? Featured { get; set; }

    [Column("views")]
    public int? Views { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("Products")]
    public virtual Brand Brand { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    [InverseProperty("Product")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("Product")]
    public virtual ICollection<Specification> Specifications { get; set; } = new List<Specification>();
}
