using ExternalServices.Intefaces;
using ExternalServices.Models;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using RestSharp;

namespace ExternalServices.Implementations
{


    public class NominatimApiInterface : INominatimApiInterface
    {
        private const string ResourceUrl = "https://nominatim.openstreetmap.org/reverse.php?format=jsonv2";
        private readonly ILogger<NominatimApiInterface> _logger;
        private RestClient _client;

        public NominatimApiInterface(ILogger<NominatimApiInterface > logger)
        {
            _client = new RestClient();
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public  Task<string> GetCountryAsync(string gPSLocationString)
        {
            var str = gPSLocationString.Split(",");
            return GetCountryAsync(Convert.ToDouble(str[0]), Convert.ToDouble(str[1]));
        }
        public async Task<string> GetCountryAsync(double latitiude, double longitude)
        {
            var request = new RestRequest(ResourceUrl);
            request.AddOrUpdateParameter("lat", latitiude);
            request.AddOrUpdateParameter("lon", longitude);
            _logger.LogInformation("making http request to external api");
            var response = await _client.ExecuteGetAsync(request);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("HTTP request to external api success full");

                var result = JsonConvert.DeserializeObject<NominatimApiModel>(response.Content);                

                return result.Address.ISO31662Lvl4.Split('-').First();
            }
            _logger.LogInformation("HTTP request to external api failed");

            return "unknown";

        }
    }
}