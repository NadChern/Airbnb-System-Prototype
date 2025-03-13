using Airbnb_frontpages.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Airbnb_frontpages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IndexModel> _logger;

        public List<PropertyDto> Listings { get; set; } = new List<PropertyDto>();

        public IndexModel(HttpClient httpClient, ILogger<IndexModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task OnGet()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/properties");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    // Use options to be case-insensitive if you prefer, though JsonPropertyName attributes should handle it.
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Listings = JsonSerializer.Deserialize<List<PropertyDto>>(jsonString, options) ?? new List<PropertyDto>();
                    _logger.LogInformation("✅ API data successfully retrieved.");
                }
                else
                {
                    _logger.LogWarning("⚠ API call failed (Status: {0}).", response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("❌ API Request failed: {0}.", ex.Message);
            }
        }
    }
}
