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

        // Bind the property id from the query string.
        [BindProperty(SupportsGet = true)]
        public Guid id { get; set; }

        public PropertyDto? Property { get; set; }
        public bool IsBooked { get; set; }
        public Guid? ActiveBookingId { get; set; }
        public string Message { get; set; }

        public PropertyDetailModel(HttpClient httpClient, ILogger<PropertyDetailModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve property details using the query string parameter 'id'
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5013/api/properties/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Property = JsonSerializer.Deserialize<PropertyDto>(jsonString, options);
            }
            if (Property == null)
                return NotFound();

            // Retrieve bookings for this property.
            var bookingResponse = await _httpClient.GetAsync($"http://localhost:5013/api/bookings/property/{id}");
            if (bookingResponse.IsSuccessStatusCode)
            {
                var bookingJson = await bookingResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var bookings = JsonSerializer.Deserialize<List<BookingDto>>(bookingJson, options);
                if (bookings != null)
                {
                    // Filter only confirmed bookings.
                    var confirmedBookings = bookings.Where(b => b.Status == "Confirmed").ToList();
                    if (confirmedBookings.Any())
                    {
                        IsBooked = true;
                        ActiveBookingId = confirmedBookings.First().Id;
                    }
                }
            }

            return Page();
        }

        // Handler for canceling a booking.
        public async Task<IActionResult> OnPostCancelAsync(Guid bookingId)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:5013/api/bookings/{bookingId}/cancel");
            // Pass the dummy guest ID via cookie header.
            request.Headers.Add("Cookie", "UserId=e8e20f27-465f-491e-8c8f-3fd548ea9c14");
            await _httpClient.SendAsync(request);
            // Reload the detail page with the property id preserved.
            return RedirectToPage(new { id = this.id });
        }

        // Handler for deleting the property.
        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var userId = "e8e20f27-465f-491e-8c8f-3fd548ea9c14";
            var request = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5013/api/properties/{id}");
            request.Headers.Add("Cookie", $"UserId={userId}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                // Redirect to the Index page after successful deletion.
                return RedirectToPage("/Index");
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                Message = $"Error deleting property: {response.StatusCode} - {errorMsg}";
                return Page();
            }
        }
    }
}
