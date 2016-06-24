using GeoLib.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using GeoLib.Data;

namespace GeoLib.Services
{
    public class GeoManager : IGeoService
    {
        public ZipCodeData GetZipInfo(string zip)
        {
            ZipCodeData zipCodeData = null;

            IZipCodeRepository zipCodeRepo = new ZipCodeRepository();

            ZipCode zipCodeEntity = zipCodeRepo.GetByZip(zip);
            if (zipCodeEntity != null)
            {
                zipCodeData = LocalConfiguration.Mapper.Map<ZipCodeData>(zipCodeEntity);
            }

            return zipCodeData;
        }

        public IEnumerable<string> GetStates(bool primaryOnly)
        {
            List<string> stateData = new List<string>();

            IStateRepository stateRepository = new StateRepository();

            IEnumerable<State> states = stateRepository.Get(primaryOnly);
            if (states != null)
            {
                foreach (State state in states)
                {
                    stateData.Add(state.Abbreviation);
                }
            }

            return stateData;
        }

        public IEnumerable<ZipCodeData> GetZips(string state)
        {
            List<ZipCodeData> zipCodeData = new List<ZipCodeData>();

            IZipCodeRepository zipCodeRepository = new ZipCodeRepository();

            var zips = zipCodeRepository.GetByState(state);
            if (zips != null)
            {
                foreach (ZipCode zipCode in zips)
                {
                    zipCodeData.Add(LocalConfiguration.Mapper.Map<ZipCodeData>(zipCode));
                }
            }

            return zipCodeData;
        }

        public IEnumerable<ZipCodeData> GetZips(string zip, int range)
        {
            List<ZipCodeData> zipCodeData = new List<ZipCodeData>();

            IZipCodeRepository zipCodeRepository = new ZipCodeRepository();

            ZipCode zipEntity = zipCodeRepository.GetByZip(zip);
            var zips = zipCodeRepository.GetZipsForRange(zipEntity, range);
            if (zips != null)
            {
                foreach (ZipCode zipCode in zips)
                {
                    zipCodeData.Add(LocalConfiguration.Mapper.Map<ZipCodeData>(zipCode));
                }
            }

            return zipCodeData;
        }
    }
}
