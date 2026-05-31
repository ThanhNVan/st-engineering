using Napoleon.Fos.Domain.Enums;
using Napoleon.Shared.Domain.AuditTrails;
using Napoleon.Shared.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Napoleon.Fos.Domain.Entities;

public class Order : BaseEntity, IDeletingAuditTrail, IModifyingAuditTrail, IAddingAuditTrail
{
    public Guid CustomerId { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public OrderPaymentStatus OrderPaymentStatus { get; set; }

    #region [ Properties - Virtual ]
    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail>? OrderDetails { get; set; }

    [ForeignKey(nameof(CustomerId))]
    [InverseProperty("Orders")]
    public virtual Customer Customer { get; set; }
    #endregion
}
