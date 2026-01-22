namespace Shared.LabFulfillment;

public interface ILabFulfillmentProvider
{
    Task<LabOrderResult> SubmitOrderAsync(PrintJobRequest request, CancellationToken ct = default);
    Task<LabOrderResult> GetOrderStatusAsync(string labOrderId, CancellationToken ct = default);
    Task<LabOrderResult> CancelOrderAsync(string labOrderId, CancellationToken ct = default);
    Task<IReadOnlyList<string>> GetAvailableProductsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<string>> GetAvailableShippingMethodsAsync(string countryCode, CancellationToken ct = default);
}
