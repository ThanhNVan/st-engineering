using Microsoft.EntityFrameworkCore;
using Napoleon.Shared.Domain.AuditTrails;
using Napoleon.Shared.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Napoleon.Fos.Domain.Entities;


[Index(nameof(Email), IsUnique = true)]
public class Customer : BaseEntity, IDeletingAuditTrail, IModifyingAuditTrail, IAddingAuditTrail
{
    public string Name { get; set; }

    public string Email { get; set; }

    public int Age { get; set; }

    public string Password { get; set; }

    #region [ Properties - Virtual ]
    [InverseProperty("Customer")]
    public virtual ICollection<AuthenticationToken>? AuthenticationTokens { get; set; }
    #endregion
}
