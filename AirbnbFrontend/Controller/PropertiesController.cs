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

    public async Task<IActionResult> Index(Guid id) // note: change parameter type to Guid
    {
        PropertyDto property = null;

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/properties/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                property = JsonSerializer.Deserialize<PropertyDto>(jsonString);
            }
            else
            {
                // property = LoadFromBackup(id);
            }
        }
        catch
        {
            // property = LoadFromBackup(id);
        }

        return View(property);
    }

    // private PropertyDto LoadFromBackup(Guid id)
    // {
    //     string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "listings.json");

    //     if (System.IO.File.Exists(jsonFilePath))
    //     {
    //         var json = System.IO.File.ReadAllText(jsonFilePath);
    //         var listings = JsonSerializer.Deserialize<List<PropertyDto>>(json) ?? new List<PropertyDto>();

    //         return listings.FirstOrDefault(p => p.Id == id);
    //     }
    //     return null;
    // }
}
