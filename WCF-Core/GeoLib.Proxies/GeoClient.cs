using System.Collections.Generic;
using GeoLib.Contracts;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace GeoLib.Proxies
{
    public class GeoClient : DuplexClientBase<IGeoService>, IGeoService
    {
        public GeoClient(InstanceContext instanceContext) : base(instanceContext) { }

        public GeoClient(string endpointName)
            : base(endpointName)
        { }

        public GeoClient(InstanceContext instanceContext, string endpointName)
            : base(instanceContext, endpointName)
        { }

        public GeoClient(InstanceContext instanceContext, Binding binding, EndpointAddress address)
            : base(instanceContext, binding, address)
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

        public int UpdateZipCity(IEnumerable<ZipCityData> zipCityData)
        {
            return base.Channel.UpdateZipCity(zipCityData);
        }

        public Task<int> UpdateZipCityAsync(IEnumerable<ZipCityData> zipCityData)
        {
            return base.Channel.UpdateZipCityAsync(zipCityData);
        }

        public void OneWayExample()
        {
            base.Channel.OneWayExample();
        }
    }
}