using DomainServices.Interfaces;

using Microsoft.Extensions.Logging;

using PersistenceServices.Interfaces;
using PersistenceServices.PersistenceModels;

using Shared.DomainModels;

namespace DomainServices.Implementations
{
    public class TripPlanService : IBizService<TripPlanDOM>, ITripPlanService
    {
        private readonly IBizService<DriverDOM> bizServiceDriver;
        private readonly ILogger<TripPlanService> logger;
        private readonly IDataStoreRepository<TripLogDocument, TripLogDOM> tripLogDataStore;
        private readonly IDataStoreRepository<TripPlanDocument, TripPlanDOM> tripPlanDataStore;

        public TripPlanService(ILogger<TripPlanService> logger,
                                IBizService<DriverDOM> bizServiceDriver,
                               IDataStoreRepository<TripPlanDocument, TripPlanDOM> tripPlanDataStore,
                               IDataStoreRepository<TripLogDocument, TripLogDOM> tripLogDataStore)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.bizServiceDriver = bizServiceDriver;
            this.tripPlanDataStore = tripPlanDataStore ?? throw new ArgumentNullException(nameof(tripPlanDataStore));
            this.tripLogDataStore = tripLogDataStore;
        }

        public TripPlanDOM Add(TripPlanDOM data)
        {
            UpdateDriverAge(data);
            return tripPlanDataStore.Add(data);
        }

        public bool Between(DateTime input, DateTime lower, DateTime upper)
        {
            return (input >= lower && input <= upper);
        }

        public int Count() => tripPlanDataStore.Count();

        public IEnumerable<TripPlanDOM> FindAll(List<string> id) => tripPlanDataStore.FindAsModels(x => id.Contains(x.Id));

        public TripPlanDOM Get(string id) => tripPlanDataStore.GetModel(id);

        public double GetDistanceDrivenForTripPlanInKM(string id)
        {
            var result = Get(id);
            if (result.CurrentState == "completed")
            {
                return result.CurrentDistanceInMeters / 1000;
            }
            else
            {
                return tripLogDataStore.FindAsModels(x => x.TripPlanId == id).Sum(x => x.DistanceInMeters) / 1000;
            }
        }

        public double GetKMByAgeAndCountryOverAPeriod(int age, string countryCode, DateTime lower, DateTime upper)
        {
            return tripPlanDataStore.FindAsModels(x => x.DriverAge > age && x.CountryCode == countryCode && Between(x.EndTime, lower, upper)).Sum(x => x.CurrentDistanceInMeters) / 1000;
        }

        public double GetKMByAgeAndCountryOverAPeriodFromLogs(int age, string countryCode, DateTime lower, DateTime upper)
        {
            return tripLogDataStore.FindAsModels(x => x.DriverAge > age && x.CountryCode == countryCode).Sum(x => x.DistanceInMeters) / 1000;
        }

        public void TripProgressUpdate(TripLogDOM data)
        {
            var plan = tripPlanDataStore.GetModel(data.TripPlanId);
            plan.CountryCode = data.CountryCode;
            data.PreviousLocation = plan.CurrentLocation;
            plan.CurrentLocation = data.CurrentLocation;
            plan.CurrentState = data.EventMessage;
            plan.CurrentDistanceInMeters += data.DistanceInMeters;

            if (data.EventMessage == "completed")
            {
                plan.EndLocation = data.CurrentLocation;
                plan.EndTime = data.EventTimeStamp;
            }
            else if (data.EventMessage == "started")
            {
                plan.StartLocation = data.CurrentLocation;
                plan.StartTime = data.EventTimeStamp;
            }
            tripLogDataStore.Add(data);
            Update(plan);
        }

        public TripPlanDOM Update(TripPlanDOM data)
        {
            UpdateDriverAge(data);
            return tripPlanDataStore.UpdateModel(data);
        }

        private void UpdateDriverAge(TripPlanDOM data)
        {
            var driver = bizServiceDriver.Get(data.DriverId);
            data.DriverAge = driver.Age;
        }
    }
}