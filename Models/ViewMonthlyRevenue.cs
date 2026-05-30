using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Models;

[Keyless]
public partial class ViewMonthlyRevenue
{
    [Column("year")]
    public int? Year { get; set; }

    [Column("month")]
    public int? Month { get; set; }

    [Column("total_orders")]
    public int? TotalOrders { get; set; }

    [Column("total_revenue", TypeName = "decimal(38, 2)")]
    public decimal? TotalRevenue { get; set; }

    [Column("total_products_sold")]
    public int? TotalProductsSold { get; set; }
}
