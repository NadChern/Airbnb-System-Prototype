using System;
using System.ComponentModel.DataAnnotations;

namespace Airbnb_frontpages.Models
{
    public class CreateBookingDto
    {
        [Required(ErrorMessage = "Property ID is required.")]
        public Guid PropertyId { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }
    }
}
