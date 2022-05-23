
using Newtonsoft.Json;

namespace ExternalServices.Models
{
    public class NominatimApiModel
    {
        public class NominatimAddress
        {
            public string Road { get; set; }
            public string Village { get; set; }
            public string County { get; set; }
            public string State { get; set; }

            [JsonProperty("ISO3166-2-lvl4")]
            public string ISO31662Lvl4 { get; set; }
            public string Postcode { get; set; }
            public string Country { get; set; }
            public string CountryCode { get; set; }
        }
        public int PlaceId { get; set; }
        public string Licence { get; set; }
        public string OsmType { get; set; }
        public int OsmId { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public int PlaceRank { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public double Importance { get; set; }
        public string Addresstype { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public NominatimAddress Address { get; set; }
        public List<string> Boundingbox { get; set; }
    }
}