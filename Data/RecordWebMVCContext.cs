using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecordWebMVC.Models;

namespace RecordWebMVC.Data
{
    public class RecordWebMVCContext : DbContext
    {
        public RecordWebMVCContext (DbContextOptions<RecordWebMVCContext> options)
            : base(options)
        {
        }

        public DbSet<RecordWebMVC.Models.Person> Person { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Person>()
                .HasIndex(c => c.CPF)
                .IsUnique();
        }
    }
}
