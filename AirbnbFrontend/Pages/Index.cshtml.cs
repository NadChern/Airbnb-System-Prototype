using Airbnb_frontpages.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Airbnb_frontpages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IndexModel> _logger;

        public List<PropertyDto> Listings { get; set; } = new List<PropertyDto>();

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? MinPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? MaxPrice { get; set; }

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
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var allListings = JsonSerializer.Deserialize<List<PropertyDto>>(jsonString, options) ?? new List<PropertyDto>();

                    // Apply filtering in C#
                    if (!string.IsNullOrEmpty(SearchTerm))
                        allListings = allListings.Where(l => l.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

                    if (MinPrice.HasValue)
                        allListings = allListings.Where(l => l.PricePerNight >= MinPrice.Value).ToList();

                    if (MaxPrice.HasValue)
                        allListings = allListings.Where(l => l.PricePerNight <= MaxPrice.Value).ToList();

                    Listings = allListings;
                    _logger.LogInformation($"✅ API data retrieved. {Listings.Count} properties found after filtering.");
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
