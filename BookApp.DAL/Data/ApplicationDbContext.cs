using Microsoft.EntityFrameworkCore;
using BookApp.Domain.Entity;

namespace BookApp.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Genre> Genres { get; set; }
  

protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId);

            DbInitializer.SeedData(modelBuilder);
        }
    }
}
