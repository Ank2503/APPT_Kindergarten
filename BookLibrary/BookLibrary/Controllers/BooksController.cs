﻿using BookLibrary.Data;
using BookLibrary.Models;
using BookLibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace BookLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBooksRepository _repository;

        public BooksController(ApplicationDbContext context, IBooksRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public ViewResult Index()
        {
            var books = _repository.GetBooks().ToArray();
            var userBooks = _context.UserBook.ToArray();

            var takenBooks = userBooks.ToArray().Select(p => p.BookId).Distinct().ToArray();
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var booksVM = new BooksViewModel { Book = books, UserBook = userBooks, CurrentUserId = currentUserId };

            if (User.IsInRole("admin"))
                return View("List", booksVM);
            return View("ReadOnlyList", booksVM);
        }

        [Authorize(Roles = "admin")]
        public ViewResult New()
        {
            var viewModel = new BookFormViewModel();
            return View("BookForm", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Save(Book book)
        {
            if(book.Id == 0)
                _context.Book.Add(book);
            else
            {
                var bookInDb = _context.Book.Single(b => b.Id == book.Id);
                bookInDb.Name = book.Name;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Books");
        }

        public IActionResult Details(int id)
        {
            var book = _context.Book.SingleOrDefault(m => m.Id == id);

            if (book == null)
                return Content("Page Not Found");

            return View(book);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var book = _context.Book.SingleOrDefault(b => b.Id == id);

            if (book == null)
                return NotFound();

            var viewModel = new BookFormViewModel
            {
                Book = book
            };
            return View("BookForm", viewModel);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Delete(Book book)
        {
            var bookInDb = _context.Book.Single(b => b.Id == book.Id);
            _context.Book.Remove(bookInDb);
            _context.SaveChanges();
            return RedirectToAction("Index", "Books");

        }

        [Authorize]
        public ViewResult Take(int id)
        {
            var book = _context.Book.SingleOrDefault(b => b.Id == id);
            return View(book);
        }

        [Authorize]
        public IActionResult TakeBook(int id)
        {
            var bookId = id;
            ClaimsPrincipal currentUser = User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userBook = new UserBook
            {
                UserId = currentUserID,
                BookId = bookId
            };

            _context.UserBook.Add(userBook);

            _context.SaveChanges();

            return RedirectToAction("Index", "Books");
        }

        [Authorize]
        public ViewResult Return(int id)
        {
            var book = _context.Book.SingleOrDefault(b => b.Id == id);
            return View(book);
        }

        [Authorize]
        public IActionResult ReturnBook(int id)
        {
            UserBook returnedBook = _context.UserBook
                .Where(b => b.BookId == id)
                .FirstOrDefault();

            _context.UserBook.Remove(returnedBook);
            _context.SaveChanges();

            return RedirectToAction("Index", "Books");
        }
    }
}