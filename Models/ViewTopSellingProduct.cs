using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Models;

[Keyless]
public partial class ViewTopSellingProduct
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("sku")]
    [StringLength(50)]
    public string Sku { get; set; } = null!;

    [Column("total_sold")]
    public int? TotalSold { get; set; }

    [Column("current_stock")]
    public int CurrentStock { get; set; }
}
