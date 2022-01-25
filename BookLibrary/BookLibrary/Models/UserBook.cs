using Microsoft.AspNetCore.Identity;

namespace BookLibrary.Models
{
    public class UserBook
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } 

    }
}
