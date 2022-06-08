using Caliburn.Micro;
using System.Windows;

namespace WpfCaliburnApp.ViewModels
{
    public class MainViewModel : Screen
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public bool CanSayHello
        {
            get { return !string.IsNullOrEmpty(Name); }
        }

        public void SayHello()
        {
            MessageBox.Show($"Hello~ {Name}");
        }
    }
}
