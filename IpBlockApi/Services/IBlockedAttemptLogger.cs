using IpBlockApi.Models;

namespace IpBlockApi.Services
{
    public interface IBlockedAttemptLogger
    {
        void LogAttempt(BlockedAttemptLog log);
        List<BlockedAttemptLog> GetBlockedAttempts(int pageNumber, int pageSize);
    }
   
    }
