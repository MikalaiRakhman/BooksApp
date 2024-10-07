using BookApp.Domain.Entity;

namespace BookApp.BLL.Services
{
    public class StubBooksService
    {
        private static List<Book> _books =
            [
            new Book {Id = 1, Name = "Johnny Mneminic", Author = "William Gibson", Year = 1981 },
            new Book {Id = 2, Name = "Dune", Author = "Frank Herbert", Year = 1963},
            new Book {Id = 3, Name = "The Witcher", Author = "Andrzej Sapkowski", Year = 1990}
            ];

        public IEnumerable<Book> GetAllSkills()
        {
            return _books;
        }

        public Book? GetBookById(int id)
        {
            return _books?.FirstOrDefault(s => s.Id == id);
        }

        public void AddBook(Book book)
        {
            book.Id = _books.Count > 0 ? _books.Max(s => s.Id) + 1 : 1;
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

        public void UpdateBook(Book book)
        {
            var existingBook = GetBookById(book.Id);

            if (existingBook != null)
            {
                existingBook.Name = book.Name;
                existingBook.Author = book.Author;
                existingBook.Year = book.Year;
            }
        }
    }
}
