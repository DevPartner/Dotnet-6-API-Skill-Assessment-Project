
namespace ChuckNorris.Core.Models.Configs;

public class ReplaceRuleConfig
{
#if NET7_0_OR_GREATER
    required
#endif
    public string OldValue { get; set; } = string.Empty;
#if NET7_0_OR_GREATER
    required
#endif
    public string NewValue { get; set; } = string.Empty;
}
