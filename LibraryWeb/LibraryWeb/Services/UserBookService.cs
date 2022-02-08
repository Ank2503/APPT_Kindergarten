using LibraryWeb.Data;
using LibraryWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWeb.Services
{
    public class UserBookService : IUserBookService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserBookService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        UserBook[] IUserBookService.UserBooks => _dbContext.UserBook.ToArray();

        public async Task AddUserBook(UserBook userBook)
        {
            _dbContext.UserBook.Add(userBook);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserBook(UserBook userBook)
        {
            _dbContext.Entry(userBook).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }
    }
}
