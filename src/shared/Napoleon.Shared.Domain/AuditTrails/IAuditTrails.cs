using Napoleon.Shared.Common;
using Napoleon.Shared.Domain.Entity;

namespace Napoleon.Shared.Domain.AuditTrails;

public interface IDeletingAuditTrail;
public interface IModifyingAuditTrail;
public interface IAddingAuditTrail;

public enum AuditType
{
    Adding,
    Modifying,
    Deleting
}

public static class AuditHelper
{
    public static AuditTrail GetDeletingAuditTrail(this BaseEntity baseEntity)
    {
        return new AuditTrail
        {
            EntityId = baseEntity.Id,
            EntityType = baseEntity.GetType().Name,
            AfterValue = "",
            AuditType = AuditType.Deleting,
            BeforeValue = baseEntity.ToJson(),
            ChangedValue = "",
        };
    }
    public static AuditTrail GetAddingAuditTrail(this BaseEntity baseEntity)
    {
        return new AuditTrail
        {
            EntityId = baseEntity.Id,
            EntityType = baseEntity.GetType().Name,
            AfterValue = baseEntity.ToJson(),
            AuditType = AuditType.Adding,
            BeforeValue = "",
            ChangedValue = "",
        };
    }

    public static AuditTrail GetModifyingAuditTrail(this BaseEntity beforeValue, BaseEntity afterValue, IEnumerable<AuditDelta> deltaList)
    {
        return new AuditTrail
        {
            EntityId = beforeValue.Id,
            EntityType = beforeValue.GetType().Name,
            AfterValue = afterValue.ToJson(),
            AuditType = AuditType.Modifying,
            BeforeValue = beforeValue.ToJson(),
            ChangedValue = deltaList.ToJson(),
        };
    }
}