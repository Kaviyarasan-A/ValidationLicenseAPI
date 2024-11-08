using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LicenseValidationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LicenseValidationController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public LicenseValidationController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // Endpoint to validate license and return company + sub-companies
        [HttpGet("validate")]
        public async Task<IActionResult> ValidateLicense([FromQuery] string licenceKey)
        {
            if (string.IsNullOrEmpty(licenceKey))
            {
                return BadRequest("License key is required.");
            }

            var companyLicense = await _databaseService.GetLicenseByKeyAsync(licenceKey);

            if (companyLicense == null)
            {
                return NotFound("License not found.");
            }

            if (companyLicense.ValidFrom > DateTime.Now)
            {
                return BadRequest("The license is not yet valid.");
            }

            if (companyLicense.ValidTo < DateTime.Now)
            {
                return BadRequest("The license has expired.");
            }

            return Ok(new
            {
                Message = "License is valid.",
                CompanyName = companyLicense.CompanyName,
                SubCompanies = companyLicense.SubCompanies ?? new List<SubCompany>()
            });
        }

        // New endpoint to get sub-company details by name
        [HttpGet("subcompany/{subCompanyName}")]
        public async Task<IActionResult> GetSubCompanyDetails(string subCompanyName)
        {
            if (string.IsNullOrEmpty(subCompanyName))
            {
                return BadRequest("Sub-company name is required.");
            }

            var subCompanyDetails = await _databaseService.GetSubCompanyDetailsAsync(subCompanyName);

            if (subCompanyDetails == null)
            {
                return NotFound("Sub-company not found.");
            }

            return Ok(subCompanyDetails); // Return the sub-company details
        }

        // Alternatively, you can use this endpoint to get by SubCompanyId
        [HttpGet("subcompany/id/{subCompanyId}")]
        public async Task<IActionResult> GetSubCompanyDetailsById(int subCompanyId)
        {
            var subCompanyDetails = await _databaseService.GetSubCompanyDetailsByIdAsync(subCompanyId);

            if (subCompanyDetails == null)
            {
                return NotFound("Sub-company not found.");
            }

            return Ok(subCompanyDetails); // Return the sub-company details
        }
    }
}
