namespace Bokhanteringssystem2.Models
{
    public class BookDetails
    {
        //Konstruktor
        public BookDetails() { }

        // Attribut
        public int BookID { get; set; }
        public string Title { get; set; }
        public int PublishedYear { get; set; }
        
        // FK till authur
        public int AuthorID { get; set; }
        public AuthorDetails? Author { get; set; } // Author kan vara null och behöver inte sättas varje gång
    }
}
