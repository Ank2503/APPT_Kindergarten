﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class BookViewModel
    {
        public List<Book> Book { get; set; }

        public List<UserBook> UserBook { get; set; }

    }
}
