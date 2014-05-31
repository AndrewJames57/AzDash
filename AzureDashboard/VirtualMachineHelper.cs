using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDashboard
{
    /// <summary>
    /// The virtual machine portion of the demo
    /// </summary>
    public class VirtualMachineHelper
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static readonly string toggle = ConfigurationManager.AppSettings["Toggle"];

        public async static Task<string> CreateVirtualMachine(
            SubscriptionCloudCredentials credentials,
            string cloudServiceName,
            string storageAccountName,
            string username,
            string password,
            string imageFilter)
        {
            using (var computeClient = new ComputeManagementClient(credentials))
            {
                // get the list of images from the api
                var operatingSystemImageListResult =
                    await computeClient.VirtualMachineOSImages.ListAsync();

                // find the one i want
                var imageName =
                    operatingSystemImageListResult
                        .Images
                            .FirstOrDefault(x =>
                                x.Label.Contains(imageFilter)).Name;

                var virtualMachineName = cloudServiceName + "vm";

                // set up the configuration set for the windows image
                var windowsConfigSet = new ConfigurationSet
                {
                    ConfigurationSetType = ConfigurationSetTypes.WindowsProvisioningConfiguration,
                    AdminPassword = password,
                    AdminUserName = username,
                    ComputerName = virtualMachineName,
                    HostName = string.Format("{0}.cloudapp.net", cloudServiceName)
                };

                // make sure i enable powershell & rdp access
                var endpoints = new ConfigurationSet
                {
                    ConfigurationSetType = "NetworkConfiguration",
                    InputEndpoints = new List<InputEndpoint>
                    {
                        new InputEndpoint
                        {
                            Name = "PowerShell", LocalPort = 5986, Protocol = "tcp", Port = 5986,
                        },
                        new InputEndpoint
                        {
                            Name = "Remote Desktop", LocalPort = 3389, Protocol = "tcp", Port = 3389,
                        }
                    }
                };

                // set up the hard disk with the os
                var vhd = new OSVirtualHardDisk
                {
                    SourceImageName = imageName,
                    HostCaching = VirtualHardDiskHostCaching.ReadWrite,
                    MediaLink = new Uri(string.Format(CultureInfo.InvariantCulture,
                        "https://{0}.blob.core.windows.net/vhds/{1}.vhd", storageAccountName, imageName),
                            UriKind.Absolute)
                };

                // create the role for the vm in the cloud service
                var role = new Role
                {
                    RoleName = virtualMachineName,
                    RoleSize = VirtualMachineRoleSize.Small,
                    RoleType = VirtualMachineRoleType.PersistentVMRole.ToString(),
                    OSVirtualHardDisk = vhd,
                    ConfigurationSets = new List<ConfigurationSet>
                    {
                        windowsConfigSet,
                        endpoints
                    },
                    ProvisionGuestAgent = true
                };

                // create the deployment parameters
                var createDeploymentParameters = new VirtualMachineCreateDeploymentParameters
                {
                    Name = cloudServiceName,
                    Label = cloudServiceName,
                    DeploymentSlot = DeploymentSlot.Production,
                    Roles = new List<Role> { role }
                };

                // deploy the virtual machine
                try
                {
                    var deploymentResult = await computeClient.VirtualMachines.CreateDeploymentAsync(
                                cloudServiceName,
                                createDeploymentParameters);
                }
                catch (Exception ex)
                {
                    Common.LogError(ex, log.Name, 1);
                    virtualMachineName = "";
                }

                // return the name of the virtual machine
                return virtualMachineName;
            }
        }

        public static void ListVHDS(SubscriptionCloudCredentials credentials)
        {
        }
    }
}