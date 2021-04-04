using NetIRC;
using NetIRC.Connection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NetIRC.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Client Client { get; private set; }

        public void SetClient(Client client)
        {
            Client = client;
        }
    }
}
