using LibraryWeb.Models;
using System.Threading.Tasks;

namespace LibraryWeb.Services
{
    public interface IUserBookService
    {
        public UserBook[] UserBooks { get; }

        public Task AddUserBook(UserBook userBook);

        public Task DeleteUserBook(UserBook userBook);
    }
}
