using AirbnbREST.Models;

namespace AirbnbREST.DTOs;

public class PropertyUpdateDto
{
    public string? Title { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? StreetAddress { get; set; }
    public string? ZipCode { get; set; }
    public string? About { get; set; }
    public int? Bedrooms { get; set; }
    public decimal? Bathrooms { get; set; }
    public int? PricePerNight { get; set; }
    public int? SquareFeet { get; set; }
    public List<PropertyPhoto>? Photos { get; set; }
}
