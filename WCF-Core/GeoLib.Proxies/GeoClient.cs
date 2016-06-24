using GeoLib.Contracts;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace GeoLib.Proxies
{
    public class GeoClient : ClientBase<IGeoService>, IGeoService
    {
        public GeoClient(string endpointName) : base(endpointName)
        { }

        public GeoClient(Binding binding, EndpointAddress address) : base(binding, address)
        { }

        public ZipCodeData GetZipInfo(string zip)
        {
            return base.Channel.GetZipInfo(zip);
        }

        public System.Collections.Generic.IEnumerable<string> GetStates(bool primaryOnly)
        {
            return base.Channel.GetStates(primaryOnly);
        }

        public System.Collections.Generic.IEnumerable<ZipCodeData> GetZips(string state)
        {
            return base.Channel.GetZips(state);
        }

        public System.Collections.Generic.IEnumerable<ZipCodeData> GetZips(string zip, int range)
        {
            return base.Channel.GetZips(zip, range);
        }
    }
}