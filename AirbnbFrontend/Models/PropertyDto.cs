using System;
using System.Collections.Generic;

namespace Airbnb_frontpages.Models
{
    public class PropertyDto
    {
        public Guid Id { get; set; }
        public Guid Owner { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int SquareFeet { get; set; }
        public int PricePerNight { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public List<PropertyPhotoDto>? Photos { get; set; }
    }

    public class PropertyPhotoDto
    {
        public Guid Id { get; set; }
        public string PhotoUrl { get; set; }
    }
}
