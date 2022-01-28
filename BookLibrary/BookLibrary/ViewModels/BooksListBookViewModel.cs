﻿using BookLibrary.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.ViewModels
{
    public class BooksListBookViewModel
    {
        public Book Book { get; set; }
        public List<IdentityUser> Customers { get; set; } 
    }
}