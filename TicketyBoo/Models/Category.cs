namespace TicketyBoo.Models
{
    public class Category
    {
        //Primary key
        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;


        public string Organizer { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        //Navigation property
        public List<Haunt>? Events { get; set; } // ? allows the list to be null

    }
}
