using Airbnb_frontpages.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace Airbnb_frontpages.Pages
{
    public class IndexModel : PageModel
    {
        public List<AirbnbListing> Listings { get; set; }

        public void OnGet()
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "listings.json");

            // Ensure that the file exists
            if (!System.IO.File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException("Listings JSON file not found.");
            }

            // Read the content of the JSON file
            var jsonData = System.IO.File.ReadAllText(jsonFilePath); // Correct usage

            // Deserialize the JSON content into a list of AirbnbListing objects
            Listings = JsonConvert.DeserializeObject<List<AirbnbListing>>(jsonData);
        }
    }
}
