using Napoleon.Shared.Domain.AuditTrails;
using Napoleon.Shared.Domain.Entity;

namespace Napoleon.Fos.Domain.Entities;

public class Product : BaseEntity, IDeletingAuditTrail, IModifyingAuditTrail, IAddingAuditTrail
{
    public string Name { get; set; }

    public string Description { get; set; }
    
    public string ImageUrl { get; set; }
}
