using Microsoft.Extensions.DependencyInjection;

namespace Luminous
{
    public static class SolutionAssemblyMetadataServiceExtension
    {
        public static void AddLuminousAssemblyMetadata(this WebApplicationBuilder builder)
        {
            builder.Host.ConfigureServices((context, services) =>
            {
                //var path = context.HostingEnvironment.ContentRootPath;
                //var projectName = GetLastDirectoryName(path);
                //var solution = GetSolutionName(projectName);

                services.AddSingleton<ISolutionAssemblyMetadata>(serviceProvider => new SolutionAssemblyMetadata(null));
            });
        }

        public static void AddLuminousAssemblyMetadata(this WebApplicationBuilder builder, Predicate<string> assemblyNamePredicate)
        {
            if (assemblyNamePredicate == null)
            {
                throw new ArgumentNullException(nameof(assemblyNamePredicate));
            }

            builder.Host.ConfigureServices((context, services) =>
            {
                services.AddSingleton<ISolutionAssemblyMetadata>(serviceProvider => new SolutionAssemblyMetadata(assemblyNamePredicate));
            });
        }

        public static string GetLastDirectoryName(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                return string.Empty;
            }

            directoryPath = directoryPath.TrimEnd(Path.DirectorySeparatorChar);
            return Path.GetFileName(directoryPath);
        }

        public static string GetSolutionName(string projectName)
        {
            var array = projectName.Split('.').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (array.Length == 1)
            {
                return array[0];
            }
            else
            {
                var guess = array.FirstOrDefault(x => !x.Contains("api", StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(guess))
                {
                    return guess;
                }

                return array[0];
            }
        }
    }
}
