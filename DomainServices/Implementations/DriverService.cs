
using DomainServices.Interfaces;

using Microsoft.Extensions.Logging;

using PersistenceServices.Interfaces;
using PersistenceServices.PersistenceModels;

using Shared.DomainModels;

namespace DomainServices.Implementations
{
    public class DriverService : IBizService<DriverDOM>
    {
        private readonly ILogger<DriverService> logger;
        private readonly IDataStoreRepository<DriverDocument, DriverDOM> dataStore;

        public DriverService(ILogger<DriverService> logger, IDataStoreRepository<DriverDocument, DriverDOM> dataStore)
        {
            this.logger = logger;
            this.dataStore = dataStore;
        }

        public DriverDOM Add(DriverDOM data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return dataStore.Add(data);
        }

        public int Count() => dataStore.Count();

        public DriverDOM Get(string id) => dataStore.GetModel(id);
        
    }

}