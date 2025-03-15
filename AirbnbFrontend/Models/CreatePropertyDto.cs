using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airbnb_frontpages.Models
{
    public class CreatePropertyDto
    {
        [Required]
        public Guid Owner { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        public string? About { get; set; }

        [Required(ErrorMessage = "Street address is required.")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip code is required.")]
        public string ZipCode { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "At least one bedroom is required.")]
        public int Bedrooms { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "Bathrooms must be greater than 0.")]
        public decimal Bathrooms { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Square feet must be greater than 0.")]
        public int SquareFeet { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Price per night must be greater than 0.")]
        public int PricePerNight { get; set; }

        // If your BE expects a list of photo URLs
        public List<string> Photos { get; set; } = new List<string>();
    }
}
