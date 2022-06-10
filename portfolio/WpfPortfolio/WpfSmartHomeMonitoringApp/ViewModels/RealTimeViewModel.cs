using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSmartHomeMonitoringApp.Helpers;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    class RealTimeViewModel : Conductor<object>
    {
        private string livingTempVal;
        private string dinningTempVal;
        private string bedTempVal;
        private string bathTempVal;

        public string LivingTempVal
        {
            get { return livingTempVal; }
            set
            {
                livingTempVal = value;
                NotifyOfPropertyChange(() => LivingTempVal);
            }
        }
        public string DinningTempVal
        {
            get { return dinningTempVal; }
            set
            {
                dinningTempVal = value;
                NotifyOfPropertyChange(() => DinningTempVal);
            }
        }
        public string BedTempVal
        {
            get { return bedTempVal; }
            set
            {
                bedTempVal = value;
                NotifyOfPropertyChange(() => BedTempVal);
            }
        }
        public string BathTempVal
        {
            get { return bathTempVal; }
            set
            {
                bathTempVal = value;
                NotifyOfPropertyChange(() => BathTempVal);
            }
        }

        public RealTimeViewModel()
        {
            LivingTempVal = dinningTempVal = BedTempVal = BathTempVal = null;

            if (Commons.MQTT_CLIENT != null && Commons.MQTT_CLIENT.IsConnected)
            {
                Commons.MQTT_CLIENT.MqttMsgPublishReceived += MQTT_CLIENT_MqttMsgPublishReceived;
            }
            else
            {

            }
        }

        private void MQTT_CLIENT_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            var currDatas = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);

            switch (currDatas["DevId"].ToString())
            {
                case "LIVING":
                    LivingTempVal = double.Parse(currDatas["Temp"]).ToString("0.#");
                    break;
                case "DINNING":
                    DinningTempVal = double.Parse(currDatas["Temp"]).ToString("0.#");
                    break;
                case "BED":
                    BedTempVal = double.Parse(currDatas["Temp"]).ToString("0.#");
                    break;
                case "BATH":
                    BathTempVal = double.Parse(currDatas["Temp"]).ToString("0.#");
                    break;
                default:
                    break;
            }
        }
    }
}
