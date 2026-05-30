using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Models;

[Table("product_images")]
public partial class ProductImage
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("image_url")]
    [StringLength(500)]
    public string ImageUrl { get; set; } = null!;

    [Column("is_thumbnail")]
    public bool? IsThumbnail { get; set; }

    [Column("alt_text")]
    [StringLength(255)]
    public string? AltText { get; set; }

    [Column("sort_order")]
    public int? SortOrder { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductImages")]
    public virtual Product Product { get; set; } = null!;
}
