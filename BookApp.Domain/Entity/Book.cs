namespace BookApp.Domain.Entity
{
    public class Book
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public Author Author { get; set; }
        public int Year { get; set; }
    }
}
