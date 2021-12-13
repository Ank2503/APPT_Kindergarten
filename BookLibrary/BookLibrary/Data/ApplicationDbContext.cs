using BookLibrary.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookLibrary.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        //public DbSet<Customer> Customers { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<UserBook> UserBook { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
