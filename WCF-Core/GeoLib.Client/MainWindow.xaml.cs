using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
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
using GeoLib.Client.Contracts;
using GeoLib.Contracts;
using GeoLib.Proxies;

namespace GeoLib.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _Proxy = new GeoClient("tcpEP");
        }

        GeoClient _Proxy = null;

        private void btnGetInfo_Click(object sender, RoutedEventArgs e)
        {
            if (txtZipCode.Text != string.Empty)
            {
                //GeoClient proxy = new GeoClient("tcpEP");

                ZipCodeData data = _Proxy.GetZipInfo(txtZipCode.Text);
                if (data != null)
                {
                    lblCity.Content = data.City;
                    lblState.Content = data.State;
                }

                //proxy.Close();
            }
        }

        private void BtnGetZipCodes_Click(object sender, RoutedEventArgs e)
        {
            if (txtState.Text != string.Empty)
            {
                GeoClient proxy = new GeoClient("tcpEP");

                IEnumerable<ZipCodeData> data = proxy.GetZips(txtState.Text);
                if (data != null)
                {
                    lstZips.ItemsSource = data;
                }

                proxy.Close();
            }
        }

        private void BtnMakeCall_Click(object sender, RoutedEventArgs e)
        {
            EndpointAddress address = new EndpointAddress("net.tcp://localhost:8010/MessageService");
            System.ServiceModel.Channels.Binding binding = new NetTcpBinding();

            ChannelFactory<IMessageService> factory = new ChannelFactory<IMessageService>(binding, address);
            IMessageService proxy = factory.CreateChannel();

            proxy.ShowMsg(txtMessage.Text);

            factory.Close();
        }
    }
}
