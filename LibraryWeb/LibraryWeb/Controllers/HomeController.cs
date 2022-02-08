using LibraryWeb.Services;
using LibraryWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;

namespace LibraryWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;
        private readonly IUserBookService _userBookService;

        public HomeController(ILogger<HomeController> logger, IBookService bookService, IUserBookService userBookService)
        {            
            _logger = logger;
            _bookService = bookService;
            _userBookService = userBookService;
        }

        public IActionResult Index()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userBooksId = _userBookService.UserBooks
                            .Where(ub => ub.UserId == currentUserId)
                            .Select(s => s.BookId).ToArray();

            var userBooks = _bookService.GetBooks().Where(b => userBooksId.Contains(b.Id)).ToArray();

            return View(new UserBooksViewModel { Book = userBooks });
        }
    }
}
