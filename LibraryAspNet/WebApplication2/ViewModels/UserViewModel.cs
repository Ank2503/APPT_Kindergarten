﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.ViewModels
{
    public class CreateUserViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public int Year { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public int Year { get; set; }
    }
}
