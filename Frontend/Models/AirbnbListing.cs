namespace Airbnb_frontpages.Models
{
    public class AirbnbListing
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int SquareFeet { get; set; }
        public decimal PricePerNight { get; set; }
        public string About { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhotoLink { get; set; }
    }
}