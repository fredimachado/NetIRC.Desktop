using ControlzEx.Theming;
using MahApps.Metro.Controls;
using NetIRC.Desktop.Messages;
using NetIRC.Desktop.Properties;
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
        private readonly string[] themes = new[] { "Light", "Dark" };
        public string[] Themes => themes;

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

                await App.EventAggregator.PublishOnUIThreadAsync(new ConnectMessage());
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
            Settings.Default.Nick = Nickname.Text;
            Settings.Default.Alternative = Alternative.Text;
            Settings.Default.RealName = RealName.Text;
            Settings.Default.DefaultChannel = DefaultChannel.Text;

            Settings.Default.Theme = Theme.SelectedValue.ToString();

            Settings.Default.ServerName = ServerName.Text;
            Settings.Default.ServerAddress = ServerAddress.Text;
            Settings.Default.ServerPort = ServerPort.Text;
            Settings.Default.ServerPassword = ServerPassword.Text;

            Settings.Default.Save();

            ThemeManager.Current.ChangeTheme(Application.Current, $"{Theme.SelectedValue}.Blue");
        }
    }
}
