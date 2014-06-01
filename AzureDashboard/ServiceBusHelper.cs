using James.Utils.String;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.Models;
using Microsoft.WindowsAzure.Management.ServiceBus;
using Microsoft.WindowsAzure.Management.Sql;
using Microsoft.WindowsAzure.Management.Sql.Models;
using Microsoft.WindowsAzure.Management.Storage.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDashboard
{
    public static class ServiceBusHelper
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static readonly string toggle = ConfigurationManager.AppSettings["Toggle"];

        public static void ServiceBusList(SubscriptionCloudCredentials credentials)
        {
            //ServiceBusManagementClient sbMangeClient;
        }
    }
}