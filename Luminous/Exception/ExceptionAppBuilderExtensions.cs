namespace Luminous
{
    public static class ExceptionAppBuilderExtensions
    {
        /// <summary>
        ///     捕捉全局异常
        /// </summary>
        public static void UseCatchGlobalException(this IApplicationBuilder app)
        {
            app.UseMiddleware<LuminousExceptionMiddleware>();
        }
    }
}
