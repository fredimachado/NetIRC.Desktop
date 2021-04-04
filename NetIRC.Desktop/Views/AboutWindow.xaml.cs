using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetIRC.Desktop.Views
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public sealed partial class AboutWindow : MetroWindow
    {
        private AboutWindow()
        {
            InitializeComponent();
        }

        public AboutWindow(Window parent)
            : this()
        {
            Owner = parent;

            OkButton.Click += (s, e) => Close();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            if (e.Uri is null || string.IsNullOrWhiteSpace(e.Uri.OriginalString))
            {
                return;
            }

            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });

            e.Handled = true;
        }
    }
}
