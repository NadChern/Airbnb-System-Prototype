using Airbnb_frontpages.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace Airbnb_frontpages.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "listings.json");

            // Ensure the file exists
            if (!System.IO.File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException("Listings JSON file not found.");
            }

            // Read the JSON file
            var jsonData = System.IO.File.ReadAllText(jsonFilePath);
            var listings = JsonConvert.DeserializeObject<List<AirbnbListing>>(jsonData);

            // Create the view model
            var viewModel = new HomeViewModel
            {
                Listings = listings
            };

            // Return the view with the model
            return View(viewModel);
        }
    }
}
