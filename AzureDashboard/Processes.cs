using James.Utils.String;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.Models;
using Microsoft.WindowsAzure.Management.Sql;
using Microsoft.WindowsAzure.Management.Sql.Models;
using Microsoft.WindowsAzure.Management.Storage.Models;
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
    public static class Processes
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static TokenCloudCredentials credential;

        public static void ReadProcess()
        {
            //string affinityGroupResult;
            log.Info("Starting Read Process steps.");
            try
            {
                SubscriptionInfo subs = new SubscriptionInfo();
                log.Debug("Establish Credentials");
                subs = SecurityHelper.AccessModule(ProcessInfo.ProcessFlag);
                credential = subs.credential;
                Record.recHeader(subs.Name);
                log.Debug("List Affinity Groups");
                AffinityGroupListResponse affinityGroupList = AffinityGroupHelper.ListAffinityGroups(credential);
                Record.recAffinityGroups(affinityGroupList);
                log.Debug("List Cloud Services");
                HostedServiceListResponse serviceList = CloudServiceHelper.ListCloudServices(credential);
                Record.recCloudServices(serviceList);
                log.Debug("List Storage Accounts");
                StorageAccountListResponse storage = StorageHelper.ListStorageServices(credential);
                log.Debug("Service Bus");

                Record.recStorageAccounts(storage);
                log.Debug("List Virtual Machines");
                VirtualMachineHelper.ListVHDS(credential);
                log.Debug("List SQL Servers");
                ServerListResponse sqlServerList = SQLServerHelper.ListSQLServer(credential);
                Record.recSqlServers(sqlServerList);
                log.Debug("List SQL Databases");
                foreach (var item in sqlServerList)
                {
                    DatabaseListResponse SqlDatabaseList = SQLServerHelper.ListSQLDatabases(item.Name, credential);
                    Record.recSqlDatabases(SqlDatabaseList, item.Name);
                }
            }
            catch (Exception ex)
            {
                Common.LogError(ex, log.Name);
                log.Info("Process steps halted with errors. See log.");
            }
            finally
            {
                log.Info("Read Process Complete");
                Console.ReadLine();
            }
        }
    }
}