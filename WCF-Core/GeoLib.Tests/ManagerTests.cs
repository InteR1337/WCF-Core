using System;
using GeoLib.Contracts;
using GeoLib.Data;
using GeoLib.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GeoLib.Tests
{
    [TestClass]
    public class ManagerTests
    {
        [TestMethod]
        public void test_zip_code_retrieval()
        {
            Mock<IZipCodeRepository> mockZipCodeRepository = new Mock<IZipCodeRepository>();

            ZipCode zipCode = new ZipCode()
            {
                City = "LINCOLN PARK",
                State = new State() { Abbreviation = "NJ" },
                Zip = "07035"
            };

            mockZipCodeRepository.Setup(obj => obj.GetByZip("07035")).Returns(zipCode);

            IGeoService geoManager = new GeoManager(mockZipCodeRepository.Object);

            ZipCodeData data = geoManager.GetZipInfo("07035");

            Assert.IsTrue(data.City.ToUpper() == "LINCOLN PARK");
            Assert.IsTrue(data.State == "NJ");
        }
    }
}
