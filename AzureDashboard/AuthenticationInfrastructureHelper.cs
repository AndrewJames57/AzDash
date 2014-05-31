using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure;
using NLog;
using System;
using System.Threading;

namespace AzureDashboard
{
    /// <summary>
    /// Provides shortcuts for creating instances of the Token Credential using ADAL.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public class TokenCredentialHelper<T>
        where T : ITokenCredentialConfiguration, new()
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static T configuration = new T();

        private static string GetAuthorizationHeader()
        {
            AuthenticationResult result = null;
            //string x = log.Name;
            try
            {
                var context = new AuthenticationContext(
                        string.Format("https://login.windows.net/{0}",
                            configuration.GetTenantId()));

                var thread = new Thread(() =>
                {
                    try
                    {
                        result = context.AcquireToken(
                                        clientId: configuration.GetClientId(),
                                        redirectUri: new Uri(configuration.GetRedirectUrl()),
                                        resource: "https://management.core.windows.net/",
                                        promptBehavior: PromptBehavior.Auto);
                    }
                    catch (Exception ex)
                    {
                        Common.LogError(ex, log.Name, 0);
                        log.Info("Process steps halted with errors. See log.");
                        return;
                    }
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Name = "AquireTokenThread";
                thread.Start();
                thread.Join();
            }
            catch (Exception ex)
            {
                Common.LogError(ex, log.Name);
                log.Debug("Process steps halted with errors. See log.");
            }
            return result.CreateAuthorizationHeader().Substring("Bearer ".Length);
        }

        /// <summary>
        /// Hands back the credential.
        ///
        /// Credentials don't need to belong to a specific subscription
        /// a subscription needs to be accessed. In that case, the
        /// AAD tenant & app need to be "blessed," or the app needs
        /// to be accessing assets in the same subscription.
        ///
        /// Calling code can create a general-purpose credential,
        /// mainly to be used to get a list of subscriptions.
        ///
        /// Once the desired subscription is found, the token can be
        /// re-used in conjunction with a subscription ID, to provide
        /// direct management access via the Azure API to manage
        /// assets that are under that same subscription.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        public static TokenCloudCredentials GetCredentials(string subscriptionId = null)
        {
            var token = GetAuthorizationHeader();

            if (subscriptionId == null)
                return new TokenCloudCredentials(token);
            else
                return new TokenCloudCredentials(subscriptionId, token);
        }
    }
}