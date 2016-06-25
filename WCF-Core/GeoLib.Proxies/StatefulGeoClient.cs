using GeoLib.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GeoLib.Proxies
{
    public class StatefulGeoClient : ClientBase<IStatefulGeoManager>, IStatefulGeoManager
    {
        public void PushZip(string zip)
        {
            base.Channel.PushZip(zip);
        }

        public ZipCodeData GetZipInfo()
        {
            return base.Channel.GetZipInfo();
        }

        public IEnumerable<ZipCodeData> GetZips(int range)
        {
            return base.Channel.GetZips(range);
        }
    }
}
