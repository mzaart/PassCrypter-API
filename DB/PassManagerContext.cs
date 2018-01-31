using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using PassCrypter.Models;

namespace PassCrypter.DB
{
    public class PassManagerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./passmanager.db");
        }
    }
}