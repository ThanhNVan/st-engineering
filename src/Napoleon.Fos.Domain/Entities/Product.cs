using Napoleon.Shared.Domain.AuditTrails;
using Napoleon.Shared.Domain.Entity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Napoleon.Fos.Domain.Entities;

public class Product : BaseEntity, IDeletingAuditTrail, IModifyingAuditTrail, IAddingAuditTrail
{
    public string Name { get; set; }

    public string Description { get; set; }
    
    public string ImageUrl { get; set; }

    [InverseProperty("Product")]
    //[JsonIgnore]
    public virtual ICollection<ProductDetail>? ProductDetails { get; set; }
}
