using System.Net.Http.Headers;

namespace Shared.Http;

public static class HttpRequestMessageExtensions
{
    public static async Task<HttpRequestMessage> CloneAsync(this HttpRequestMessage request, CancellationToken ct)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version,
            VersionPolicy = request.VersionPolicy
        };

        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (request.Content is not null)
        {
            var contentBytes = await request.Content.ReadAsByteArrayAsync(ct).ConfigureAwait(false);
            var byteContent = new ByteArrayContent(contentBytes);
            foreach (var header in request.Content.Headers)
            {
                byteContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            clone.Content = byteContent;
        }

        return clone;
    }
}
