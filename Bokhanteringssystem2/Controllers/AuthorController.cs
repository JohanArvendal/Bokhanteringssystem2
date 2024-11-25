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
        public IActionResult SelectAuthor(int authorID)
        {
            AuthorMethods authorMethods = new AuthorMethods();
            string errorMessage;

            var author = authorMethods.GetAuthor(authorID, out errorMessage);
            ViewBag.Error = errorMessage;

            return View(author);
        }

        [HttpPost]
        public IActionResult UpdateAuthor(AuthorDetails authorDetails)
        {
            // Validera input
            if (authorDetails == null || string.IsNullOrEmpty(authorDetails.Name) || authorDetails.AuthorID <= 0)
            {
                ViewBag.Error = "Invalid author details. Please provide a valid Author ID and Name.";
                return View("SelectAuthor", authorDetails);
            }

            AuthorMethods authorMethods = new AuthorMethods();
            string errorMessage;
            AuthorDetails updatedAuthor = authorMethods.UpdateAuthor(authorDetails, out errorMessage);

            // Kontrollera om uppdateringen lyckades
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.Error = errorMessage;
                return View("SelectAuthor", authorDetails);
            }

            // Visa ett framgångsmeddelande
            ViewBag.Success = "Author updated successfully.";
            return View("SelectAuthor", updatedAuthor);
        }


    }
}
