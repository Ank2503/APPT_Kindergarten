using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BookLibrary.Models
{
    public class BooksRepository : IBooksRepository
    {
        public BooksRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public List<Book> GetBooks()
        {
            using IDbConnection db = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
            return db.Query<Book>("SELECT * FROM Book").ToList();
        }
    }
}
