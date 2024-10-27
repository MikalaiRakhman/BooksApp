using BookApp.Domain.Entity;

namespace BookApp.DAL.Repository.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task UpdateAsync(Genre entity);
    }
}
