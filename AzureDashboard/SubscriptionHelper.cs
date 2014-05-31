using James.Utils.String;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Subscriptions;
using Microsoft.WindowsAzure.Subscriptions.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDashboard
{
    /// <summary>
    /// The subscription portion of the demo.
    /// </summary>
    public class SubscriptionHelper
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static readonly string mySubscription = ConfigurationManager.AppSettings["MySubscription"];
        private static readonly string toggle = ConfigurationManager.AppSettings["Toggle"];
        private static IEnumerable<SubscriptionListOperationResponse.Subscription> subscriptions;
        public static SubscriptionListOperationResponse.Subscription mainSubscription;

        //private static SubscriptionInfo subscriptionInfo;
        //private static TokenCloudCredentials credential;

        /// <summary>
        /// Gets a list of subscriptions and then iterates over the subscriptions if the calling
        /// code has provided a filter.
        /// </summary>
        /// <param name="credentials">The credentials for the authenticated client.</param>
        /// <param name="filter">
        /// A string that will be used to search the name of the subscription from a user's
        /// subscription list.
        /// </param>
        /// <returns>An instance of a subscription API model.</returns>
        public static SubscriptionListOperationResponse.Subscription
                    SelectSubscription(SubscriptionCloudCredentials credentials,
                    string filter)
        {
            try
            {
                IEnumerable<SubscriptionListOperationResponse.Subscription> ret = null;
                SubscriptionListOperationResponse.Subscription result;
                using (var subscriptionClient = new SubscriptionClient(credentials))
                {
                    var listSubscriptionResults =
                        subscriptionClient.Subscriptions.List();
                    ret = listSubscriptionResults.Subscriptions;
                    result = ret.First(x => x.SubscriptionName.Contains(filter));
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Common.LogError(ex, log.Name, 1);
                return null;
            }
        }

        public static IEnumerable<SubscriptionListOperationResponse.Subscription>
            ListSubscriptions(SubscriptionCloudCredentials credentials)
        {
            try
            {
                IEnumerable<SubscriptionListOperationResponse.Subscription> ret = null;

                using (var subscriptionClient = new SubscriptionClient(credentials))
                {
                    var listSubscriptionResults = subscriptionClient.Subscriptions.List();
                    var subscriptions = listSubscriptionResults.Subscriptions;
                    ret = subscriptions;
                }
                return ret;
            }
            catch (System.Exception ex)
            {
                Common.LogError(ex, log.Name, 1);
                return null;
            }
        }

        public static SubscriptionInfo SubscriptionSelector(TokenCloudCredentials localCred)
        {
            SubscriptionInfo subs = new SubscriptionInfo();
            try
            {
                subscriptions = SubscriptionHelper.ListSubscriptions(localCred);
                string subSelectName = mySubscription;
                if (toggle == "On")
                {
                    subSelectName = Menus.SubSelect(subscriptions);
                }
                log.Debug("Associate the credential with a specific subscription: " + subSelectName);
                mainSubscription = SubscriptionHelper.SelectSubscription(localCred, subSelectName);
                localCred = new TokenCloudCredentials(mainSubscription.SubscriptionId, localCred.Token);
                log.Debug("Subscription associated: " + mainSubscription.SubscriptionName);
                subs.Name = mainSubscription.SubscriptionName;
                subs.credential = localCred;
                subs.Id = localCred.SubscriptionId;
                log.Debug("Subscription ID: " + localCred.SubscriptionId);
                return subs;
            }
            catch (Exception ex)
            {
                Common.LogError(ex, log.Name);
                log.Error("Subscription Selector halted with errors. See log.");
                return null;
            }
        }
    }
}