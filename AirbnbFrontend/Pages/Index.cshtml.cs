using Airbnb_frontpages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

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
                // Fetch properties from BE.
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5013/api/properties");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var allListings = JsonSerializer.Deserialize<List<PropertyDto>>(jsonString, options) ?? new List<PropertyDto>();

                    // Apply search and price filtering.
                    if (!string.IsNullOrEmpty(SearchTerm))
                        allListings = allListings.Where(l => l.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                    if (MinPrice.HasValue)
                        allListings = allListings.Where(l => l.PricePerNight >= MinPrice.Value).ToList();
                    if (MaxPrice.HasValue)
                        allListings = allListings.Where(l => l.PricePerNight <= MaxPrice.Value).ToList();

                    // For each property, check bookings to determine status.
                    foreach (var prop in allListings)
                    {
                        var bookingResponse = await _httpClient.GetAsync($"http://localhost:5013/api/bookings/property/{prop.Id}");
                        if (bookingResponse.IsSuccessStatusCode)
                        {
                            var bookingJson = await bookingResponse.Content.ReadAsStringAsync();
                            var propertyBookings = JsonSerializer.Deserialize<List<BookingDto>>(bookingJson, options);
                            if (propertyBookings != null)
                            {
                                // Only consider Confirmed bookings
                                var confirmedBookings = propertyBookings
                                    .Where(b => b.Status == "Confirmed")
                                    .ToList();

                                if (confirmedBookings.Any())
                                {
                                    prop.IsBooked = true;
                                    prop.ActiveBookingId = confirmedBookings.First().Id;
                                }
                            }

                        }
                    }

                    Listings = allListings;
                    _logger.LogInformation($"✅ API data retrieved. {Listings.Count} properties loaded.");
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

        // Cancel booking handler.
        public async Task<IActionResult> OnPostCancelAsync(Guid bookingId)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:5013/api/bookings/{bookingId}/cancel");
            // Hardcoded guest ID cookie.
            request.Headers.Add("Cookie", "UserId=e8e20f27-465f-491e-8c8f-3fd548ea9c14");
            var response = await _httpClient.SendAsync(request);
            return RedirectToPage();
        }
    }
}
