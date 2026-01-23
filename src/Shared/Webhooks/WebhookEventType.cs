namespace Shared.Webhooks;

public enum WebhookEventType
{
    Unknown,
    OrderCreated,
    OrderUpdated,
    OrderFulfilled,
    OrderCancelled,
    PaymentReceived,
    PaymentFailed,
    PaymentRefunded,
    GalleryCreated,
    GalleryPublished,
    GalleryViewed,
    ImageDownloaded,
    ClientFavorited
}
