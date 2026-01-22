namespace Shared.Payments;

public interface IPaymentProvider
{
    Task<PaymentResult> AuthorizeAsync(CardPaymentRequest request, CancellationToken ct = default);
    Task<PaymentResult> CaptureAsync(string paymentId, decimal? amount = null, CancellationToken ct = default);
    Task<PaymentResult> AuthorizeAndCaptureAsync(CardPaymentRequest request, CancellationToken ct = default);
    Task<PaymentResult> RefundAsync(RefundRequest request, CancellationToken ct = default);
    Task<PaymentResult> GetPaymentAsync(string paymentId, CancellationToken ct = default);
}
