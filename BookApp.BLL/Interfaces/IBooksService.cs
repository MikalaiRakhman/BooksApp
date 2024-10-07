using BookApp.Domain.Entity;

namespace BookApp.BLL.Interfaces
{
    internal interface IBooksService
    {
        IEnumerable<Book>? GetAllSkills();
        Book? GetBookById(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);
    }
}
