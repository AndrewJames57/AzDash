using Microsoft.WindowsAzure;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDashboard
{
    public class DataSet
    {
    }

    public class ProcessInfo
    {
        public static string ProcessFlag;
    }

    public class SubscriptionInfo
    {
        public string Name;

        public string Id;

        public TokenCloudCredentials credential;
    }

    public class ServiceInfo
    {
        public static string ServiceName;
        public static string Label;
    }

    public class CommonServiceSettings
    {
        public int AffintyNumber;
        public IEnumerable<string> AffintyGroupNames;
        public int ServiceNumber;
        public IEnumerable<string> CloudServiceNames;// { get; set; }
    }

    public class SubSettings
    {
        public IEnumerable<AffinitySubSetting> AffinitySubSettings;
        public IEnumerable<CloudServiceSetting> CloudServiceSettings;
    }

    public class AffinitySubSetting
    {
        public int AffinityNumber;
        public string AffinityGroupName;
        public string Label;
        public string Description;
        public string Location;
        public string AffinityGroup;
    }

    public class CloudServiceSetting
    {
        public int ServiceNumber;
        public string ServiceName;
        public string Label;
        public string Description;
        public string Location;
        public string AffinityGroup;
    }

    public class StorageSetting
    {
        public int StorageNumber;
        public string AccountName;
        public string Label;
        public bool GeoReplicationEnabled;
        public string location;
        public string AffinityGroup;
    }

    public static class CommonMethods
    {
        private static readonly string paramConfig = ConfigurationManager.AppSettings["ParamConfig"];
        private static Logger log = LogManager.GetCurrentClassLogger();

        public static SubSettings PopulateSubSettings()
        {
            SubSettings subSettings = new SubSettings();
            Dictionary<string, string> mySettings;
            mySettings = Common.ParamConfig(paramConfig);
            if (mySettings == null)
            {
                log.Error("Unable to load Settings");
                return null;
            }
            int cloudServiceNumber = Convert.ToInt32(mySettings["CloudServiceNumber"]);
            int affinityGroupNumber = Convert.ToInt32(mySettings["AffinityGroupNumber"]);

            List<AffinitySubSetting> affintyList = new List<AffinitySubSetting>();
            //
            try
            {
                for (int i = 0; i < affinityGroupNumber; i++)
                {
                    AffinitySubSetting affinitySubSetting = new AffinitySubSetting();
                    affinitySubSetting.AffinityNumber = i + 1;
                    affinitySubSetting.AffinityGroupName = mySettings["AffinityGroupName" + (i + 1)];
                    affinitySubSetting.Location = mySettings["AffinityLocation" + (i + 1)];
                    affinitySubSetting.Description = "";
                    affinitySubSetting.Label = "";

                    affintyList.Add(affinitySubSetting);
                }
                subSettings.AffinitySubSettings = affintyList;
            }
            catch (Exception ex)
            {
                Common.LogError(ex, log.Name);
                log.Info("Process halted with errors. See log.");
                subSettings.AffinitySubSettings = null;
            }

            List<CloudServiceSetting> serviceList = new List<CloudServiceSetting>();
            for (int i = 0; i < cloudServiceNumber; i++)
            {
                CloudServiceSetting cloudServiceSetting = new CloudServiceSetting();
                cloudServiceSetting.ServiceNumber = i + 1;
                cloudServiceSetting.ServiceName = mySettings["CloudServiceName" + (i + 1)];
                cloudServiceSetting.Location = mySettings["CloudServiceLocation" + (i + 1)];
                cloudServiceSetting.Description = "";
                cloudServiceSetting.Label = "";

                serviceList.Add(cloudServiceSetting);
            }
            subSettings.CloudServiceSettings = serviceList;
            return subSettings;
        }
    }
}