using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Models;

[Table("coupons")]
[Index("Code", Name = "UQ__coupons__357D4CF904205A0C", IsUnique = true)]
[Index("StartDate", "EndDate", Name = "idx_coupons_date")]
public partial class Coupon
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("code")]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Column("type")]
    [StringLength(20)]
    public string Type { get; set; } = null!;

    [Column("value", TypeName = "decimal(15, 2)")]
    public decimal Value { get; set; }

    [Column("min_order_value", TypeName = "decimal(15, 2)")]
    public decimal? MinOrderValue { get; set; }

    [Column("max_discount", TypeName = "decimal(15, 2)")]
    public decimal? MaxDiscount { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("used_count")]
    public int? UsedCount { get; set; }

    [Column("start_date", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column("end_date", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string? Status { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }
}
