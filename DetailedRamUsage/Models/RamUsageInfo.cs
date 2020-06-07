namespace DetailedRamUsage.Models
{
    public class RamUsageInfo
    {
        public int PercantageForTotal { get; set; }
        public int PercantageForUsage { get; set; }
        public long UsageInMB { get; set; }
        public string ProcessName { get; set; }
        public int NrOfThread { get; set; }
    }
}
