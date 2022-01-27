using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class UserBook
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int? BookId { get; set; }

        public User User { get; set; }

        public Book Book { get; set; }
    }
}
