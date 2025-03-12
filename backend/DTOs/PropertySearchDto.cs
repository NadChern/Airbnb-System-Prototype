namespace AirbnbREST.DTOs;

public class PropertySearchDto
{
    public string? Title { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public int? MinBedrooms { get; set; }
    public int? MaxBedrooms { get; set; }
    public int? MinBathrooms { get; set; }
    public int? MaxBathrooms { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public DateTime? StartDate { get; set; } 
    public DateTime? EndDate { get; set; } 
}
