using Napoleon.Shared.Domain.AuditTrails;

namespace Napoleon.Shared.Domain.Entity;

public class AuditTrail : BaseEntity
{
    public Guid EntityId  { get; set; }

    public required string EntityType { get; set; }

    public required AuditType AuditType { get; set; }

    public Guid TakenBy { get; set; }

    public required string BeforeValue { get; set; }

    public required string AfterValue { get; set; }

    public required string ChangedValue { get; set; }
}

public class AuditDelta
{
    public string FieldName { get; set; }
    public string ValueBefore { get; set; }
    public string ValueAfter { get; set; }
}
