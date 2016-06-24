using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
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
using GeoLib.Services;
using GeoLib.WindowsHost.Contracts;

namespace GeoLib.WindowsHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow MainUI { get; set; }
        private ServiceHost _HostGeoManager { get; set; }
        private ServiceHost _HostMessageManager { get; set; }

        private SynchronizationContext _SyncContext;

        public MainWindow()
        {
            InitializeComponent();

            btnStartServices.IsEnabled = true;
            btnStopServices.IsEnabled = false;

            MainUI = this;
            _SyncContext = SynchronizationContext.Current;

            this.Title = string.Format("UI Running on Thread {0} | Process {1}", Thread.CurrentThread.ManagedThreadId, Process.GetCurrentProcess().Id);
        }

        private void BtnStartServices_Click(object sender, RoutedEventArgs e)
        {
            _HostGeoManager = new ServiceHost(typeof(GeoManager));
            _HostMessageManager = new ServiceHost(typeof(MessageManager));

            _HostGeoManager.Open();
            _HostMessageManager.Open();

            btnStartServices.IsEnabled = false;
            btnStopServices.IsEnabled = true;
        }

        private void BtnStopServices_Click(object sender, RoutedEventArgs e)
        {
            _HostGeoManager.Close();
            _HostMessageManager.Close();

            btnStartServices.IsEnabled = true;
            btnStopServices.IsEnabled = false;
        }

        public void ShowMessage(string message)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            lblMessage2.Content = string.Format("{0}{1} Thread Id: {2} Process Id: {3}", message, Environment.NewLine, threadId, Process.GetCurrentProcess().Id);
        }

        private void btnInProc_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(x =>
            {
                ChannelFactory<IMessageService> channelFactory = new ChannelFactory<IMessageService>("");
                IMessageService proxy = channelFactory.CreateChannel();

                proxy.ShowMessage(DateTime.Now.ToLongTimeString() + " from in-process call.");

                channelFactory.Close();
            });

            thread.IsBackground = true;
            thread.Start();
        }
    }
}
