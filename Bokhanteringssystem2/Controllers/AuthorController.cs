﻿using Bokhanteringssystem2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bokhanteringssystem2.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAllAuthors()
        {
            AuthorMethods authorMethods = new AuthorMethods();
            string errorMessage;

            List<AuthorDetails> authors = authorMethods.GetAllAuthors(out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.Error = errorMessage;
                return View(new List<AuthorDetails>()); // Returnera tom lista om det finns ett fel
            }

            return View(authors);
        }

        [HttpGet]
        public IActionResult SelectAuthorBooks()
        {
            AuthorMethods authorMethods = new AuthorMethods();
            string errorMessage;

            // Hämta alla författare för dropdown-listan
            var authors = authorMethods.GetAllAuthors(out errorMessage);

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

            var author = authorMethods.GetAuthorDetailsList(out string errorMessage)
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

            var authors = authorMethods.GetAuthorDetailsList(out errorMessage);
            ViewBag.Error = errorMessage;

            return View(authors);
        }


    }
}