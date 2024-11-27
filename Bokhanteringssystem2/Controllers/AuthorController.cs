using Bokhanteringssystem2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bokhanteringssystem2.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SelectAuthorBooks()
        {
            AuthorMethods authorMethods = new AuthorMethods();
            string errorMessage;

            // Hämta alla författare för dropdown-listan
            var authors = authorMethods.GetAuthorList(out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.Error = errorMessage;
            }

            return View(authors);
        }

        [HttpPost]
        public IActionResult SelectAuthorBooks(int authorId)
        {
            AuthorMethods authorMethods = new AuthorMethods();
            BookMethods bookMethods = new BookMethods();

            var author = authorMethods.GetAuthorList(out string errorMessage)
                                      .FirstOrDefault(a => a.AuthorID == authorId);

            if (author == null)
            {
                ViewBag.Error = "Author not found.";
                return View(new List<BookDetails>());
            }

            ViewBag.AuthorName = author.Name;

            var books = bookMethods.GetBooksByAuthorId(authorId, out errorMessage);

            if (books == null || !books.Any())
            {
                ViewBag.Error = "No books found for the selected author.";
            }

            ViewBag.Error = errorMessage;

            return View("DisplayBooks", books);
        }

        [HttpGet]
        public IActionResult SelectAuthors()
        {
            AuthorMethods authorMethods = new AuthorMethods();
            string errorMessage;

            var authors = authorMethods.GetAuthorList(out errorMessage);
            ViewBag.Error = errorMessage;

            return View(authors);
        }

        [HttpGet]
        public IActionResult InsertAuthor()
        {
            return View("InsertAuthor"); // Visa ett formulär för att lägga till en författare
        }

        [HttpPost]
        public IActionResult InsertAuthor(AuthorDetails authorDetails)
        {
            if (string.IsNullOrEmpty(authorDetails.Name))
            {
                ViewBag.Error = "Author name cannot be empty.";
                return View("InsertAuthor",authorDetails);
            }

            AuthorMethods authorMethods = new AuthorMethods();
            string errorMessage;
            int newAuthorID = authorMethods.InsertAuthor(authorDetails, out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.Error = errorMessage;
                return View("InsertAuthor",authorDetails);
            }

            ViewBag.Success = $"Author added successfully with ID {newAuthorID}.";
            return View();
        }

        [HttpGet]
        public IActionResult UpdateAuthor(int authorID)
        {
            AuthorDetails authorDetails = new AuthorDetails();
            AuthorMethods authorMethods = new AuthorMethods();
            string errorMessage = "";
            authorDetails = authorMethods.GetAuthor(authorID, out errorMessage);

            ViewBag.Error = errorMessage;

            return View(authorDetails);
        }

        [HttpPost]
        public IActionResult UpdateAuthor(AuthorDetails authorDetails)
        {
            AuthorMethods authorMethods = new AuthorMethods();
            string errorMessage = "";
            authorDetails = authorMethods.UpdateAuthor(authorDetails, out errorMessage);

            ViewBag.error = errorMessage;

            return View(authorDetails);
        }


    }
}
