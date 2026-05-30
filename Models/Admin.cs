using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Models;

[Table("admins")]
[Index("Email", Name = "UQ__admins__AB6E6164CE73F557", IsUnique = true)]
[Index("Username", Name = "UQ__admins__F3DBC5720A7E83A5", IsUnique = true)]
public partial class Admin
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("password")]
    [StringLength(255)]
    public string Password { get; set; } = null!;

    [Column("fullname")]
    [StringLength(255)]
    public string? Fullname { get; set; }

    [Column("role")]
    [StringLength(50)]
    public string? Role { get; set; }

    [Column("avatar")]
    [StringLength(500)]
    public string? Avatar { get; set; }

    [Column("last_login", TypeName = "datetime")]
    public DateTime? LastLogin { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string? Status { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("RepliedByNavigation")]
    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    [InverseProperty("Admin")]
    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
}
