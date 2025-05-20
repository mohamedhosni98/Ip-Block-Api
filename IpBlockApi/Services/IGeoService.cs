using IpBlockApi.Models;

namespace IpBlockApi.Services
{
    public interface IGeoService
    {
        Task<GeoLocationResponse> LookIPasync(string IpAdress);
    }
}
