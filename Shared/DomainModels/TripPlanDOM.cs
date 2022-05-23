namespace Shared.DomainModels
{
    public class TripPlanDOM
    {
        public double ActualDistanceInMeters { get; set; }
        public string CountryCode { get; set; }
        public double CurrentDistanceInMeters { get; set; }
        public string CurrentLocation { get; set; }
        public string CurrentState { get; set; }
        public int DriverAge { get; set; }
        public string DriverId { get; set; }
        public string EndLocation { get; set; }
        public DateTime EndTime { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string StartLocation { get; set; }
        public DateTime StartTime { get; set; }
        public string TruckId { get; set; }
    }
}