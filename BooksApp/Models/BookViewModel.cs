namespace BooksApp.WEB.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Author { get; set; }
        public int Year { get; set; }
        public int PublisherId { get; set; }
        public int GenreId { get; set; }
    }
}
