using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Forms.Models;
using System;
using System.Text;  // Assuming this is where you define your models (optional)

namespace YourMvcApp.Controllers
{
    
    public class LicenseController : Controller
    {
        // private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

        // Constructor to inject IHttpClientFactory
       // public LicenseController(IHttpClientFactory clientFactory)
       // {
        //    _clientFactory = clientFactory;
       // }

          public LicenseController(HttpClient httpClient)
       {
        _httpClient = httpClient;
       }




        // GET: License/Validate
        [HttpGet]
        public async Task<IActionResult> Validate(string licenceKey)
        {
            var requestPayload = new
            {
                LicenseKey = licenceKey,
                // Add additional data as required by the API
            };

            // Serialize object to JSON
            var jsonContent = JsonConvert.SerializeObject(requestPayload);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var url = $"https://localhost:44395/api/LicenseValidation/ValidateLicense/validate?licenceKey={{licenceKey}}";
            // Call the Web API to validate license
            var response = await _httpClient.GetAsync(url);
           // if (string.IsNullOrEmpty(licenceKey))
           // {
               
           // }

            

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var validationResult = JsonConvert.DeserializeObject<LicenseValidationResult>(responseJson);

                    if (string.IsNullOrEmpty(licenceKey))
                    {
                        return View("LicenseValid");
                    }
                    else
                    {
                        return View("LicenseInvalid");
                    }

                }
                else
                {
                    // Show error message in case the API call fails
                    ViewBag.ErrorMessage = $"Error calling API: {response.StatusCode}";
                    return View("Validate");
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Log the error and display a friendly message to the user
                Console.WriteLine($"HttpRequestException: {httpEx.Message}");
                ViewBag.ErrorMessage = $"An error occurred while sending the request: {httpEx.Message}";
                return View("Validate");
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Validate");
            }  // Renders the form for entering the license key
        }

        // POST: License/Validate
        [HttpPost("LicenseValidation")]
        public async Task<IActionResult> ValidateLicense(string licenceKey)
        {
            var requestPayload = new
            {
                LicenseKey = licenceKey,
                // Add additional data as required by the API
            };

            // Serialize object to JSON
            var jsonContent = JsonConvert.SerializeObject(requestPayload);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Call the Web API to validate license
            var response = await _httpClient.PostAsync("https://localhost:44395/api/LicenseValidation/ValidateLicense/validate?licenceKey={licenceKey}", content);
           // if (string.IsNullOrEmpty(licenceKey))
           // {
               
           // }

            

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var validationResult = JsonConvert.DeserializeObject<LicenseValidationResult>(responseJson);

                    if (string.IsNullOrEmpty(licenceKey))
                    {
                        return View("LicenseValid");
                    }
                    else
                    {
                        return View("LicenseInvalid");
                    }

                }
                else
                {
                    // Show error message in case the API call fails
                    ViewBag.ErrorMessage = $"Error calling API: {response.StatusCode}";
                    return View("Validate");
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Log the error and display a friendly message to the user
                Console.WriteLine($"HttpRequestException: {httpEx.Message}");
                ViewBag.ErrorMessage = $"An error occurred while sending the request: {httpEx.Message}";
                return View("Validate");
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Validate");
            }
        }
    }
}
