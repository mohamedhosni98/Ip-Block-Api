using IpBlockApi.Models;

namespace IpBlockApi.Services
{
    public interface ITemporaryBlockService
    {
        void AddTemporalBlock(string countryCode, int durationMinutes);
        bool IsCountryTemporarilyBlocked(string countryCode);
        List<TemporaryBlock> GetAllTemporalBlocks();
        void RemoveExpiredBlocks();
    }
}
