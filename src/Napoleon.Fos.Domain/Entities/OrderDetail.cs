using Napoleon.Shared.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Napoleon.Fos.Domain.Entities;

public class OrderDetail : BaseEntity
{
    public Guid ProductDetailId { get; set; }
    public Guid OrderId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    public string ProductDetailValue { get; set; }

    [InverseProperty("OrderDetails")]
    [ForeignKey(nameof(ProductDetailId))]
    public virtual ProductDetail? ProductDetail { get; set; }
    
    [InverseProperty("OrderDetails")]
    [ForeignKey(nameof(OrderId))]
    public virtual Order? Order { get; set; }
}
