using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DetailedRamUsage.Models;

namespace DetailedRamUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            Process[] allProcesses = Process.GetProcesses();
            List<RamUsageInfo> ramUsageInfos = new List<RamUsageInfo>();

            Dictionary<string, long> keyValuePairs = new Dictionary<string, long>();
            foreach (var item in allProcesses.GroupBy(x => x.ProcessName).OrderByDescending(x => x.Count()))
            {
                ramUsageInfos.Add(new RamUsageInfo() { NrOfThread = item.Count(), ProcessName = item.Key });
            }
            ulong totalSizeOfRamInMB = GetTotalMemoryInMB() / 2;
            long totalSizeOfRamUsageInMB = 0;
            foreach (var item in ramUsageInfos)
            {
                var item2 = ramUsageInfos.Where(x => x.ProcessName.Equals(item.ProcessName)).FirstOrDefault();
                item2.UsageInMB = (Process.GetProcesses().Where(x => x.ProcessName.Equals(item.ProcessName)).Sum(x => x.PrivateMemorySize64) / (1024 * 1024));
                totalSizeOfRamUsageInMB += item2.UsageInMB;
            }
            foreach (var item in ramUsageInfos)
            {
                var item2 = ramUsageInfos.Where(x => x.ProcessName.Equals(item.ProcessName)).FirstOrDefault();
                item2.PercantageForTotal = (int)(((item.UsageInMB * 1.0) / (totalSizeOfRamInMB * 1.0)) * 100);
                item2.PercantageForUsage = (int)(((item.UsageInMB * 1.0) / (totalSizeOfRamUsageInMB * 1.0)) * 100);
            }
            var total1 = 0;
            var total2 = 0;
            foreach (var item in ramUsageInfos.OrderByDescending(x => x.UsageInMB))
            {
                total1 += item.PercantageForTotal;
                total2 += item.PercantageForUsage;
                Console.WriteLine($"{item.PercantageForTotal}% ({total1}) - {item.PercantageForUsage}% ({total2}) - {item.UsageInMB} MB - {item.ProcessName}({item.NrOfThread}) ");
            }
        }
        static ulong GetTotalMemoryInMB()
        {
            return new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / (1024 * 1024);
        }
    }
}
