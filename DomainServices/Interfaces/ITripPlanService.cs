
using Shared.DomainModels;

namespace DomainServices.Interfaces
{
    public interface ITripPlanService
    {
        TripPlanDOM Add(TripPlanDOM data);
        int Count();
        IEnumerable<TripPlanDOM> FindAll(List<string> id);
        TripPlanDOM Get(string id);
        double GetDistanceDrivenForTripPlanInKM(string id);
        double GetKMByAgeAndCountryOverAPeriod(int age, string countryCode, DateTime lower, DateTime upper);
        double GetKMByAgeAndCountryOverAPeriodFromLogs(int age, string countryCode, DateTime lower, DateTime upper);
        void TripProgressUpdate(TripLogDOM data);
        TripPlanDOM Update(TripPlanDOM data);
    }
}