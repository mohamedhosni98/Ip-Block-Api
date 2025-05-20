using IpBlockApi.Models;
using IpBlockApi.Services;

namespace IpBlockApi.Repository
{

    public class InMemoryTemporaryBlockService : ITemporaryBlockService
    {
        private readonly List<TemporaryBlock> _temporalBlocks = new List<TemporaryBlock>();

        public void AddTemporalBlock(string countryCode, int durationMinutes)
        {
            var expirationTime = DateTime.UtcNow.AddMinutes(durationMinutes);
            _temporalBlocks.Add(new TemporaryBlock
            {
                CountryCode = countryCode,
                ExpirationTime = expirationTime
            });
        }

        public bool IsCountryTemporarilyBlocked(string countryCode)
        {
            return _temporalBlocks.Any(block => block.CountryCode == countryCode && block.ExpirationTime > DateTime.UtcNow);
        }

        public List<TemporaryBlock> GetAllTemporalBlocks()
        {
            return _temporalBlocks;
        }

        public void RemoveExpiredBlocks()
        {
            _temporalBlocks.RemoveAll(block => block.ExpirationTime <= DateTime.UtcNow);
        }
    }
}
   