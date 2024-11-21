namespace Bokhanteringssystem2.Models
{
    public class AuthorDetails
    {
        public AuthorDetails() { }
        public int AuthorID { get; set; }
        public string? Name { get; set; }

        public List<BookDetails> Books { get; set; } = new List<BookDetails>(); // Navigationsegenskap
    }
}
