namespace Luminous
{
    public static class LuminousAppBuilderExtensions
    {
        public static WebApplication UseLuminous(this WebApplication app)
        {
            app.UseLuminousServiceLocator();
            app.UseLuminousGlobalException();
            app.UseLuminousDebug();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
