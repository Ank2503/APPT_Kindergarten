using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name= "Book title")]
        public string Name { get; set; }

    }
}
