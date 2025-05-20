using IpBlockApi.Models;
using IpBlockApi.Services;

namespace IpBlockApi.Repository
{
    public class InMemoryBlockedAttemptLogger : IBlockedAttemptLogger
    {
        private readonly List<BlockedAttemptLog> _logs = new List<BlockedAttemptLog>();

        public List<BlockedAttemptLog> GetBlockedAttempts(int pageNumber, int pageSize)
        {
            return _logs
                .Where(log => log.IsBlocked) 
                .OrderByDescending(log => log.Timestamp) 
                .Skip((pageNumber - 1) * pageSize)   
                .Take(pageSize) 
                .ToList();
        }
        

        public void LogAttempt(BlockedAttemptLog log)
        {
            _logs.Add(log);
        }
    }
}
