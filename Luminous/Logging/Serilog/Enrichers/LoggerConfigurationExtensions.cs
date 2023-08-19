using Serilog;
using Serilog.Configuration;

namespace Luminous
{
    public static class LoggerConfigurationExtensions2
    {
        /// <summary>
        ///     绑定 <see cref="ClientAgentEnricher"/>
        /// </summary>
        public static LoggerConfiguration WithClientAgent(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            ArgumentGuard.CheckForNull(enrichmentConfiguration, nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<ClientAgentEnricher>();
        }

        /// <summary>
        ///     绑定 <see cref="QueryStringEnricher"/>
        /// </summary>
        public static LoggerConfiguration WithQueryString(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            ArgumentGuard.CheckForNull(enrichmentConfiguration, nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<QueryStringEnricher>();
        }

        /// <summary>
        ///     绑定 <see cref="AccountUniqueIdEnricher"/>
        /// </summary>
        public static LoggerConfiguration WithAccountUniqueId(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            ArgumentGuard.CheckForNull(enrichmentConfiguration, nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<AccountUniqueIdEnricher>();
        }

        /// <summary>
        ///     绑定 <see cref="UserNameEnricher"/>
        /// </summary>
        public static LoggerConfiguration WithUserName(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            ArgumentGuard.CheckForNull(enrichmentConfiguration, nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<UserNameEnricher>();
        }
    }
}