using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using GeoLib.Contracts;
using GeoLib.Services;
using System.ServiceModel.Description;

namespace GeoLib.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost hostGeoManager = new ServiceHost(typeof(GeoManager));
            ServiceHost statefulHostGeoManager = new ServiceHost(typeof(StatefulGeoManager));

            hostGeoManager.Open();
            statefulHostGeoManager.Open();

            Console.WriteLine("Services started. Press [Enter] to exit.");
            Console.ReadLine();

            hostGeoManager.Close();
            statefulHostGeoManager.Close();
        }
    }
}
