using James.Utils.String;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Models;
using Microsoft.WindowsAzure.Management.Storage;
using Microsoft.WindowsAzure.Management.Storage.Models;
using NLog;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace AzureDashboard
{
    /// <summary>
    /// The storage component of the demo.
    /// </summary>
    public class StorageHelper
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static readonly string toggle = ConfigurationManager.AppSettings["Toggle"];

        /// <summary>
        /// Create a storage account.
        /// </summary>
        /// <param name="credentials">The credentials for the authenticated client.</param>
        /// <returns>The name of the storage account</returns>
        public async static Task<string>
            CreateStorageAccount(SubscriptionCloudCredentials credentials, string accountName)
        {
            if (accountName.IsEmpty())
            {
                accountName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            }
            using (var storageClient = new StorageManagementClient(credentials))
            {
                var result = await storageClient.StorageAccounts.CreateAsync(
                    new StorageAccountCreateParameters
                    {
                        GeoReplicationEnabled = false,
                        Label = "Sample Storage Account",
                        Location = LocationNames.NorthEurope,
                        Name = accountName
                    });
            }

            return accountName;
        }

        public static string CheckStorageService(SubscriptionCloudCredentials credentials, string storageAccountName)
        {
            CheckNameAvailabilityResponse checkStorageNameResponse;
            storageAccountName = storageAccountName.ToLower();

            using (var storageClient = new StorageManagementClient(credentials))
            {
                checkStorageNameResponse = storageClient.StorageAccounts.CheckNameAvailability(storageAccountName);
                if (checkStorageNameResponse.IsAvailable)
                {
                    return "Available";
                }
                else
                {
                    return "Unavailable";
                }
            }
        }

        public static StorageAccountListResponse ListStorageServices(SubscriptionCloudCredentials credentials)
        {
            using (var storageClient = new StorageManagementClient(credentials))
            {
                StorageAccountListResponse storageList = storageClient.StorageAccounts.List();
                if (toggle == "On")
                {
                    Menus.StorageAccountDisplay(storageList);
                }
                else
                {
                    foreach (var item in storageList)
                    {
                        log.Debug("Storage Account: " + item.Name);
                    }
                }
                return storageList;
            }
        }
    }
}