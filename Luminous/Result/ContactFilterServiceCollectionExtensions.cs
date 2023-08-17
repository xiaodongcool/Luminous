using Luminous.ModelValidation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContactFilterServiceCollectionExtensions
    {
        /// <summary>
        ///     格式化接口响应报文内容, 将所有接口请求响应报文内容格式化为 <see cref="IResult"/>
        /// </summary>
        public static void AddLuminousResult(this IServiceCollection services)
        {
            services.AddControllers(mvc =>
            {
                mvc.Filters.TryAdd<ModelBindFailFilter>();
                mvc.Filters.TryAdd<DataStructureOptimizeResultFilter>();
                mvc.Filters.TryAdd<UnifyResultFilter>();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
