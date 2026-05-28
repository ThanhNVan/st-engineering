using Napoleon.Shared.Domain.AuditTrails;
using Napoleon.Shared.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Napoleon.Fos.Domain.Entities;

public class ProductDetail : BaseEntity, IDeletingAuditTrail, IModifyingAuditTrail, IAddingAuditTrail
{

    public string Varience { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public Guid ProductId { get; set; }

    [InverseProperty("ProductDetails")]
    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; }
}

