using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    class MainViewModel : Conductor<object>                 //Screen보다 Conductor가 상위임
    {
        public MainViewModel()
        {
            DisplayName = "SmartHome Monitoring v2.0";      //윈도우 타이틀
        }

        public void LoadDataBaseView()
        {
            ActivateItemAsync(new DataBaseViewModel());
        }

        public void LoadRealTimeView()
        {
            ActivateItemAsync(new RealTimeViewModel());
        }

        public void LoadHistoryView()
        {
            ActivateItemAsync(new HistoryViewModel());
        }

        public void ExitProgram()
        {
            Environment.Exit(0);
        }
    }
}
