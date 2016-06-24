using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoLib.WindowsHost.Contracts;
using System.ServiceModel;

namespace GeoLib.WindowsHost
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class MessageManager : IMessageService
    {
        public void ShowMessage(string message)
        {
            MainWindow.MainUI.ShowMessage(message);
        }
    }
}
