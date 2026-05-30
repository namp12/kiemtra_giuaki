using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Models;

[Table("orders")]
[Index("OrderNumber", Name = "UQ__orders__730E34DFC1D48D4B", IsUnique = true)]
[Index("CustomerId", "OrderStatus", Name = "idx_orders_customer_status")]
[Index("OrderedAt", Name = "idx_orders_date")]
public partial class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_number")]
    [StringLength(50)]
    public string OrderNumber { get; set; } = null!;

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("total_amount", TypeName = "decimal(15, 2)")]
    public decimal TotalAmount { get; set; }

    [Column("shipping_fee", TypeName = "decimal(15, 2)")]
    public decimal? ShippingFee { get; set; }

    [Column("discount_amount", TypeName = "decimal(15, 2)")]
    public decimal? DiscountAmount { get; set; }

    [Column("tax_amount", TypeName = "decimal(15, 2)")]
    public decimal? TaxAmount { get; set; }

    [Column("final_amount", TypeName = "decimal(15, 2)")]
    public decimal FinalAmount { get; set; }

    [Column("payment_method")]
    [StringLength(50)]
    public string? PaymentMethod { get; set; }

    [Column("payment_status")]
    [StringLength(50)]
    public string? PaymentStatus { get; set; }

    [Column("order_status")]
    [StringLength(50)]
    public string? OrderStatus { get; set; }

    [Column("shipping_address")]
    public string ShippingAddress { get; set; } = null!;

    [Column("shipping_phone")]
    [StringLength(20)]
    public string? ShippingPhone { get; set; }

    [Column("shipping_name")]
    [StringLength(255)]
    public string? ShippingName { get; set; }

    [Column("note")]
    public string? Note { get; set; }

    [Column("ordered_at", TypeName = "datetime")]
    public DateTime? OrderedAt { get; set; }

    [Column("delivered_at", TypeName = "datetime")]
    public DateTime? DeliveredAt { get; set; }

    [Column("cancelled_at", TypeName = "datetime")]
    public DateTime? CancelledAt { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual Customer Customer { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
