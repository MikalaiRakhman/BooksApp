using BookApp.DAL.Data;
using BookApp.DAL.Repository.Interfaces;
using BookApp.Domain.Entity;

namespace BookApp.DAL.Repository
{
    public class PublisherRepository : Repository<Publisher>, IPublisherRepository
    {
        private readonly ApplicationDbContext _context;
        public PublisherRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(Publisher entity)
        {
            var originalEntity = await _context.Publishers.FindAsync(entity.Id);

            if (originalEntity != null)
            {
                originalEntity.Name = entity.Name;
                originalEntity.Address = entity.Address;
                originalEntity.Id = entity.Id;
                originalEntity.Books = entity.Books;

                await _context.SaveChangesAsync();
            }
        }
    }
}
