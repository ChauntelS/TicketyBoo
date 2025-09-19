namespace TicketyBoo.Models
{
    public class Category
    {
        //Primary key
        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        //Navigation property
        public List<Event>? Events { get; set; } // ? allows the list to be null

    }
}
