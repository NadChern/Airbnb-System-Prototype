using Airbnb_frontpages.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Airbnb_frontpages.Pages
{
    public class PropertyDetailModel : PageModel
    {
        public Property? Property { get; set; }

        public void OnGet(int id)
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "listings.json");

            if (System.IO.File.Exists(jsonFilePath))
            {
                var json = System.IO.File.ReadAllText(jsonFilePath);
                var listings = JsonSerializer.Deserialize<List<Property>>(json) ?? new List<Property>();

                Property = listings.FirstOrDefault(p => p.Id == id);
            }
        }
    }
}
