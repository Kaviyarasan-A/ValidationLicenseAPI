using Microsoft.AspNetCore.Mvc;
using MvcWebApplication;
using MvcWebApplication.Services;
using System.Threading.Tasks;
using System.Web;

namespace MvcWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly LicenseValidationService _licenseValidationService;

        // Inject the LicenseValidationService through the constructor
        public HomeController(LicenseValidationService licenseValidationService)
        {
            _licenseValidationService = licenseValidationService;
        }

        // GET: Home/Index
        public ActionResult Index()
        {
            return View();
        }

        // POST: Home/ValidateLicense
        [HttpPost]
        public async Task<ActionResult> ValidateLicense(string licenseKey)
        {
            if (string.IsNullOrEmpty(licenseKey))
            {
                ViewBag.Result = "License key is required.";
                return View("Index");
            }

            // Call the LicenseValidationService to validate the license key
            var result = await _licenseValidationService.ValidateLicenseAsync(licenseKey);
            ViewBag.Result = result;

            // Return the view with the validation result
            return View("Index");
        }
    }
}
