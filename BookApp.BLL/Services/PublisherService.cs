using BookApp.Domain.Entity;
using BookApp.DAL.Repository.Interfaces;
using BookApp.BLL.Interfaces;

namespace BookApp.BLL.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherService(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public async Task AddPublisherAsync(Publisher publisher)
        {
            await _publisherRepository.AddAsync(publisher);
        }

        public async Task DeletePublisherAsync(int id)
        {
            await _publisherRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
        {
            return await _publisherRepository.GetAllAsync();
        }

        public Task<Publisher> GetPublisherByIdAsync(int id)
        {
            return _publisherRepository.GetByIdAsync(id);
        }

        public async Task UpdatePublisherAsync(Publisher publisher)
        {
            await _publisherRepository.UpdateAsync(publisher);
        }
    }
}
