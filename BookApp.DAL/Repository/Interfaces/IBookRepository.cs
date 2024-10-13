using BookApp.Domain.Entity;

namespace BookApp.DAL.Repository.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author);
    }
}
