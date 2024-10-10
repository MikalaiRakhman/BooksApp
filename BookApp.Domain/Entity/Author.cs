namespace BookApp.Domain.Entity
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        List<Book> ListOfBooks { get; set; }
    }
}
