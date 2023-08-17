namespace Luminous.Exception
{
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        ///     捕捉全局异常
        /// </summary>
        public static void UseCatchGlobalException(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
