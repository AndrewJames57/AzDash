using NLog;
using System.Configuration;

namespace AzureDashboard
{
    /// <summary>
    /// Provides the basic data points needed to support AAD authentication in an app making use of
    /// the management libraries.
    /// </summary>
    public interface ITokenCredentialConfiguration
    {
        string GetTenantId();

        string GetClientId();

        string GetRedirectUrl();
    }

    /// <summary>
    /// My personal configuration. :)
    /// </summary>
    public class MyPersonalConfiguration : ITokenCredentialConfiguration
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public string GetTenantId()
        {
            return ConfigurationManager.AppSettings["TenantId"];
        }

        public string GetClientId()
        {
            return ConfigurationManager.AppSettings["ClientId"];
        }

        public string GetRedirectUrl()
        {
            return ConfigurationManager.AppSettings["RedirectUrl"];
        }
    }

    public class WorkConfiguration : ITokenCredentialConfiguration
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public string GetTenantId()
        {
            return ConfigurationManager.AppSettings["WTenantId"];
        }

        public string GetClientId()
        {
            return ConfigurationManager.AppSettings["WClientId"];
        }

        public string GetRedirectUrl()
        {
            return ConfigurationManager.AppSettings["WRedirectUrl"];
        }
    }
}