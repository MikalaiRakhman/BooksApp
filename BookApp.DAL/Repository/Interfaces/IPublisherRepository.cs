using BookApp.Domain.Entity;

namespace BookApp.DAL.Repository.Interfaces
{
    public interface IPublisherRepository : IRepository<Publisher>
    {
        Task UpdateAsync(Publisher entity);
    }
}
