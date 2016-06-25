using GeoLib.Contracts;
using GeoLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GeoLib.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class StatefulGeoManager : IStatefulGeoManager
    {
        ZipCode _zipCodeEntity;

        public void PushZip(string zip)
        {
            ZipCodeRepository zipCodeRepository = new ZipCodeRepository();
            _zipCodeEntity = zipCodeRepository.GetByZip(zip);
        }

        public ZipCodeData GetZipInfo()
        {
            ZipCodeData zipCodeData = null;

            if (_zipCodeEntity != null)
            {
                zipCodeData = LocalConfiguration.Mapper.Map<ZipCodeData>(_zipCodeEntity);
            }
            else
            {
                throw new ArgumentException("Uh no");
            }

            return zipCodeData;
        }

        public IEnumerable<ZipCodeData> GetZips(int range)
        {
            List<ZipCodeData> zipCodeData = new List<ZipCodeData>();
            if (_zipCodeEntity != null)
            {
                ZipCodeRepository zipCodeRepository = new ZipCodeRepository();

                var zips = zipCodeRepository.GetZipsForRange(_zipCodeEntity, range);
                if (zips != null)
                {
                    foreach (ZipCode zipCode in zips)
                    {
                        zipCodeData.Add(LocalConfiguration.Mapper.Map<ZipCodeData>(zipCode));
                    }
                }
            }
            else
            {
                throw new ArgumentException("Uh no");
            }

            return zipCodeData;
        }
    }
}
