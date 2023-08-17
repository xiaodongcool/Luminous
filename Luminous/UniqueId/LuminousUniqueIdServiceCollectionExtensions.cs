namespace Microsoft.Extensions.DependencyInjection;

public static class LuminousUniqueIdServiceCollectionExtensions
{
    /// <summary>
    ///     添加雪花id
    /// </summary>
    public static void AddLuminousUniqueId(this IServiceCollection services)
    {
        services.AddSingleton<ILuminousUniqueId, LuminousUniqueId>();
    }
}