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

        private IDbConnection GetSqlConnection(string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
                connectionString = _connectionString;

            return new SqlConnection(connectionString);
        }

        public List<Book> GetBooks()
        {
            using (var db = GetSqlConnection(_connectionString))
            {
                return db.Query<Book>("SELECT * FROM Book").ToList();
            }
        }

        public Book Get(int? id)
        {
            using (var db = GetSqlConnection(_connectionString))
            {
                return db.Query<Book>("SELECT * FROM Book WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }
    }
}
