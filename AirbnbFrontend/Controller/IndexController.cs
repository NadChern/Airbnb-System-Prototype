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
        List<PropertyDto> listings = new List<PropertyDto>();

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/properties");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                listings = JsonSerializer.Deserialize<List<PropertyDto>>(jsonString) ?? new List<PropertyDto>();
            }
            else
            {
                listings = LoadFromBackup();
            }
        }
        catch
        {
            listings = LoadFromBackup();
        }

        return View(listings);
    }

    private List<PropertyDto> LoadFromBackup()
    {
        string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "listings.json");

        if (System.IO.File.Exists(jsonFilePath))
        {
            var json = System.IO.File.ReadAllText(jsonFilePath);
            return JsonSerializer.Deserialize<List<PropertyDto>>(json) ?? new List<PropertyDto>();
        }
        return new List<PropertyDto>();
    }
}
