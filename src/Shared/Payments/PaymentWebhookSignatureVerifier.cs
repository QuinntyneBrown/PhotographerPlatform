using Shared.Security;

namespace Shared.Payments;

public static class PaymentWebhookSignatureVerifier
{
    public static bool Verify(string payload, string signature, string secret)
    {
        return HmacSignature.VerifyHex(payload, secret, signature);
    }
}
