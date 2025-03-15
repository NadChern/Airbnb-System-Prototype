using Airbnb_frontpages.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Airbnb_frontpages.Pages
{
    public class PropertyDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PropertyDetailModel> _logger;

        public PropertyDto? Property { get; set; }
        public bool IsBooked { get; set; }
        public Guid? ActiveBookingId { get; set; }

        public PropertyDetailModel(HttpClient httpClient, ILogger<PropertyDetailModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            // 1. Retrieve property data
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5013/api/properties/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Property = JsonSerializer.Deserialize<PropertyDto>(jsonString, options);
            }
            if (Property == null)
                return NotFound();

            // 2. Check for active booking by calling bookings endpoint
            var bookingResponse = await _httpClient.GetAsync($"http://localhost:5013/api/bookings/property/{id}");
            if (bookingResponse.IsSuccessStatusCode)
            {
                var bookingJson = await bookingResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var bookings = JsonSerializer.Deserialize<List<BookingDto>>(bookingJson, options);
                if (bookings != null && bookings.Any())
                {
                    // Mark as booked and capture the first booking ID as the active one.
                    IsBooked = true;
                    ActiveBookingId = bookings.First().Id;
                }
            }

            return Page();
        }

        // Cancel booking handler from detail page
        public async Task<IActionResult> OnPostCancelAsync(Guid bookingId)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:5013/api/bookings/{bookingId}/cancel");
            // Pass the dummy guest ID via cookie header.
            request.Headers.Add("Cookie", "UserId=e8e20f27-465f-491e-8c8f-3fd548ea9c14");
            await _httpClient.SendAsync(request);

            // Reload the detail page with the same property ID.
            return RedirectToPage(new { id = Property?.Id });
        }
    }
}
