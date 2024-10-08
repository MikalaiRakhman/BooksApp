using BookApp.Domain.Entity;

namespace BookApp.BLL.Interfaces
{
    public interface IBooksService
    {
        IEnumerable<Book>? GetAllBooks();
        Book? GetBookById(int id);
        void AddBook(Book book);
        void EditBook(Book book);
        void DeleteBook(int id);
    }
}
