using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Models;

[Table("inventory_transactions")]
public partial class InventoryTransaction
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("admin_id")]
    public int AdminId { get; set; }

    [Column("transaction_type")]
    [StringLength(20)]
    public string TransactionType { get; set; } = null!;

    [Column("quantity_change")]
    public int QuantityChange { get; set; }

    [Column("old_quantity")]
    public int OldQuantity { get; set; }

    [Column("new_quantity")]
    public int NewQuantity { get; set; }

    [Column("reason")]
    public string? Reason { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("AdminId")]
    [InverseProperty("InventoryTransactions")]
    public virtual Admin Admin { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("InventoryTransactions")]
    public virtual Product Product { get; set; } = null!;
}
