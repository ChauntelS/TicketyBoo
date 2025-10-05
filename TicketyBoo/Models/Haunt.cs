using Microsoft.Extensions.Primitives;

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

        public int ScareLevel { get; set; } // Scale of 1-5

        public DateTime Date { get; set; }

        //Foreign key
        public int CategoryId { get; set; }

        //Navigation property
        public Category? Category { get; set; }
    }
}
