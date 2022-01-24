using LibraryWeb.Data;
using LibraryWeb.Models;
using LibraryWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace LibraryWeb.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) : base(context)
        {            
            _logger = logger;
        }

        public IActionResult Index()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userBooksId = DbContext.UserBook
                            .Where(ub => ub.UserId == currentUserId)
                            .Select(s => s.BookId).ToArray();

            var userBooks = DbContext.Book.Where(b => userBooksId.Contains(b.Id)).ToArray();

            return View(new UserBooksViewModel { Book = userBooks });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
