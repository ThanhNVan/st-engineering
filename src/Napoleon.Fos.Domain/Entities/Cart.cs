using Napoleon.Shared.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Napoleon.Fos.Domain.Entities;

public class Cart : BaseEntity
{
    public Guid CustomerId { get; set; }

    public Guid ProductDetailId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
    public int Quantity { get; set; }

    #region [ Properties - Virtual ]
    [ForeignKey(nameof(CustomerId))]
    [InverseProperty("Carts")]
    public virtual Customer Customer { get; set; }
    
    [ForeignKey(nameof(ProductDetailId))]
    [InverseProperty("Carts")]
    public virtual ProductDetail ProductDetail { get; set; }
    #endregion
}
