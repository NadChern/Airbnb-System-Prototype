using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Airbnb_frontpages.Models;

namespace Airbnb_frontpages.Pages
{
    public class BookingModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public BookingModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Bind the booking DTO from the form.
        [BindProperty]
        public CreateBookingDto BookingRequest { get; set; } = new CreateBookingDto();

        // Accept the property ID from query string.
        [BindProperty(SupportsGet = true)]
        public Guid PropertyId { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            // Assign the query string property ID to the booking DTO.
            if (PropertyId != Guid.Empty)
            {
                BookingRequest.PropertyId = PropertyId;
            }
            
            // Set default start date to today if not provided.
            if (BookingRequest.StartDate == default(DateTime))
            {
                BookingRequest.StartDate = DateTime.Today;
            }
            
            // Set default end date to tomorrow if not provided.
            if (BookingRequest.EndDate == default(DateTime))
            {
                BookingRequest.EndDate = DateTime.Today.AddDays(1);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Simulate a guest ID (dummy GUID)
            var guestId = "11111111-1111-1111-1111-111111111111";

            // Serialize the booking request to JSON.
            var json = JsonConvert.SerializeObject(BookingRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Create the request message to the booking API endpoint.
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5013/api/bookings")
            {
                Content = content
            };

            // Add a dummy guest ID in the Cookie header.
            request.Headers.Add("Cookie", $"UserId={guestId}");

            try
            {
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Message = "Booking created successfully!";
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Message = $"Error creating booking: {response.StatusCode} - {errorResponse}";
                }
            }
            catch (Exception ex)
            {
                Message = $"Unexpected error: {ex.Message}";
            }

            return Page();
        }
    }
}
