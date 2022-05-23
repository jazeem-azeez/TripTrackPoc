
using ExternalServices.Implementations;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System;
using System.Threading.Tasks;

namespace L1TestProject.Implementations
{
    [TestClass]
    public class NominatimApiInterfaceTests
    {
        private MockRepository mockRepository;
        private Mock<ILogger<NominatimApiInterface>> logger;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            logger = new Mock<ILogger<NominatimApiInterface>>();
        }

        private NominatimApiInterface CreateNominatimApiInterface()
        {
            return new NominatimApiInterface(logger.Object);
        }

        [TestMethod]
        public async Task GetCountryAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var nominatimApiInterface = this.CreateNominatimApiInterface();
            float latitiude = 37.09024f;
            float longitude = -95.712891f;

            // Act
            var result = await nominatimApiInterface.GetCountryAsync(
                latitiude,
                longitude);

            // Assert
            Assert.AreEqual("US",result);
        }
    }
}
