using GeoLib.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GeoLib.Data;
using System.Threading;
using System.Windows.Forms;

namespace GeoLib.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class GeoManager : IGeoService
    {
        private IZipCodeRepository _ZipCodeRepository;
        private IStateRepository _StateRepository;
        private int _Counter = 0;

        public GeoManager()
        {

        }

        public GeoManager(IZipCodeRepository zipCodeRepository)
        {
            _ZipCodeRepository = zipCodeRepository;
        }

        public GeoManager(IStateRepository stateRepository)
        {
            _StateRepository = stateRepository;
        }

        public GeoManager(IZipCodeRepository zipCodeRepository, IStateRepository stateRepository)
        {
            _ZipCodeRepository = zipCodeRepository;
            _StateRepository = stateRepository;
        }

        public ZipCodeData GetZipInfo(string zip)
        {
            //Thread.Sleep(10000);
            _Counter++;

            ZipCodeData zipCodeData = null;

            IZipCodeRepository zipCodeRepository = _ZipCodeRepository ?? new ZipCodeRepository();

            ZipCode zipCodeEntity = zipCodeRepository.GetByZip(zip);
            if (zipCodeEntity != null)
            {
                zipCodeData = LocalConfiguration.Mapper.Map<ZipCodeData>(zipCodeEntity);
            }

            Console.WriteLine("Counter = {0}", _Counter);

            return zipCodeData;
        }

        public IEnumerable<string> GetStates(bool primaryOnly)
        {
            List<string> stateData = new List<string>();

            IStateRepository stateRepository = _StateRepository ?? new StateRepository();

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

            IZipCodeRepository zipCodeRepository = _ZipCodeRepository ?? new ZipCodeRepository();

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

            IZipCodeRepository zipCodeRepository = _ZipCodeRepository ?? new ZipCodeRepository();

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

        [OperationBehavior(TransactionScopeRequired = true)]
        public int UpdateZipCity(IEnumerable<ZipCityData> zipCityData)
        {
            IZipCodeRepository zipCodeRepository = _ZipCodeRepository ?? new ZipCodeRepository();

            int counter = 0;
            foreach (ZipCityData zipCityItem in zipCityData)
            {
                ZipCode zipCodeEntity = zipCodeRepository.GetByZip(zipCityItem.ZipCode);
                zipCodeEntity.City = zipCityItem.City;
                ZipCode updatedItem = zipCodeRepository.Update(zipCodeEntity);

                IUpdateZipCallback callback = OperationContext.Current.GetCallbackChannel<IUpdateZipCallback>();

                if (callback != null)
                {
                    callback.ZipUpdated(zipCityItem);
                    Thread.Sleep(500);
                }
                counter++;
            }

            return counter;
        }

        public void OneWayExample()
        {
            MessageBox.Show("Made it to the service.");
        }
    }
}
