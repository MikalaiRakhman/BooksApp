using Microsoft.EntityFrameworkCore;
using BookApp.Domain.Entity;

namespace BookApp.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        DbSet<Book> Book { get; set; }
    }
}
