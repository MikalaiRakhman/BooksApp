using BookApp.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookApp.DAL.Data
{
    class DbInitializer
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id = 1, Name = "Penguin Random House", Address = "New York, NY" },
                new Publisher { Id = 2, Name = "HarperCollins", Address = "New York, NY" }
            );

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Fantasy" },
                new Genre { Id = 2, Name = "Science Fiction" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Name = "The Hobbit", Author = "J.R.R. Tolkien", Year = 1937, PublisherId = 1, GenreId = 1 },
                new Book { Id = 2, Name = "1984", Author = "George Orwell", Year = 1949, PublisherId = 2, GenreId = 2 }
            );
        }
    }
}
