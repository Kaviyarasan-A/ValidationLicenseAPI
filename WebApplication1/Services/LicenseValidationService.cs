using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcWebApplication.Services
{
    public class LicenseValidationService
    {
        private static readonly HttpClient client = new HttpClient();

        // API endpoint URL
        private readonly string _apiUrl = "https://localhost:44395/api";

        // Method to validate the license key via the Web API
        public async Task<string> ValidateLicenseAsync(string licenseKey)
        {
            if (string.IsNullOrEmpty(licenseKey))
            {
                return "License key is required.";
            }

            // Build the request URL with the query string parameter
            var requestUrl = $"{_apiUrl}?licenceKey={licenseKey}";

            try
            {
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to get the success message and other details
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent; // You can return a detailed response or just the message
                }
                else
                {
                    // Handle the error response
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"Error: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }
    }
}
