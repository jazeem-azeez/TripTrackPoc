
using Geo;

namespace Shared.DomainModels
{
    public class TripLogDOM
    {
        public string CountryCode { get; set; }
        public int DriverAge { get; set; }
        public string DriverId { get; set; }
        public string TripPlanId { get; set; }
        public string CurrentLocation { get; set; }
        public string EventMessage { get; set; }
        public DateTime EventTimeStamp { get; set; }
        public string Id { get; set; }
        public string PreviousLocation { get; set; }
        public double DistanceInMeters
        {
            get
            {
                if (string.IsNullOrEmpty(PreviousLocation) || string.IsNullOrEmpty(CurrentLocation))
                    return 0;
                var coordinateSeq = new CoordinateSequence(GetCoordinate(PreviousLocation), GetCoordinate(CurrentLocation));
                return GeoContext.Current.GeodeticCalculator.CalculateLength(coordinateSeq).Value;
            }
        }

        public string TruckId { get; set; }

        private Coordinate GetCoordinate(string location)
        {
            var doublValues = location.Split(",");
            var coordinates = new Coordinate(Convert.ToDouble(doublValues[0]), Convert.ToDouble(doublValues[1]));
            return coordinates;
        }
    }
}