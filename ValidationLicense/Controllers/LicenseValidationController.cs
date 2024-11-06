using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LicenseValidationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseValidationController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public LicenseValidationController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateLicense([FromQuery] string licenceKey)
        {
            if (string.IsNullOrEmpty(licenceKey))
            {
                return BadRequest("License key is required.");
            }

            // Fetch the license from the database using Dapper
            var license = await _databaseService.GetLicenseByKeyAsync(licenceKey);

            if (license == null)
            {
                return NotFound("License not found.");
            }

            // Check if the license is within the valid date range
            if (license.ValidFrom > DateTime.Now)
            {
                return BadRequest("The license is not yet valid.");
            }

            if (license.ValidTo < DateTime.Now)
            {
                return BadRequest("The license has expired.");
            }

           // Return success if the license is valid
            return Ok(new
            {
                Message = "License is valid.",
                CompanyName = license.CompanyName,
                ValidFrom = license.ValidFrom,
                ValidTo = license.ValidTo
            });
        }
    }
}
