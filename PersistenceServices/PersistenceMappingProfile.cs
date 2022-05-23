using AutoMapper;

using PersistenceServices.PersistenceModels;

using Shared.DomainModels;

namespace PersistenceServices
{
    public class PersistenceMappingProfile : Profile
    {
        public PersistenceMappingProfile()
        {
            CreateMap<DriverDOM, DriverDocument>();
            CreateMap<TruckDOM, TruckDocument>();
            CreateMap<TripPlanDOM, TripPlanDocument>();
            CreateMap<TripLogDOM, TripLogDocument>();

            CreateMap<DriverDocument, DriverDOM>();
            CreateMap<TruckDocument, TruckDOM>();
            CreateMap<TripPlanDocument, TripPlanDOM>();
            CreateMap<TripLogDocument, TripLogDOM>();


            CreateMap<TripPlanDOM, TripLogDOM>();
            CreateMap<TripLogDOM, TripPlanDOM>();
        }
    }
}