using Napoleon.Shared.Domain.AuditTrails;
using Napoleon.Shared.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Napoleon.Fos.Domain.Entities;

public class AuthenticationToken : BaseEntity, IDeletingAuditTrail, IModifyingAuditTrail, IAddingAuditTrail
{
    public string Token { get; set; }

    public DateTimeOffset ValidTill { get; set; }

    public Guid CustomerId { get; set; }

    #region [ Properties - Virtual ]
    [ForeignKey(nameof(CustomerId))]
    [InverseProperty("AuthenticationTokens")]
    public virtual Customer Customer { get; set; }
    #endregion
}
