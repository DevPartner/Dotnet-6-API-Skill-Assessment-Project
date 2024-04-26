using ChuckNorris.Core.Models;
using ChuckNorris.Core.Models.Configs;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace ChuckNorris.Core.Services.Transformations;

public class ReplacementTransformation : IJokeTransformation
{
    public IReadOnlyCollection<ReplaceRuleConfig> ReplaceRules { get; set; } = new List<ReplaceRuleConfig>();

    public ReplacementTransformation(IConfigurationSection transformConfig)
    {
        transformConfig.Bind(this);
    }

    public Task<JokeModel> TransformJokeAsync(JokeModel joke, CancellationToken token = default)
    {

        foreach (var rule in ReplaceRules)
        {
            joke.Value = Regex.Replace(joke.Value, @"\b" + Regex.Escape(rule.OldValue) + @"\b", rule.NewValue, RegexOptions.Multiline, TimeSpan.FromSeconds(1));
        }

        return Task.FromResult(joke);
    }
}
