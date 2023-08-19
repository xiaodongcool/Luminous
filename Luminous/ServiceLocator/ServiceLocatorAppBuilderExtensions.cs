namespace Luminous
{
    public static class ServiceLocatorAppBuilderExtensions
    {
        public static IApplicationBuilder UseLuminousServiceLocator(this WebApplication app)
        {
            ServiceLocator.Initialize(app.Services);

            return app;
        }
    }
}
