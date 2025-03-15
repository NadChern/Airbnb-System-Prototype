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
        public decimal Bathrooms { get; set; }
        public int SquareFeet { get; set; }
        public int PricePerNight { get; set; }
        public required string Title { get; set; }
        public string? About { get; set; }
        public required string StreetAddress { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string ZipCode { get; set; }
        public List<PropertyPhotoDto>? Photos { get; set; }

        // Booking status fields for UI.
        public bool IsBooked { get; set; }
        public Guid? ActiveBookingId { get; set; }
    }

    public class PropertyPhotoDto
    {
        public Guid Id { get; set; }
        public required string PhotoUrl { get; set; }
    }
}
