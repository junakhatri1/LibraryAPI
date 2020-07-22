using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Domain
{
    public class LibraryDataContext : DbContext
    {
        public LibraryDataContext(DbContextOptions<LibraryDataContext> ctx): base(ctx) { }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().Property(b => b.Title).HasMaxLength(200);
            modelBuilder.Entity<Book>().Property(b => b.Author).HasMaxLength(200);
            // etc. etc.

            //modelBuilder.Entity<Book>().HasData(
            //    new Book {  Id =1, Title="Walden", Author="Thoreau", Genre="Non-Fiction", InStock=true, NumberOfPages = 128},
            //    new Book {  Id =2, Title="Nature", Author="Emerson", Genre="Non-Fiction", InStock=true, NumberOfPages = 328}

            //    );

        }
    }
}
