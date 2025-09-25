namespace TicketyBoo.Models
{
    public class Category
    {
        //Primary key
        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Type { get; set; }

        public string Organizer { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        //Navigation property
        public List<Haunts>? Events { get; set; } // ? allows the list to be null

    }
}
