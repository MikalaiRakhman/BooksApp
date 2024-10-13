namespace BookApp.Domain.Entity
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
