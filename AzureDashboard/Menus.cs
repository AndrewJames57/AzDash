using James.Utils.String;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.Models;
using Microsoft.WindowsAzure.Management.Sql.Models;
using Microsoft.WindowsAzure.Management.Storage.Models;
using Microsoft.WindowsAzure.Subscriptions.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace AzureDashboard
{
    internal class Menus
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static readonly string mySubscription = ConfigurationManager.AppSettings["MySubscription"];
        private static string processFlag;
        private static string dividerLine = "___________________________________________________";

        public static string Main1()
        {
            Console.WriteLine(dividerLine);
            Console.WriteLine("");
            Console.WriteLine("             Azure Manager Menu ");
            Console.WriteLine(dividerLine);
            Console.WriteLine("");
            Console.WriteLine("Select operation type:");
            Console.WriteLine("1.  Personal");
            Console.WriteLine("2.  EMVS");
            Console.WriteLine(dividerLine);
            string choice = Console.ReadLine();
            if (choice == "1" || choice == "2")
            {
                processFlag = choice;
                return choice;
            }
            else
            {
                choice = "1";
                return choice;
            }
        }

        public static string Main2()
        {
            Console.WriteLine(dividerLine);
            Console.WriteLine("Select option listed below:");
            Console.WriteLine("1.  Azure Environment Read");
            if (processFlag == "1")
            {
                Console.WriteLine("2.  Azure Environment Create");
            }
            Console.WriteLine(dividerLine);
            string choice = Console.ReadLine();
            return choice;
        }

        public static string SubSelect(IEnumerable<SubscriptionListOperationResponse.Subscription> subscriptions)
        {
            Console.WriteLine(dividerLine);
            Console.WriteLine("");
            Console.WriteLine("             Subscriptions Avaialble ");
            Console.WriteLine(dividerLine);
            foreach (var subby in subscriptions)
            {
                Console.WriteLine(subby.SubscriptionName + "  " + subby.SubscriptionStatus);
            }
            Console.WriteLine(dividerLine);
            Console.WriteLine();
            Console.Write("Enter Subscription name (Default is Premium): ");

            string subSelectName = Console.ReadLine();
            if (subSelectName.IsEmpty())
            {
                subSelectName = mySubscription;
            }
            return subSelectName;
        }

        public static void AffinityDisplay(AffinityGroupListResponse groups)
        {
            {
                Console.WriteLine(dividerLine);
                Console.WriteLine("");
                Console.WriteLine("             Affinity Groups ");
                Console.WriteLine(dividerLine);
                foreach (var item in groups)
                {
                    Console.WriteLine(item.Name);
                }
                Console.WriteLine(dividerLine);
                Console.WriteLine();
            }
        }

        public static void CloudServiceDisplay(HostedServiceListResponse serviceList)
        {
            Console.WriteLine(dividerLine);
            Console.WriteLine(" ");
            Console.WriteLine("             Cloud Services ");
            Console.WriteLine(dividerLine);
            foreach (var item in serviceList)
            {
                Console.WriteLine(item.ServiceName);
            }
            Console.WriteLine(dividerLine);
            Console.WriteLine();
        }

        public static void StorageAccountDisplay(StorageAccountListResponse storageList)
        {
            Console.WriteLine(dividerLine);
            Console.WriteLine(" ");
            Console.WriteLine("             Storage Accounts ");
            Console.WriteLine(dividerLine);
            foreach (var item in storageList)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine(dividerLine);
            Console.WriteLine();
        }

        public static void VHDDisplay()
        {
            Console.WriteLine(dividerLine);
            Console.WriteLine(" ");
            Console.WriteLine("             Virtual Machines");
            Console.WriteLine(dividerLine);
            //foreach (var item in SqlServerList)
            //{
            //    Console.WriteLine(item.Name);
            //}
            Console.WriteLine(dividerLine);
            Console.WriteLine();
        }

        public static void SqlServerDisplay(ServerListResponse SqlServerList)
        {
            Console.WriteLine(dividerLine);
            Console.WriteLine(" ");
            Console.WriteLine("             SQL Servers ");
            Console.WriteLine(dividerLine);
            foreach (var item in SqlServerList)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine(dividerLine);
            Console.WriteLine();
        }

        public static void SqlDatabasesDisplay(DatabaseListResponse SqlDatabaseList, string sqlServerName)
        {
            Console.WriteLine(dividerLine);
            Console.WriteLine(" ");
            Console.WriteLine("    SQL Server " + sqlServerName + " databases");
            Console.WriteLine(dividerLine);
            foreach (var item in SqlDatabaseList)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine(dividerLine);
            Console.WriteLine();
        }
    }
}