using AutoMapper;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class TestsBase
    {
        public Mapper mapper;

        [TestInitialize]
        public void TestInitialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PersistenceServices.PersistenceMappingProfile>();
            });
            mapper = new Mapper(config);

        }
    }
}