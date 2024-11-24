using Microsoft.AspNetCore.Mvc;
using Bokhanteringssystem2.Models;


namespace Bokhanteringssystem2.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InsertBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertBook(string bookTitle, int publishedYear, string authorName)
        {
            if (string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(authorName))
            {
                ViewBag.Error = "Book title and author name are required.";
                return View();
            }

            AuthorMethods authorMethods = new AuthorMethods();
            BookMethods bookMethods = new BookMethods();
            string errorMessage;

            // Kontrollera om författaren finns
            int authorID = authorMethods.GetAuthorIdByName(authorName, out errorMessage);

            if (authorID == 0)
            {
                // Om författaren inte finns, lägg till den
                authorID = authorMethods.InsertAuthor(new AuthorDetails { Name = authorName }, out errorMessage);

                if (authorID == 0)
                {
                    ViewBag.Error = $"Failed to add author: {errorMessage}";
                    return View();
                }
            }

            // Lägg till boken
            var newBook = new BookDetails
            {
                Title = bookTitle,
                PublishedYear = publishedYear,
                AuthorID = authorID
            };

            int rowsAffected = bookMethods.InsertBook(newBook, out errorMessage);

            if (rowsAffected > 0)
            {
                ViewBag.Message = "Book successfully added.";
            }
            else
            {
                ViewBag.Error = $"Failed to add book: {errorMessage}";
            }

            return View();
        }


        public ActionResult SelectBooks()
        {
            List<BookDetails> bookDetailsList = new List<BookDetails>();
            BookMethods bookMethods = new BookMethods(); // Skapa en ny instans
            string error = string.Empty;

            // Hämta listan med böcker
            var result = bookMethods.GetBookList(out error);

            if (result != null)
            {
                bookDetailsList = result;
            }

            ViewBag.error = error; // Skicka eventuellt felmeddelande till vyn
            return View(bookDetailsList); // Returnera listan till vyn
        }

    }
}
