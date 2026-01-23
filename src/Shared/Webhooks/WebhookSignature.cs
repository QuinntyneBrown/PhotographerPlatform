using Shared.Security;

namespace Shared.Webhooks;

public static class WebhookSignature
{
    public static string Create(string payload, string secret)
    {
        return HmacSignature.ComputeHex(payload, secret);
    }

    public static bool Verify(string payload, string signature, string secret)
    {
        return HmacSignature.VerifyHex(payload, secret, signature);
    }
}
