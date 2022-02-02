using LibraryWeb.Data;
using LibraryWeb.Models;
using LibraryWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryWeb.Controllers
{
    public class BooksController : BaseController
    {
        private readonly IBookRepository _repository;

        public BooksController(ApplicationDbContext context, IBookRepository repository) : base(context)
        {
            _repository = repository;
        }

        public ViewResult Index()
        {
            var books = _repository.GetBooks().ToArray();
            var userBooks = DbContext.UserBook.ToArray();

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
            DbContext.Book.Add(book);
            await DbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var book = _repository.Get(id);

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
                var book = _repository.Get(id);

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

            DbContext.Book.Update(book);
            await DbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
           
            var book = _repository.Get(id);

            if (book != null)
            {
                DbContext.Entry(book).State = EntityState.Deleted;
                await DbContext.SaveChangesAsync();
            }

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
           
            DbContext.UserBook.Add(userBook);
            await DbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> Return(int? id)
        {
            if (id == null)
                return NotFound();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userBook = await DbContext.UserBook.FirstOrDefaultAsync(ub => ub.BookId == id && 
                ub.UserId == currentUserId);

            if (userBook != null)
            {
                DbContext.Entry(userBook).State = EntityState.Deleted;
                await DbContext.SaveChangesAsync();
            }
            
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
