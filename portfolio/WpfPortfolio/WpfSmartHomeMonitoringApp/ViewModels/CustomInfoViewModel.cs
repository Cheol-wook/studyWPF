using Caliburn.Micro;
using System.Reflection;
using WpfSmartHomeMonitoringApp.Helpers;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    public class CustomInfoViewModel : Conductor<object>
    {
        private string applicationInfo;
        public CustomInfoViewModel(string title)
        {
            this.DisplayName = title;
            setApplicationInfo();
        }

        public string ApplicationInfo
        {
            get { return applicationInfo; }
            set
            {
                applicationInfo = value;
                NotifyOfPropertyChange(() => ApplicationInfo);
            }
        }

        private void setApplicationInfo()
        {

            ApplicationInfo = AssemblyTitle + "\nVer." + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ApplicationInfo += "\n" + AssemblyCopyright;
        }

        public void AcceptClose()
        {
            // 창 닫기
            TryCloseAsync(true);
        }
        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }
    }
}
