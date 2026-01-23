using Shared.Security;

namespace Shared.LabFulfillment;

public static class LabWebhookSignatureVerifier
{
    public static bool Verify(string payload, string signature, string secret)
    {
        return HmacSignature.VerifyHex(payload, secret, signature);
    }
}
