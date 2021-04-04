using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace NetIRC.Desktop.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public App App => (App)Application.Current;

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
