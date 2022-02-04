using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext _db;
        private IBookRepository _repository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context, IBookRepository bookRepository)
        {
            _repository = bookRepository;
            _db = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var books = _repository.GetBooks();
            var userBooks = _repository.GetUserBooks();

            var bookView = new BookViewModel { Book = books, UserBook = userBooks };

            return View(bookView);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Take(int? bookId)
        {
            var takenBook = new UserBook
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                BookId = bookId
            };

            _db.TakenBooks.Add(takenBook);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Return(int? bookId)
        {
            UserBook userBook =
                await _db.TakenBooks.FirstOrDefaultAsync(p => p.BookId == bookId && p.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _db.TakenBooks.Remove(userBook);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
