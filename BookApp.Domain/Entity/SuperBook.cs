namespace BookApp.Domain.Entity
{
    public class SuperBook : Book
    {
        public bool IsBestseller { get; set; }
        public int Circulation { get; set; }
    }
}
