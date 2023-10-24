
using System;
using System.Collections.Generic;
using EntityFrameworkCore.EncryptColumn.Attribute;
namespace back.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    [EncryptColumn]
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string State { get; set; }
    public bool Status { get; set; }

    [ForeignKey(nameof(Role))]
    public int Id_role { get; set; }

    public DateTime Created_at { get; set; } = DateTime.UtcNow;

    public DateTime Updated_at { get; set; } = DateTime.UtcNow;


    [ForeignKey("Id_role")]

    public virtual Role Role { get; set; }

}