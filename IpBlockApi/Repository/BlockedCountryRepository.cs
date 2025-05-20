using IpBlockApi.Models;
using IpBlockApi.Services;
using System.Collections.Concurrent;

namespace IpBlockApi.Repository
{
    public class BlockedCountryRepository : IBlockedCountryRepository
    {
        private readonly ConcurrentDictionary<string,BlockedCountry> blockedcountries = new(); 

        public bool Add(BlockedCountry blockedCountry)
        {
            return blockedcountries.TryAdd(blockedCountry.Country.CountryCode.ToUpper(), blockedCountry);
        }

        public bool Delete(string counrtycode)
        {
            return blockedcountries.TryRemove(counrtycode.ToUpper(),out _);
        }

        public List<BlockedCountry> GetAll()
        {
            return blockedcountries.Values.ToList();
        }

        public BlockedCountry GetByCode(string countryCode)
        {
            blockedcountries.TryGetValue(countryCode.ToUpper(), out var country);
            return country;
        }
    }
}
