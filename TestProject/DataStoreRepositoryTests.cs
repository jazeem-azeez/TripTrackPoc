using System;
using System.Linq;

using AutoMapper;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using PersistenceServices.Implementataions;
using PersistenceServices.PersistenceModels;

using Shared.DomainModels;

namespace TestProject
{
    [TestClass]
    public class DataStoreRepositoryTests:TestsBase
    { 

        [TestMethod]
        public void Add_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dataStoreRepository = this.CreateDataStoreRepository();
            var data = new DriverDOM() { DOB=DateTime.Now.AddYears(-50),Name="P.K" };
            int count = dataStoreRepository.Count();

            // Act
            dataStoreRepository.Add(data);

            // Assert
            Assert.IsTrue(count < dataStoreRepository.Count());
            
        }

        [TestMethod]
        public void FindAsModels_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dataStoreRepository = this.CreateDataStoreRepository();


            dataStoreRepository.Add(new DriverDOM { DOB = DateTime.Now.AddYears(-51) });

            // Act
            var result = dataStoreRepository.FindAsModels(x => x.Age > 50);

            // Assert
            Assert.IsTrue(result.Count() == 1);

        }

        [TestMethod]
        public void UpdateModel_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dataStoreRepository = this.CreateDataStoreRepository();


            var data = new DriverDOM { DOB = DateTime.Now.AddYears(-51) };
           data = dataStoreRepository.Add(data);

            // Act
            var firstResult = dataStoreRepository.FindAsModels(x => x.Age > 50).Count();



            data.DOB = DateTime.Now;
            dataStoreRepository.UpdateModel(data);

            var result = dataStoreRepository.FindAsModels(x => x.Age > 50).Count(); 

            // Assert
            Assert.IsTrue(firstResult == 1);
            Assert.IsTrue(result == 0);

        }

        [TestMethod]
        public void Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dataStoreRepository = this.CreateDataStoreRepository();
            string id = null;

            var data = new DriverDOM { DOB = DateTime.Now.AddYears(-51) };
            data = dataStoreRepository.Add(data);
            // Act
            var result = dataStoreRepository.GetModel(data.Id);

            // Assert
            Assert.IsNotNull(result);
            
        }


        private DataStoreRepository<DriverDocument,DriverDOM> CreateDataStoreRepository()
        {
            return new DataStoreRepository<DriverDocument, DriverDOM>(mapper);
        }
    }
}