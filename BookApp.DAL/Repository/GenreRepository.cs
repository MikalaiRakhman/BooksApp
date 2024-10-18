using BookApp.DAL.Data;
using BookApp.DAL.Repository.Interfaces;
using BookApp.Domain.Entity;

namespace BookApp.DAL.Repository
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        private readonly ApplicationDbContext _context;
        public GenreRepository (ApplicationDbContext context) : base (context)
        {
            _context = context;
        }
        public async Task UpdateAsync(Genre entity)
        {
            var originalEntity = await _context.Genres.FindAsync(entity.Id);

            if (originalEntity != null)
            {
                originalEntity.Name = entity.Name;
                originalEntity.Id = entity.Id;
                originalEntity.Books = entity.Books;

                await _context.SaveChangesAsync();
            }
        }
    }
}
