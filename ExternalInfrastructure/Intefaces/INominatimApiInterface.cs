namespace ExternalServices.Intefaces
{
    public interface INominatimApiInterface
    {
        Task<string> GetCountryAsync(double latitiude, double longitude);
        Task<string> GetCountryAsync(string gPSLocationString);
    }
}