using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class BookRepository : IBookRepository
    {
        private string _connectionString = null;

        private IDbConnection _dbConnection = null;

        public BookRepository(string connection)
        {
            _connectionString = connection;
            _dbConnection = new SqlConnection(_connectionString);
        }

        public Book Get(int? id)
        {
            return _dbConnection.Query<Book>("SELECT * FROM Books WHERE Id = @id", id).FirstOrDefault();
        }

        public List<Book> GetBooks()
        {
            return _dbConnection.Query<Book>("SELECT * FROM Books").ToList();
        }

        public List<UserBook> GetUserBooks()
        {
            return _dbConnection.Query<UserBook>("SELECT * FROM TakenBooks").ToList();
        }
    }
}
