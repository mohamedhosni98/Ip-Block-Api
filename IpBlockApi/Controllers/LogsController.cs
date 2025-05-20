using IpBlockApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IpBlockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IBlockedAttemptLogger _blockedAttemptLogger;

        public LogsController(IBlockedAttemptLogger blockedAttemptLogger)
        {
            _blockedAttemptLogger = blockedAttemptLogger;
        }

        [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedAttempts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                var blockedAttempts = _blockedAttemptLogger.GetBlockedAttempts(pageNumber, pageSize);
                return Ok(blockedAttempts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}

