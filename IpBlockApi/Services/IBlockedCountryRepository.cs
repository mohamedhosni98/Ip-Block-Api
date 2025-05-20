using IpBlockApi.Models;

namespace IpBlockApi.Services
{
    public interface IBlockedCountryRepository
    {
        bool Add(BlockedCountry blockedCountries);
        bool Delete(string countrycode);

        List<BlockedCountry> GetAll();

        BlockedCountry GetByCode(string countryCode);
    }
}
