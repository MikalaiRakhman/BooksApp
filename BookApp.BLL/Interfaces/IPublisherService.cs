using BookApp.Domain.Entity;

namespace BookApp.BLL.Interfaces
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetAllPublishersAsync();
        Task<Publisher> GetPublisherByIdAsync(int id);
        Task AddPublisherAsync(Publisher publisher);
        Task UpdatePublisherAsync(Publisher publisher);
        Task DeletePublisherAsync(int id);
    }
}
