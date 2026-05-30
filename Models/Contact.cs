using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Models;

[Table("contacts")]
public partial class Contact
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("phone")]
    [StringLength(20)]
    public string? Phone { get; set; }

    [Column("subject")]
    [StringLength(255)]
    public string? Subject { get; set; }

    [Column("message")]
    public string Message { get; set; } = null!;

    [Column("status")]
    [StringLength(20)]
    public string? Status { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("replied_at", TypeName = "datetime")]
    public DateTime? RepliedAt { get; set; }

    [Column("replied_by")]
    public int? RepliedBy { get; set; }

    [ForeignKey("RepliedBy")]
    [InverseProperty("Contacts")]
    public virtual Admin? RepliedByNavigation { get; set; }
}
