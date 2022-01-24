using LibraryWeb.Models;

namespace LibraryWeb.ViewModels
{
    public class BooksViewModel
    {
        public Book[] Book { get; set; }
        public UserBook[] UserBook { get; set; }
        public string CurrentUserId { get; set; }
    }
}
