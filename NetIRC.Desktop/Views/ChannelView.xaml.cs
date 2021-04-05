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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetIRC.Desktop.Views
{
    /// <summary>
    /// Interaction logic for ChannelView.xaml
    /// </summary>
    public partial class ChannelView : UserControl
    {
        public ChannelView()
        {
            InitializeComponent();
        }

        private void UsersListView_Loaded(object sender, RoutedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(((ListBox)e.OriginalSource).ItemsSource) as ListCollectionView;
            view.CustomSort = new ChannelUserComparer();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var channelUser = (e.OriginalSource as TextBlock).DataContext as ChannelUser;

            (Application.Current as App).Client.Queries.GetQuery(channelUser.User);
        }
    }
}
