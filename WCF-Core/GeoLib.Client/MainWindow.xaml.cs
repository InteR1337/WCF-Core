using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GeoLib.Client.Contracts;
using GeoLib.Contracts;
using GeoLib.Proxies;

namespace GeoLib.Client
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = false)]
    public partial class MainWindow : Window, IUpdateZipCallback
    {
        private readonly StatefulGeoClient _Proxy;
        private readonly SynchronizationContext _SyncContext;

        public MainWindow()
        {
            InitializeComponent();
            //_Proxy = new GeoClient("tcpEP");
            _Proxy = new StatefulGeoClient();
            _SyncContext = SynchronizationContext.Current;
        }

        public void ZipUpdated(ZipCityData zipCityData)
        {
            //MessageBox.Show(string.Format("Updated zipcode {0} with city {1}.", zipCityData.ZipCode, zipCityData.City));

            SendOrPostCallback callback = arg => { lstUpdates.Items.Add(zipCityData); };

            _SyncContext.Send(callback, null);
        }

        private void btnGetInfo_Click(object sender, RoutedEventArgs e)
        {
            //GeoClient proxy = new GeoClient("tcpEP");

            var data = _Proxy.GetZipInfo();
            if (data != null)
            {
                lblCity.Content = data.City;
                lblState.Content = data.State;
            }

            //proxy.Close();
        }

        private void btnGetInRange_Click(object sender, RoutedEventArgs e)
        {
            if (txtState.Text != string.Empty)
            {
                //GeoClient proxy = new GeoClient("tcpEP");

                var data = _Proxy.GetZips(int.Parse(txtState.Text));
                if (data != null)
                {
                    lstZips.ItemsSource = data;
                }

                //_Proxy.Close();
            }
        }

        private void BtnMakeCall_Click(object sender, RoutedEventArgs e)
        {
            var address = new EndpointAddress("net.tcp://localhost:8010/MessageService");
            Binding binding = new NetTcpBinding();

            var factory = new ChannelFactory<IMessageService>(binding, address);
            var proxy = factory.CreateChannel();

            proxy.ShowMsg(txtMessage.Text);

            factory.Close();
        }

        private void btnPush_Click(object sender, RoutedEventArgs e)
        {
            if (txtZipCode.Text != null)
            {
                _Proxy.PushZip(txtZipCode.Text);
            }
        }

        private void btnOneWay_Click(object sender, RoutedEventArgs e)
        {
            var proxy = new GeoClient("httpEP");

            proxy.OneWayExample();

            MessageBox.Show("Oneway Example called. Back at client.");

            proxy.Close();

            MessageBox.Show("Proxy now closed.");
        }

        private async void btnUpdateBatch_Click(object sender, RoutedEventArgs e)
        {
            var cityZipList = new List<ZipCityData>
            {
                new ZipCityData {ZipCode = "07035", City = "Bedrock"},
                new ZipCityData {ZipCode = "33033", City = "End of the World"},
                new ZipCityData {ZipCode = "90210", City = "Alderan"},
                new ZipCityData {ZipCode = "07094", City = "Storybrooke"}
            };

            lstUpdates.Items.Clear();

            //await Task.Run(() =>
            //{
            //    try
            //    {
            //        var proxy = new GeoClient(new InstanceContext(this), "tcpEP");

            //        proxy.UpdateZipCity(cityZipList);

            //        proxy.Close();

            //        MessageBox.Show("Updated.");
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Error: " + ex.Message);
            //    }
            //});

            GeoClient proxy = new GeoClient(new InstanceContext(this), "tcpEP");
            Task<int> task = proxy.UpdateZipCityAsync(cityZipList);

            task.ContinueWith(x =>
            {
                MessageBox.Show(string.Format("Service returned {0} items.", x.Result));
            });

            MessageBox.Show("Call made.");
        }

        private async void btnPutBack_Click(object sender, RoutedEventArgs e)
        {
            var cityZipList = new List<ZipCityData>
            {
                new ZipCityData {ZipCode = "07035", City = "Lincoln Park"},
                new ZipCityData {ZipCode = "33033", City = "Homestead"},
                new ZipCityData {ZipCode = "90210", City = "Beverly Hills"},
                new ZipCityData {ZipCode = "07094", City = "Secaucus"}
            };

            lstUpdates.Items.Clear();

            await Task.Run(() =>
            {
                try
                {
                    var proxy = new GeoClient(new InstanceContext(this), "tcpEP");

                    proxy.UpdateZipCity(cityZipList);

                    proxy.Close();

                    MessageBox.Show("Updated.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            });
        }
    }
}