using James.Utils.String;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.Models;
using Microsoft.WindowsAzure.Management.Sql;
using Microsoft.WindowsAzure.Management.Sql.Models;
using Microsoft.WindowsAzure.Management.Storage.Models;
using Microsoft.WindowsAzure.Subscriptions;
using Microsoft.WindowsAzure.Subscriptions.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDashboard
{
    public static class Record
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static readonly string mySubscription = ConfigurationManager.AppSettings["MySubscription"];

        public static void recHeader(string subName)
        {
            log.Trace("___________________________________________________");
            log.Trace("  Environment Verification");
            log.Trace("___________________________________________________");
            log.Trace("Environment: " + subName);
            log.Trace("Date stamp:  " + DateTime.Now);
            log.Trace("___________________________________________________");
        }

        public static void recAffinityGroups(AffinityGroupListResponse affinityGroupList)
        {
            log.Trace("___________________________________________________");
            log.Trace("             Affinity Groups ");
            log.Trace("___________________________________________________");
            foreach (var item in affinityGroupList)
            {
                log.Trace("Affinty Group: " + item.Name);
                log.Trace("Description:   " + item.Description);
                log.Trace("Date created:  " + item.CreatedTime);
                log.Trace("Location:      " + item.Location);
                log.Trace("___________________________________________________");
            }
        }

        public static void recCloudServices(HostedServiceListResponse serviceList)
        {
            log.Trace("___________________________________________________");
            log.Trace("             Cloud Services ");
            log.Trace("___________________________________________________");
            foreach (var item in serviceList)
            {
                log.Trace("Cloud Service: " + item.ServiceName);
                log.Trace("Affinity Group:" + item.Properties.AffinityGroup);
                log.Trace("Date created:  " + item.Properties.DateCreated);
                log.Trace("Description:   " + item.Properties.Description);
                log.Trace("Location:      " + item.Properties.Location);
                log.Trace("___________________________________________________");
            }
        }

        public static void recStorageAccounts(StorageAccountListResponse storage)
        {
            log.Trace("___________________________________________________");
            log.Trace("             Storage Accounts ");
            log.Trace("___________________________________________________");
            foreach (var item in storage)
            {
                log.Trace("Storage Account: " + item.Name);
                log.Trace("Affinity Group:  " + item.Properties.AffinityGroup);
                log.Trace("Georeplication:  " + item.Properties.GeoReplicationEnabled);
                log.Trace("Description:     " + item.Properties.Description);
                var x = item.Properties.
                
                log.Trace("___________________________________________________");
            }
        }

        public static void recVirtualMachines(StorageAccountListResponse storage)
        {
            log.Trace("___________________________________________________");
            log.Trace("             Virtual Machines ");
            log.Trace("___________________________________________________");
        }

        public static void recSqlServers(ServerListResponse sqlServerList)
        {
            log.Trace("___________________________________________________");
            log.Trace("             SQL  Servers ");
            log.Trace("___________________________________________________");
            foreach (var item in sqlServerList)
            {
                log.Trace("SQL Server:    " + item.Name);
                log.Trace("Location:      " + item.Location);
                log.Trace("Description:   " + item.AdministratorUserName);
                log.Trace("Version:       " + item.Version);
                log.Trace("___________________________________________________");
            }
        }

        public static void recSqlDatabases(DatabaseListResponse SqlDatabaseList, string sqlServerName)
        {
            log.Trace("___________________________________________________");
            log.Trace("  " + sqlServerName + ": SQL Databases");
            log.Trace("___________________________________________________");
            foreach (var item in SqlDatabaseList)
            {
                log.Trace("Database name: " + item.Name);
                log.Trace("Edition:       " + item.Edition);
                log.Trace("Date created: " + item.CreationDate);
                log.Trace("Size in MB:    " + item.SizeMB);
                log.Trace("___________________________________________________");
            }
        }
    }
}