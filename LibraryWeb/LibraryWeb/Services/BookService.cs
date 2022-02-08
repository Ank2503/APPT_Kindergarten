using Dapper;
using LibraryWeb.Data;
using LibraryWeb.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWeb.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SqlConnection _sqlConnection;
       
        public BookService(IConfiguration configuration, ApplicationDbContext context)
        {
            _sqlConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            _dbContext = context;
        }

        public Book GetBook(int? id)
        {
            using (_sqlConnection)
            {
                return _sqlConnection.Query<Book>("SELECT * FROM Book WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

        public List<Book> GetBooks()
        {
            using (_sqlConnection)
            {
                return _sqlConnection.Query<Book>("SELECT * FROM Book").ToList();
            }
        }

        public async Task AddBook(Book book)
        {
            _dbContext.Book.Add(book);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBook(Book book)
        {
            _dbContext.Book.Update(book);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBook(Book book)
        {
            _dbContext.Entry(book).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }
    }
}
