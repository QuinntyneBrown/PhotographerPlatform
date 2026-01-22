namespace Shared.Webhooks;

public enum WebhookDeliveryStatus
{
    Pending,
    Delivered,
    Failed,
    Retrying
}
