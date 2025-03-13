using Airbnb_frontpages.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Airbnb_frontpages.Pages
{
    public class PropertyDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PropertyDetailModel> _logger;

        public PropertyDto? Property { get; set; }

        public PropertyDetailModel(HttpClient httpClient, ILogger<PropertyDetailModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/properties/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Property = JsonSerializer.Deserialize<PropertyDto>(jsonString, options);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API request failed: {Message}", ex.Message);
            }

            if (Property == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
