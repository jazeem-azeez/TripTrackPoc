using System;
using System.Collections.Generic;
using System.Linq;

using DomainServices.Implementations;
using DomainServices.Interfaces;

using Geo;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using PersistenceServices.Implementataions;
using PersistenceServices.PersistenceModels;

using Shared.DomainModels;

namespace TestProject.Implementations
{
    [TestClass]
    public class TripPlanServiceTests : TestsBase
    {
        private DriverDOM driverDOM;
        private Mock<IBizService<DriverDOM>> mockDriverService;
        private Mock<ILogger<TripPlanService>> mockLogger = new Mock<ILogger<TripPlanService>>();

        [TestMethod]
        public void Add_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            TripPlanDOM data = GetTripPlanModel();

            // Act
            service.Add(data);

            // Assert
            Assert.IsTrue(service.Count() > 0);
        }

        [TestMethod]
        public void ComputeDistance()
        {
            // Act
            var length = GeoContext.Current.GeodeticCalculator.CalculateLength(
                new CoordinateSequence(
                new Coordinate(55.67291913499782d, 12.564951273933206d),
                new Coordinate(55.675266529345116d, 12.56744036383077d)
                   )
                );
            //Assert
            Assert.AreEqual(304.6780798874341d, length.Value);
        }

        [TestMethod]
        public void GetDistanceDrivenForTruckPlan_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            const string countryCode = "DK";
            var plans = SimulateSampleData(service, countryCode);
            foreach (var item in plans)
            {
                SimulateTripMovement(service, item);
            }
            // Act
            var result = service.GetDistanceDrivenForTripPlanInKM(plans.First().Id);

            // Assert
            Assert.AreEqual(0.30467807988743406d, result);
        }

        [TestMethod]
        public void GetKMByAgeAndCountryOverAPeriod_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            int age = 50;
            string countryCode = "DE";
            DateTime upper = DateTime.Now.AddDays(1);
            DateTime lower = DateTime.Now.Subtract(new TimeSpan(DateTime.Now.Day, 0, 0, 0));

            var plans = SimulateSampleData(service, countryCode);
            foreach (var item in plans)
            {
                SimulateTripMovement(service, item);
            }

            // Act
            var result = service.GetKMByAgeAndCountryOverAPeriod(
                age,
                countryCode,
                lower,
                upper);

            // Assert
            Assert.IsTrue(result == 3.0467807988743411d);

            plans = SimulateSampleData(service, "DK", 25);
            foreach (var item in plans)
            {
                SimulateTripMovement(service, item);
            }

            // Act
            result = service.GetKMByAgeAndCountryOverAPeriod(
               age,
               countryCode,
               lower,
               upper);

            // Assert
            Assert.IsTrue(result == 3.0467807988743411d);
        }

        [TestMethod]
        public void GetKMByAgeAndCountryOverAPeriodFromLogs_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            int age = 0;
            string countryCode = "DE";
            DateTime upper = DateTime.Now.AddDays(1);
            DateTime lower = DateTime.Now.Subtract(new TimeSpan(DateTime.Now.Day, 0, 0, 0));
            var plans = SimulateSampleData(service, countryCode);
            foreach (var item in plans)
            {
                SimulateTripMovement(service, item);
            }

            // Act
            var result = service.GetKMByAgeAndCountryOverAPeriodFromLogs(
                age,
                countryCode,
                lower,
                upper);

            // Assert
            Assert.IsTrue(result == 3.0467807988743411d);

            plans = SimulateSampleData(service, "DK", 25);
            foreach (var item in plans)
            {
                SimulateTripMovement(service, item);
            }

            // Act
            result = service.GetKMByAgeAndCountryOverAPeriodFromLogs(
               age,
               countryCode,
               lower,
               upper);

            // Assert
            Assert.IsTrue(result == 3.0467807988743411d);
        }

        [TestMethod]
        public void TripProgressUpdate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();

            TripPlanDOM tripPlanDOM = GetTripPlanModel();
            tripPlanDOM = service.Add(tripPlanDOM);

            // Act
            SimulateTripMovement(service, tripPlanDOM);

            // Assert
            Assert.IsTrue(service.Count() == 1);
            tripPlanDOM = service.Get(tripPlanDOM.Id);
            Assert.IsTrue(tripPlanDOM.CurrentDistanceInMeters == 304.6780798874341d);
        }

        [TestMethod]
        public void Update_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            TripPlanDOM data = GetTripPlanModel();

            data.StartTime = DateTime.Now;
            data.CurrentState = "started";
            // Act
            data = service.Add(data);
            data.EndTime = DateTime.Now;
            data.CurrentState = "completed";
            service.Update(data);

            // Assert
            Assert.IsTrue(service.Count() == 1);
        }

        private static TripPlanDOM GetTripPlanModel(string countryCode = "DK", int age = 55)
        {
            return new TripPlanDOM { CountryCode = countryCode, DriverAge = age, DriverId = "123", Name = "random trip", CurrentState = "planned" };
        }

        private TripPlanService CreateService()
        {
            this.driverDOM = new DriverDOM
            {
                Id = "1234",
                Name = "testuser",
                DOB = DateTime.Now.AddYears(-55)
            };
            this.mockDriverService = new Mock<IBizService<DriverDOM>>();
            mockDriverService.Setup(x => x.Get(It.IsAny<string>())).Returns(driverDOM);
            mockDriverService.Setup(x => x.Add(It.IsAny<DriverDOM>())).Returns(driverDOM);
            return new TripPlanService(
                this.mockLogger.Object, mockDriverService.Object,
                new DataStoreRepository<TripPlanDocument, TripPlanDOM>(mapper),
                new DataStoreRepository<TripLogDocument, TripLogDOM>(mapper));
        }

        private TripLogDOM GetTripLogModel(TripPlanDOM tripPlanDOM)
        {
            return new TripLogDOM
            {
                DriverAge = tripPlanDOM.DriverAge,
                DriverId = tripPlanDOM.DriverId,
                CountryCode = tripPlanDOM.CountryCode,
                EventTimeStamp = DateTime.Now,
                CurrentLocation = tripPlanDOM.CurrentLocation,
                TripPlanId = tripPlanDOM.Id
            };
        }

        private List<TripPlanDOM> SimulateSampleData(TripPlanService service, string countryCode, int age = 55)
        {
            var plans = new List<TripPlanDOM>();
            for (int i = 0; i < 10; i++)
            {
                plans.Add(service.Add(GetTripPlanModel(countryCode, age)));
            }
            return plans;
        }

        private void SimulateTripMovement(TripPlanService service, TripPlanDOM tripPlanDOM)
        {
            TripLogDOM data = GetTripLogModel(tripPlanDOM);
            data.CurrentLocation = "55.67291913499782, 12.564951273933206";
            data.EventMessage = "started";
            data.EventTimeStamp = DateTime.Now.AddDays(-1);
            service.TripProgressUpdate(data);

            data = GetTripLogModel(tripPlanDOM);
            data.CurrentLocation = "55.675266529345116, 12.56744036383077";
            data.EventMessage = "completed";
            service.TripProgressUpdate(data);
        }
    }
}