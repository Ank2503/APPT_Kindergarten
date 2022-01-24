using Microsoft.AspNetCore.Identity;

namespace LibraryWeb.Models
{
    public class UserBook
    {
        public virtual int Id { get; set; }

        public virtual string UserId { get; set; }        
        public IdentityUser User { get; set; }

        public virtual int BookId { get; set; }
        public Book Book { get; set; }
    }
}
