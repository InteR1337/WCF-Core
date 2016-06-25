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
            ServiceHost hostGeoManager = new ServiceHost(typeof(GeoManager), 
                new Uri("http://localhost:8080"), 
                new Uri("net.tcp://localhost:8009"));

            ServiceMetadataBehavior metadataBehavior = hostGeoManager.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (metadataBehavior == null)
            {
                metadataBehavior = new ServiceMetadataBehavior();
                metadataBehavior.HttpGetEnabled = true;
                hostGeoManager.Description.Behaviors.Add(metadataBehavior);
            }
            else
            {
                metadataBehavior.HttpGetEnabled = true;
            }

            hostGeoManager.AddServiceEndpoint(typeof(IMetadataExchange), 
                MetadataExchangeBindings.CreateMexHttpBinding(), "MEX");

            hostGeoManager.Open();




            Console.WriteLine("Services started. Press [Enter] to exit.");
            Console.ReadLine();

            hostGeoManager.Close();
        }
    }
}
