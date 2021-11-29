using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using BookLibrary.Data;
using BookLibrary.Models;
using BookLibrary.ViewModels;

namespace BookLibrary.Controllers
{
    public class BooksController : Controller
    {
        public ViewResult Index()
        {
            var books = GetBooks();
            return View(books);
        }

        private IEnumerable<Book> GetBooks()
        {
            return new List<Book>
            {
                new Book { Id = 1, Name = "C# Basics" },
                new Book { Id = 2, Name = "Kolobok"}
            };
        }
        public IActionResult BooksList()
        {
            var book = new Book() { Name = "C# Basics" };
            var customers = new List<Customer>
            {
                new Customer {Name = "Customer 1"},
                new Customer {Name = "Customer 2"}
            };
            var viewModel = new BooksListBookViewModel()
            {
                Book = book,
                Customers = customers
            };
            return View(viewModel);
        }
    }
}
