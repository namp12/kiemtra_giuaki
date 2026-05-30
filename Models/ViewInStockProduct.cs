using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Models;

[Keyless]
public partial class ViewInStockProduct
{
    [Column("id")]
    public int Id { get; set; }

    [Column("sku")]
    [StringLength(50)]
    public string Sku { get; set; } = null!;

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("price", TypeName = "decimal(15, 2)")]
    public decimal Price { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("sold")]
    public int? Sold { get; set; }

    [Column("brand")]
    [StringLength(100)]
    public string Brand { get; set; } = null!;

    [Column("category")]
    [StringLength(100)]
    public string Category { get; set; } = null!;
}
