using Airbnb_frontpages.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Airbnb_frontpages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IndexModel> _logger;
        private readonly string jsonFilePath = "wwwroot/data/listings.json"; // ✅ Ensure correct path

        public List<Property> Listings { get; set; } = new List<Property>();

        public IndexModel(HttpClient httpClient, ILogger<IndexModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task OnGet()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5092/api/properties");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    Listings = JsonSerializer.Deserialize<List<Property>>(jsonString) ?? new List<Property>();
                    _logger.LogInformation("✅ API data successfully retrieved.");
                }
                else
                {
                    _logger.LogWarning("⚠ API failed (Status: {0}), switching to JSON backup.", response.StatusCode);
                    Listings = LoadFromBackup();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("❌ API Request failed: {0}. Using JSON backup.", ex.Message);
                Listings = LoadFromBackup();
            }
        }

        private List<Property> LoadFromBackup()
        {
            if (System.IO.File.Exists(jsonFilePath))
            {
                _logger.LogInformation("📂 Loading properties from JSON backup.");

                try
                {
                    var json = System.IO.File.ReadAllText(jsonFilePath);
                    return JsonSerializer.Deserialize<List<Property>>(json) ?? new List<Property>();
                }
                catch (JsonException ex)
                {
                    _logger.LogError("❌ JSON Parsing Error: {0}", ex.Message);
                }
            }
            else
            {
                _logger.LogError("🚨 JSON file not found at: {0}", jsonFilePath);
            }
            return new List<Property>(); // Empty list if file is missing or unreadable
        }
    }
}
