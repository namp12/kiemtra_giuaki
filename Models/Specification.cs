using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Models;

[Table("specifications")]
public partial class Specification
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("spec_group")]
    [StringLength(100)]
    public string? SpecGroup { get; set; }

    [Column("spec_name")]
    [StringLength(255)]
    public string SpecName { get; set; } = null!;

    [Column("spec_value")]
    public string SpecValue { get; set; } = null!;

    [Column("sort_order")]
    public int? SortOrder { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Specifications")]
    public virtual Product Product { get; set; } = null!;
}
