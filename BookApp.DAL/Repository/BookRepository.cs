using BookApp.DAL.Data;
using BookApp.DAL.Repository.Interfaces;
using BookApp.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookApp.DAL.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
        {
            return await _dbSet.Where(b => b.Author == author).ToListAsync();
        }

    }
}
