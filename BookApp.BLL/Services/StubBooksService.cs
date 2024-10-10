using BookApp.BLL.Interfaces;
using BookApp.Domain.Entity;

namespace BookApp.BLL.Services
{
    public class StubBooksService : IBooksService
    {
        private static List<Book> _books =
            [
            new Book 
            {
                BookId = 1, 
                Title = "Johnny Mneminic", 
                Author = new Author { AuthorId = 1, Name = "William", Surname = "Gibson" }, 
                Year = 1981 
            },
            new Book 
            {
                BookId = 2, 
                Title = "Dune", 
                Author = new Author { AuthorId = 2, Name = "Frank", Surname = "Herbert" }, 
                Year = 1963
            },
            new Book 
            {
                BookId = 3,
                Title = "The Witcher",
                Author = new Author {AuthorId = 3, Name = "Andrzej", Surname = "Sapkowski" },
                Year = 1990
            }
            ];

        public IEnumerable<Book> GetAllBooks()
        {
            return _books;
        }

        public Book? GetBookById(int id)
        {
            return _books?.FirstOrDefault(s => s.BookId == id);
        }

        public void AddBook(Book book)
        {
            book.BookId = _books.Count > 0 ? _books.Max(s => s.BookId) + 1 : 1;
            _books.Add(book);
        }

        public void DeleteBook(int id)
        {
            var book = GetBookById(id);

            if (book != null)
            {
                _books.Remove(book);
            }
        }

        public void EditBook(Book book)
        {
            var existingBook = GetBookById(book.BookId);

            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Year = book.Year;
            }
        }
    }
}
