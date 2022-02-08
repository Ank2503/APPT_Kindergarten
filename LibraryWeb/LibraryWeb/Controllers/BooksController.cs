using LibraryWeb.Models;
using LibraryWeb.Services;
using LibraryWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryWeb.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IUserBookService _userBookService;

        public BooksController(IBookService bookService, IUserBookService userBookService)
        {
            _bookService = bookService;
            _userBookService = userBookService;
        }

        public ViewResult Index()
        {
            var books = _bookService.GetBooks().ToArray();
            var userBooks = _userBookService.UserBooks.ToArray();

            var takenBooks = userBooks.ToArray().Select(p => p.BookId).Distinct().ToArray();
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var booksVM = new BooksViewModel { Book = books, UserBook = userBooks, CurrentUserId = currentUserId };

            return View("Index", booksVM);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddBook()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
            await _bookService.AddBook(book);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var book = _bookService.GetBook(id);

                if (book != null)
                    return View(book);
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var book = _bookService.GetBook(id);

                if (book != null)
                    return View(book);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (book == null)
                return NotFound();

            await _bookService.UpdateBook(book);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
           
            var book = _bookService.GetBook(id);

            if (book != null)            
                await _bookService.DeleteBook(book);            

            return RedirectToAction("Index");                            
        }

        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> Take(int? id)
        {
            if (id == null)
                return NotFound();
            
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userBook = new UserBook
            {
                UserId = currentUserId,
                BookId = (int)id
            };
           
            await _userBookService.AddUserBook(userBook);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> Return(int? id)
        {
            if (id == null)
                return NotFound();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userBook = _userBookService.UserBooks.FirstOrDefault(ub => ub.BookId == id && 
                ub.UserId == currentUserId);

            if (userBook != null)            
                await _userBookService.DeleteUserBook(userBook);            
            
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
