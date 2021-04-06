using MahApps.Metro.Controls;
using NetIRC.Desktop.ViewModels;
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

            DataContext = new AboutWindowViewModel(Close);
        }

        public AboutWindow(Window parent)
            : this()
        {
            Owner = parent;
        }
    }
}
