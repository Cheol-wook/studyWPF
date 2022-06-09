using Bogus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace DummyDataApp
{
    public class Program
    {
        public static string MqttBrokerUrl { get; set; }
        public static MqttClient Client { get; set; }
        private static Thread MqttThread { get; set; }
        private static Faker<SensorInfo> SensorData { get; set; }
        private static string CurrValue { get; set; }
        static void Main(string[] args)
        {
            InitializeConfig();
            ConnectMqttBroker();
            StartPublish();
        }
        private static void InitializeConfig()
        {
            MqttBrokerUrl = "210.119.12.72";

            var Rooms = new[] { "DINNING", "LIVING", "BATH", "BED" };
            
            SensorData = new Faker<SensorInfo>()
                            .RuleFor(r => r.DevId, f => f.PickRandom(Rooms))
                            .RuleFor(r => r.currTime, f => f.Date.Past(0))
                            .RuleFor(r => r.Temp, f => f.Random.Float(19.0f, 30.9f))
                            .RuleFor(r => r.Humid, f => f.Random.Float(40.0f, 63.9f));
        }
        private static void ConnectMqttBroker()
        {
            try
            {
                Client = new MqttClient(MqttBrokerUrl);
                Client.Connect("SMARTHOME01");
                Console.WriteLine("접속 성공!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"접속 불가 : {ex}");
                Environment.Exit(5);
            }
        }

        private static void StartPublish()
        {
            MqttThread = new Thread(() => LoopPublish());
            MqttThread.Start();
        }

        private static void LoopPublish()
        {
            while (true)
            {
                SensorInfo tempvalue = SensorData.Generate();
                CurrValue = JsonConvert.SerializeObject(tempvalue, Formatting.Indented);
                Client.Publish("home/device/fakedata", Encoding.Default.GetBytes(CurrValue));
                Console.WriteLine($"Published : {CurrValue}");
                Thread.Sleep(1000);
            }
        }
    }
}
