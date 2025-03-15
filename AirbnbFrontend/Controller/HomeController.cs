using Airbnb_frontpages.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Airbnb_frontpages.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            List<PropertyDto> listings = new List<PropertyDto>();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/properties");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    listings = JsonSerializer.Deserialize<List<PropertyDto>>(jsonString) ?? new List<PropertyDto>();
                }
            }
            catch
            {
                // Handle error or fallback as needed
            }

            var viewModel = new HomeViewModel { Listings = listings };
            return View(viewModel);
        }
    }
}
