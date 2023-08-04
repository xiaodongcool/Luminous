namespace Microsoft.Extensions.DependencyInjection;

public static class WorkIdServiceCollectionExtensions
{
    /// <summary>
    ///     添加雪花id
    /// </summary>
    public static void AddWorkId(this IServiceCollection services)
    {
        services.AddSingleton<IWorkId, WorkId>();
    }
}