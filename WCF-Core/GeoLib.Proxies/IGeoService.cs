﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GeoLib.Contracts;

namespace GeoLib.Proxies
{
    [ServiceContract(CallbackContract = typeof(IUpdateZipCallback))]
    public interface IGeoService
    {
        [OperationContract]
        ZipCodeData GetZipInfo(string zip);
        [OperationContract]
        IEnumerable<string> GetStates(bool primaryOnly);
        [OperationContract(Name = "GetZipsByState")]
        IEnumerable<ZipCodeData> GetZips(string state);
        [OperationContract(Name = "GetZipsForRange")]
        IEnumerable<ZipCodeData> GetZips(string zip, int range);
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        int UpdateZipCity(IEnumerable<ZipCityData> zipCityData);
        [OperationContract]
        Task<int> UpdateZipCityAsync(IEnumerable<ZipCityData> zipCityData);
        [OperationContract(IsOneWay = true)]
        void OneWayExample();
    }
}
