using BookApp.DAL.Data;
using BookApp.DAL.Repository.Interfaces;
using BookApp.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookApp.DAL.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
        {
            return await _context.Books.Where(b => b.Author == author).ToListAsync();
        }


        public async Task UpdateAsync(Book entity)
        {
            var originalEntity = await _context.Books.FindAsync(entity.Id);

            if (originalEntity != null)
            {
                originalEntity.Name = entity.Name;
                originalEntity.Author = entity.Author;
                originalEntity.Year = entity.Year;
                originalEntity.PublisherId = entity.PublisherId;
                originalEntity.Publisher = await _context.Publishers.FindAsync(entity.PublisherId);
                originalEntity.GenreId = entity.GenreId;
                originalEntity.Genre = await _context.Genres.FindAsync(entity.GenreId);

                await _context.SaveChangesAsync();
            }
        }
    }
}
