using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Airbnb_frontpages.Models;

public class PropertyDetailController : Controller
{
    private readonly HttpClient _httpClient;

    public PropertyDetailController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Index(int id)
    {
        Property? property = null;

        try
        {
            // Try fetching property details from API
            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5092/api/properties/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                property = JsonSerializer.Deserialize<Property>(jsonString);
            }
            else
            {
                property = LoadFromBackup(id); // Use backup JSON file
            }
        }
        catch
        {
            property = LoadFromBackup(id); // Fallback in case of API failure
        }

        return View(property);
    }

    private Property? LoadFromBackup(int id)
    {
        string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "listings.json");

        if (System.IO.File.Exists(jsonFilePath))
        {
            var json = System.IO.File.ReadAllText(jsonFilePath);
            var listings = JsonSerializer.Deserialize<List<Property>>(json) ?? new List<Property>();

            return listings.FirstOrDefault(p => p.Id == id); // Find the property by ID
        }
        return null; // Return null if file is missing or property isn't found
    }
}
