using System.Text.RegularExpressions;
using PhotographerPlatform.Communication.Core.Models;

namespace PhotographerPlatform.Communication.Core.Services;

public sealed class SimpleTemplateRenderer : ITemplateRenderer
{
    private static readonly Regex TokenPattern = new("{{\s*(?<key>[^\s}]+)\s*}}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public RenderedTemplate Render(EmailTemplate template, IReadOnlyDictionary<string, string> variables)
    {
        return new RenderedTemplate
        {
            Subject = ReplaceTokens(template.Subject, variables),
            Body = ReplaceTokens(template.Body, variables)
        };
    }

    private static string ReplaceTokens(string input, IReadOnlyDictionary<string, string> variables)
    {
        return TokenPattern.Replace(input, match =>
        {
            var key = match.Groups["key"].Value;
            if (variables.TryGetValue(key, out var value))
            {
                return value;
            }

            return match.Value;
        });
    }
}
