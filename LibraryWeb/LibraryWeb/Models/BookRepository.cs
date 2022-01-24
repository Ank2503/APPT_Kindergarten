using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LibraryWeb.Models
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString = string.Empty;

        public BookRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Book> GetBooks()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Book>("SELECT * FROM Book").ToList();
            }
        }

        public Book Get(int? id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Book>("SELECT * FROM Book WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }
    }
}
