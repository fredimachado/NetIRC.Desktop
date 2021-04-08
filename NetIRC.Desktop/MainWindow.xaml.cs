using ControlzEx.Theming;
using MahApps.Metro.Controls;
using NetIRC.Desktop.Properties;
using NetIRC.Desktop.ViewModels;
using NetIRC.Desktop.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetIRC.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly MainViewModel mainViewModel;

        public MainWindow()
        {
            InitializeComponent();

            mainViewModel = new MainViewModel(ShowSettingsWindowDialog, ShowAboutWindowDialog);
            DataContext = mainViewModel;

            ContentRendered += MainWindow_ContentRendered;

            ThemeManager.Current.ChangeTheme(Application.Current, $"{Settings.Default.Theme}.Blue");
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            ShowSettingsWindowDialog();
        }

        private void ShowSettingsWindowDialog()
        {
            new SettingsWindow(this).ShowDialog();
        }

        private void ShowAboutWindowDialog()
        {
            new AboutWindow(this).ShowDialog();
        }
    }
}
