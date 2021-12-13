using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models
{
    public interface IUserRepository
    {
        List<Book> GetBooks();
    }
    public class UserRepository : IUserRepository
    {
        string connectionString = null;
        public UserRepository(string conn)
        {
            connectionString = conn;
        }
        public List<Book> GetBooks()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Book>("SELECT * FROM Book").ToList();
            }
        }
    }
}
