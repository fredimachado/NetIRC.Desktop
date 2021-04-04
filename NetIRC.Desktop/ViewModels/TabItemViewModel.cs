using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NetIRC.Desktop.ViewModels
{
    public abstract class TabItemViewModel : BaseViewModel
    {
        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

        private string message;
        public string Message { get => message; set => SetProperty(ref message, value); }

        public ICommand SendMessageCommand { get; protected set; }
    }
}
