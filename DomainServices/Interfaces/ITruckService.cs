
using Shared.DomainModels;

namespace DomainServices.Interfaces
{
    public interface ITruckService
    {
        TruckDOM Add(TruckDOM data);
        int Count();
        IEnumerable<TruckDOM> FindAll(List<string> id);
        TruckDOM Get(string id);
        Task<GpsInfoDOM> TruckMovementAsync(GpsInfoDOM gpsInfoDOM);
        TruckDOM Update(TruckDOM data);
    }
}