using System;

namespace WpfSmartHomeMonitoringApp.Models
{
    public class SmartHomeModel
    {
        public string DevId { get; set; }
        public DateTime CurrTime { get; set; }
        public Double Temp { get; set; }
        public double Humid { get; set; }
    }
}
