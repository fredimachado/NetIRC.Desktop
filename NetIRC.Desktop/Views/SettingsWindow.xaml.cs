using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {
        private SettingsWindow()
        {
            InitializeComponent();
        }

        public SettingsWindow(MainWindow parent)
            : this()
        {
            Owner = parent;

            ConnectButton.Click += async (s, e) =>
            {
                Save();
                Close();

                await parent.ConnectAsync();
            };

            OkButton.Click += (s, e) =>
            {
                Save();
                Close();
            };

            CancelButton.Click += (s, e) => Close();
        }

        private void Save()
        {
            Settings.Default.Save();
        }
    }
}
