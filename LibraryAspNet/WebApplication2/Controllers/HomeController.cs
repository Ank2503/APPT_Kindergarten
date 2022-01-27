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

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _db = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Books"] = await _db.Books.ToListAsync();
            ViewData["UserBooks"] = await _db.TakenBooks.ToListAsync();
            return View();
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
            UserBook userBook = await _db.TakenBooks.FirstOrDefaultAsync(p => p.BookId == bookId && p.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
