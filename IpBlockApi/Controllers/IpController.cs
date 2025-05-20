using IpBlockApi.Models;
using IpBlockApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IpBlockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IGeoService geoService;
        private readonly IBlockedAttemptLogger blockedAttemptLogger;
        private readonly ITemporaryBlockService temporaryBlockService;
        

        // list of blocked country 
        private readonly List<string> blockedCountries = new List<string> { "EG", "SY", "IQ" }; 
        public IpController(IGeoService geoService,IBlockedAttemptLogger blockedAttemptLogger, ITemporaryBlockService temporaryBlockService  )
        {
            this.geoService = geoService;
            this.blockedAttemptLogger = blockedAttemptLogger;
            this.temporaryBlockService = temporaryBlockService;
        }

        [HttpGet("LookUp")]
        public async Task<IActionResult> LookUp([FromQuery] string? IpAddress)
        {
            if (string.IsNullOrWhiteSpace(IpAddress))
            {
                IpAddress = "154.183.133.4";             }

            var geoonfo = await geoService.LookIPasync(IpAddress);
            return Ok(geoonfo);
          
        
        }


        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            try
            {
                // get ip for local device 
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString(); 
                if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
                {
                    
                    ipAddress = "154.183.133.4";
                }

                var geoInfo = await geoService.LookIPasync(ipAddress);
                bool isBlocked = blockedCountries.Contains(geoInfo.CountryCode) 
                    ||
                    temporaryBlockService.IsCountryTemporarilyBlocked(geoInfo.CountryCode);

                var logEntry = new BlockedAttemptLog
                {
                    IpAddress = ipAddress,
                    Timestamp = DateTime.UtcNow,
                    CountryCode = geoInfo.CountryCode,
                    IsBlocked = isBlocked,
                    UserAgent = Request.Headers["User-Agent"].ToString()
                };
                blockedAttemptLogger.LogAttempt(logEntry);

                return Ok(new
                {
                    IpAddress = ipAddress,
                    CountryCode = geoInfo.CountryCode,
                    IsBlocked = isBlocked
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}

