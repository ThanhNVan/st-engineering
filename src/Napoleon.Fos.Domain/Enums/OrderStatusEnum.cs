namespace Napoleon.Fos.Domain.Enums;

public enum OrderStatus
{
    Created,
    Processing,
    InDelivery,
    Delivered,
    Failed,
    Success,
}

public enum OrderPaymentStatus
{
    Success,
    Processing,
    Failed,
}
