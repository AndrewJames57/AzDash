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
    internal class Program
    {
        private static readonly Logger log = LogManager.GetLogger("AzureDashboard:Program");

        private static void Main(string[] args)
        {
            log.Info("Start Application");
            if (Menus.Main1() == "1")
            {
                ProcessInfo.ProcessFlag = "Home";
                log.Info("Selected Presonal");
            }
            else
            {
                ProcessInfo.ProcessFlag = "Work";
                log.Info("Selected Work");
            }
            string exit = "";
            do
            {
                if (Menus.Main2() != "2")
                {
                    log.Info("Selected Azure Reader");
                    Task task = new Task(Processes.ReadProcess);
                    task.Start();
                    task.Wait();
                }
                Console.WriteLine("Enter X to exit");
                exit = Console.ReadLine();
            } while (exit != "x");

            log.Info("Stop Application - Press Enter");
            Console.ReadLine();
        }
    }
}