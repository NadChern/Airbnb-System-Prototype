using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using Airbnb_frontpages.Models;

public class IndexController : Controller
{
    private readonly HttpClient _httpClient;

    public IndexController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Index()
    {
        List<Property> listings = new List<Property>();

        try
        {
            // Try fetching data from the API
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5092/api/properties");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                listings = JsonSerializer.Deserialize<List<Property>>(jsonString) ?? new List<Property>();
            }
            else
            {
                listings = LoadFromBackup(); // Use backup JSON file
            }
        }
        catch
        {
            listings = LoadFromBackup(); // Fallback in case of API failure
        }

        return View(listings);
    }

    private List<Property> LoadFromBackup()
    {
        string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "listings.json");

        if (System.IO.File.Exists(jsonFilePath))
        {
            var json = System.IO.File.ReadAllText(jsonFilePath);
            return JsonSerializer.Deserialize<List<Property>>(json) ?? new List<Property>();
        }
        return new List<Property>(); // Return empty list if file is missing
    }
}
