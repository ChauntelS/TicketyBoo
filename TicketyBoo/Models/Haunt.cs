using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketyBoo.Models
{
    public class Haunt
    {
        public string ImagePath { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Organizer { get; set; } = string.Empty;

        [Display(Name = "Created")]
        public DateTime CreateDate { get; set; }

        public DateTime Date { get; set; }
        // Foreign key
        public int CategoryId { get; set; }
        // Navigation property
        public Category? Category { get; set; }

        [NotMapped]
        [Display(Name = "Photograph")]
        public IFormFile? FormFile { get; set; } // nullable
    }
}
