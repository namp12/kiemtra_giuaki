using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Models;

[Table("customers")]
[Index("Email", Name = "UQ__customer__AB6E61645F330AF1", IsUnique = true)]
public partial class Customer
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("password")]
    [StringLength(255)]
    public string Password { get; set; } = null!;

    [Column("fullname")]
    [StringLength(255)]
    public string Fullname { get; set; } = null!;

    [Column("phone")]
    [StringLength(20)]
    public string? Phone { get; set; }

    [Column("avatar")]
    [StringLength(500)]
    public string? Avatar { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("city")]
    [StringLength(100)]
    public string? City { get; set; }

    [Column("district")]
    [StringLength(100)]
    public string? District { get; set; }

    [Column("ward")]
    [StringLength(100)]
    public string? Ward { get; set; }

    [Column("email_verified")]
    public bool? EmailVerified { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string? Status { get; set; }

    [Column("last_login", TypeName = "datetime")]
    public DateTime? LastLogin { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Customer")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
