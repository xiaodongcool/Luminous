namespace Luminous
{
    public static class ServiceLocatorExtension
    {
        public static IApplicationBuilder UseLuminousServiceLocator(this WebApplication app)
        {
            ServiceLocator.Initialize(app.Services);

            return app;
        }
    }
}
