namespace TicketyBoo.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public DateTime Creation { get; set; }

        //Foreign key
        public int CategoryId { get; set; }

        //Navigation property
        public Category? Category { get; set; }
    }
}
