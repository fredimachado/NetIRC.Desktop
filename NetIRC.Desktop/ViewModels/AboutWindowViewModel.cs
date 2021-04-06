using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetIRC.Desktop.ViewModels
{
    public class AboutWindowViewModel
    {
        public ICommand CloseCommand { get; }

        public ICommand OpenLinkCommand { get; }

        public AboutWindowViewModel(Action closeAction)
        {
            CloseCommand = new Command(closeAction);
            OpenLinkCommand = new Command(OpenLink);
        }

        private void OpenLink(object link)
        {
            if (!(link is Uri uri))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(uri.OriginalString))
            {
                return;
            }

            Process.Start(new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
        }
    }
}
