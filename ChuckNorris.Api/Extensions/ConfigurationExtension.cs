namespace ChuckNorris.Api.Extensions;

public static class ConfigurationExtension
{
    public static T GetRequiredValue<T>(this IConfiguration configuration, string? section = null)
    {
        section ??= typeof(T).Name;
        return configuration.GetRequiredSection(section).Get<T>() ??
               throw new KeyNotFoundException($"Key: {section}");
    }
}
