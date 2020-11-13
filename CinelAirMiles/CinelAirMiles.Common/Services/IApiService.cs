namespace CinelAirMiles.Common.Services
{
    using CinelAirMiles.Common.Responses;

    using System.Threading.Tasks;

    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetItemAsync<T>(string urlBase, string servicePrefix, string controller);
    }
}
