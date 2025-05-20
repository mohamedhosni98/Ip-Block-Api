using IpBlockApi.Models;
using IpBlockApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace IpBlockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IBlockedCountryRepository blockedCountryRepository;
        private readonly ITemporaryBlockService temporaryBlockService;

        public CountriesController(IBlockedCountryRepository blockedCountryRepository , ITemporaryBlockService temporaryBlockService)
        {
            this.blockedCountryRepository = blockedCountryRepository;
            this.temporaryBlockService = temporaryBlockService;
        }

        // api/countries/block
        [HttpPost("blocked")]
        public IActionResult Countries (Country country)
        {
            if (string.IsNullOrWhiteSpace(country.CountryName) || string.IsNullOrWhiteSpace(country.CountryCode))
                return BadRequest("please the country code and county name are requied ");

            // check country is exist or not 
           var exsist= blockedCountryRepository.GetByCode(country.CountryCode);
            if (exsist != null)
                return Conflict("Country is blocked ");


            var blocked = new BlockedCountry
            {
                Country = new Country
                { CountryCode = country.CountryCode
                , CountryName = country.CountryName }
                ,IsTemporary=false
                ,UnblockTime=null 
            };

            blockedCountryRepository.Add(blocked);

            return Ok("COUNTRY BLOCKED SUCCESFULLY");
        }

        //*******************************************************************************************************************************
        [HttpGet("blocked")]
        public IActionResult Countries(
            [FromQuery] int page = 1
            , [FromQuery] int pagesize = 10
            , [FromQuery] string? serech = null)

        {
            var countryblocked = blockedCountryRepository.GetAll();
            if (!string.IsNullOrEmpty(serech))
            {
                serech = serech.ToLower();
                countryblocked = countryblocked.Where(cb =>
                cb.Country.CountryCode.ToLower().Contains(serech) ||
                cb.Country.CountryName.ToLower().Contains(serech)).ToList();
            };
            var totalCount = countryblocked.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagesize);

            var paged = countryblocked
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            var response = new
            {
                currentPage = page,
                pagesize,
                totalCount,
                totalPages,
                data = paged
            };

            return Ok(response);
        }

        //*********************************************************************************************************************************

        [HttpDelete("block/{countrycode}")]
        public IActionResult Countries(string countrycode)
        {
            if (string.IsNullOrWhiteSpace(countrycode))
                return BadRequest("please country code is required");


            var countryblocked = blockedCountryRepository.GetByCode(countrycode);
            if (countryblocked == null)
            
                return BadRequest("country is not blocked");

            blockedCountryRepository.Delete(countrycode);

            return Ok("Country Unblocked Successfully"); 
        }

        //**********************************************************************************************************************************

        [HttpPost("temporal-block")]
        public IActionResult TemporalBlock([FromBody] TemporalBlockRequest request)
        {
            try
            {
                // verify of duration minute 
                if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
                {
                    return BadRequest("Duration must be between 1 and 1440 minutes.");
                }

                // verify of country code 
                if (string.IsNullOrEmpty(request.CountryCode) || !Regex.IsMatch(request.CountryCode, @"^[A-Z]{2}$"))
                {
                    return BadRequest("Invalid country code. It must be a 2-letter code (e.g., 'EG').");
                }

                //  verify of dublicate blocked 
                if (temporaryBlockService.IsCountryTemporarilyBlocked(request.CountryCode))
                {
                    return Conflict($"Country {request.CountryCode} is already temporarily blocked.");
                }

                // add temp block 
                temporaryBlockService.AddTemporalBlock(request.CountryCode, request.DurationMinutes);
                return Ok($"Country {request.CountryCode} has been temporarily blocked for {request.DurationMinutes} minutes.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }

    public class TemporalBlockRequest
    {
        public string CountryCode { get; set; }
        public int DurationMinutes { get; set; }
    }
}

