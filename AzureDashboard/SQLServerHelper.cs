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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDashboard
{
    public static class SQLServerHelper
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        //private static TokenCloudCredentials credential;
        private static readonly string toggle = ConfigurationManager.AppSettings["Toggle"];

        public static ServerListResponse ListSQLServer(SubscriptionCloudCredentials credentials)
        {
            using (var sqlManagementClient = new SqlManagementClient(credentials))
            {
                ServerListResponse SqlServerList = sqlManagementClient.Servers.List();
                if (toggle == "On")
                {
                    Menus.SqlServerDisplay(SqlServerList);
                }
                else
                {
                    foreach (var item in SqlServerList)
                    {
                        log.Debug("SQL Server: " + item.Name);
                    }
                }
                return SqlServerList;
            }
        }

        public static DatabaseListResponse ListSQLDatabases(string sqlServerName, SubscriptionCloudCredentials credentials)
        {
            DatabaseListResponse SqlDatabaseList;
            using (var sqlManagementClient = new SqlManagementClient(credentials))
            {
                SqlDatabaseList = sqlManagementClient.Databases.List(sqlServerName);
            }
            if (toggle == "On")
            {
                Menus.SqlDatabasesDisplay(SqlDatabaseList, sqlServerName);
            }
            else
            {
                foreach (var item in SqlDatabaseList)
                {
                    log.Debug("SQL Server: " + item.Name);
                }
            }
            return SqlDatabaseList;
        }
    }
}