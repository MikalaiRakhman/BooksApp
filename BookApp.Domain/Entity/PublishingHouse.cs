namespace BookApp.Domain.Entity
{
    public class PublishingHouse
    {
        public int PublishingHouseId { get; set; }
        public required string Name { get; set; }

        public List<Book>? ListOfBooks { get; set; }
        public int MaximumCirculation {  get; set; } 
    }
}
