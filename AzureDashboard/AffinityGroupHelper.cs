using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management;
using Microsoft.WindowsAzure.Management.Models;
using Microsoft.WindowsAzure.Management.Storage;
using Microsoft.WindowsAzure.Management.Storage.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using James.Utils.String;

namespace AzureDashboard
{
    public static class AffinityGroupHelper
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static readonly string toggle = ConfigurationManager.AppSettings["Toggle"];

        public async static Task<string>
            CreateAffinityGroup(SubscriptionCloudCredentials credentials, string affinityName, string location)
        {
            log.Debug("Create Affinity Group module");
            using (var managementClient = new ManagementClient(credentials))
            {
                try
                {
                    var result = await managementClient.AffinityGroups.CreateAsync(
                                        new AffinityGroupCreateParameters
                                        {
                                            Name = affinityName,
                                            Label = affinityName,
                                            Description = affinityName + " Affinity Group",
                                            Location = location //LocationNames.NorthEurope
                                        });
                }
                catch (Exception ex)
                {
                    Common.LogError(ex, log.Name, 1);
                    return "Failed to create affintiy group";
                }
            }
            return "created!";
        }

        public async static Task<string> CheckAffinityGroup(SubscriptionCloudCredentials credentials, string affinityName)
        {
            using (var managementClient = new ManagementClient(credentials))
            {
                try
                {
                    //AffinityGroupGetResponse
                    AffinityGroupGetResponse check = await managementClient.AffinityGroups.GetAsync(affinityName);
                }
                catch (System.Exception)
                {
                    log.Debug("Affinity Group " + affinityName + " does not exist.");
                    return "";
                }
            }
            return "Affinity Group - " + affinityName + " already exists.";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public static AffinityGroupListResponse ListAffinityGroups(SubscriptionCloudCredentials credentials)
        {
            using (var managementClient = new ManagementClient(credentials))
            {
                try
                {
                    AffinityGroupListResponse result = managementClient.AffinityGroups.List();

                    if (toggle == "On")
                    {
                        Menus.AffinityDisplay(result);
                    }
                    else
                    {
                        foreach (var item in result)
                        {
                            log.Debug("Affinity Group: " + item.Name);
                        }
                    } 
                    return result;
                }
                catch (System.Exception ex)
                {
                    Common.LogError(ex, log.Name, 1);
                    return null;
                }
            }
        }
    }
}