using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using NLog;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace AzureDashboard
{
    public static class CloudServiceHelper
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static readonly string mySubscription = ConfigurationManager.AppSettings["MySubscription"];
        private static readonly string toggle = ConfigurationManager.AppSettings["Toggle"];

        public async static Task<string> CreateCloudService(
            this SubscriptionCloudCredentials credentials, string serviceName, string affinityGroup
            )
        {
            using (var computeManagementClient = new ComputeManagementClient(credentials))
            {
                try
                {
                    log.Debug("Creating Cloud Service: " + serviceName);
                    var createHostedServiceResult = await computeManagementClient.HostedServices.CreateAsync(
                        new HostedServiceCreateParameters
                        {
                            //AffinityGroup = affinityGroup,
                            Label = "CompoundCloudService",
                            AffinityGroup = affinityGroup,
                            ServiceName = serviceName
                        });
                    log.Info(serviceName + " Cloud Service created.");
                }
                catch (Exception ex)
                {
                    Common.LogError(ex, log.Name);
                    log.Debug("Process steps halted with errors. See log.");
                    return null;
                }
            }
            return serviceName;
        }

        public static HostedServiceListResponse ListCloudServices(SubscriptionCloudCredentials credentials)
        {
            HostedServiceListResponse serviceList = new HostedServiceListResponse();
            try
            {
                using (var computeManagementClient = new ComputeManagementClient(credentials))
                {
                    serviceList = computeManagementClient.HostedServices.List();

                    if (toggle == "On")
                    {
                        Menus.CloudServiceDisplay(serviceList);
                    }
                    else
                    {
                        foreach (var item in serviceList)
                        {
                            log.Debug("Cloud Service: " + item.ServiceName);
                        }
                    }
                }
                return serviceList;
            }
            catch (Exception ex)
            {
                Common.LogError(ex, log.Name);
                log.Info("Process step halted with errors. See log.");
                return null;
            }
        }

        public async static Task<string>
            CheckCloudService(SubscriptionCloudCredentials credentials, string serviceName)
        {
            using (var computeManagementClient = new ComputeManagementClient(credentials))
            {
                try
                {
                    //AffinityGroupGetResponse
                    var check = await computeManagementClient.HostedServices.GetAsync(serviceName);
                }
                catch (System.Exception)
                {
                    log.Info("Cloud Service " + serviceName + " does not exist.");
                    return "";
                }
            }
            return "Exists.";
        }
    }
}