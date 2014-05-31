using Microsoft.WindowsAzure;
using NLog;

namespace AzureDashboard
{
    public static class SecurityHelper
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static TokenCloudCredentials credential;
        //private static SubscriptionInfo subs;

        public static SubscriptionInfo AccessModule(string processFlag)
        {
            log.Debug("Authenticating against ADAL");
            if (processFlag == "Work")
            {
                log.Debug("Get Work Credentials");
                credential = TokenCredentialHelper<WorkConfiguration>.GetCredentials();
            }
            else
            {
                log.Debug("Get Personal Credentials");
                credential = TokenCredentialHelper<MyPersonalConfiguration>.GetCredentials();
            }
            if (credential == null)
            {
                log.Debug("Unable to authenticate.");
                return null;
            }
            log.Debug("Authenticated token issued");
            log.Debug("Expand credential with subscription info.");
            SubscriptionInfo subs = new SubscriptionInfo();
            subs = SubscriptionHelper.SubscriptionSelector(credential);

            //subs.credential
            log.Debug("Credential expansion complete.");
            log.Debug("Subscription name is: " + subs.Name);
            return subs;
        }
    }
}