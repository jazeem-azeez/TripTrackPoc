
using Ardalis.GuardClauses;

using DomainServices.Interfaces;

using ExternalServices.Intefaces;

using Microsoft.Extensions.Logging;

using PersistenceServices.Interfaces;
using PersistenceServices.PersistenceModels;

using Shared.DomainModels;

namespace DomainServices.Implementations
{
    public class TruckService : IBizService<TruckDOM>, ITruckService
    {
        private readonly ILogger<TruckService> logger;
        private readonly IDataStoreRepository<TruckDocument, TruckDOM> dataStore;
        private readonly IBizService<DriverDOM> bizServiceDriver;
        private readonly ITripPlanService tripPlanService;
        private readonly INominatimApiInterface nominatimApi;

        public TruckService(ILogger<TruckService> logger,
                            IDataStoreRepository<TruckDocument, TruckDOM> dataStore,
                            IBizService<DriverDOM> bizServiceDriver,
                            ITripPlanService tripPlanService,
                            INominatimApiInterface nominatimApi)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            this.bizServiceDriver = bizServiceDriver ?? throw new ArgumentNullException(nameof(bizServiceDriver));
            this.tripPlanService = tripPlanService ?? throw new ArgumentNullException(nameof(tripPlanService));
            this.nominatimApi = nominatimApi ?? throw new ArgumentNullException(nameof(nominatimApi));
        }

        public TruckDOM Add(TruckDOM data) => dataStore.Add(data);
        public TruckDOM Update(TruckDOM data) => dataStore.UpdateModel(data);

        public int Count() => dataStore.Count();

        public TruckDOM Get(string id) => dataStore.GetModel(id);
        public IEnumerable<TruckDOM> FindAll(List<string> id) => dataStore.FindAsModels(x => id.Contains(x.Id));
        public async Task<GpsInfoDOM> TruckMovementAsync(GpsInfoDOM gpsInfo)
        {
            var truckDom = Get(gpsInfo. TruckId);
            var driverDom = bizServiceDriver.Get(truckDom.CurrentDriverId);
            Guard.Against.Null(truckDom);
            var trip = tripPlanService.Get(gpsInfo.TripPlanId);
            Guard.Against.Null(trip);
            string countryCode = await nominatimApi.GetCountryAsync(gpsInfo.GPSLocationString);
            Guard.Against.NullOrWhiteSpace(countryCode);

            tripPlanService.TripProgressUpdate(new TripLogDOM
            {
                CountryCode = countryCode,
                CurrentLocation = gpsInfo.GPSLocationString,
                TruckId = gpsInfo.TruckId,
                TripPlanId = trip.Id,
                DriverId = truckDom.CurrentDriverId,
                DriverAge = driverDom.Age,
                EventMessage = gpsInfo.StatusMessage,
                EventTimeStamp = DateTime.Now
            });
            truckDom.LastkKnownLocation = gpsInfo.GPSLocationString;
            dataStore.UpdateModel(truckDom);
            return gpsInfo;
        }
    }

}