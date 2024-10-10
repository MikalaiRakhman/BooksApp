using System.ComponentModel.DataAnnotations;

namespace BooksApp.WEB.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(50, ErrorMessage = "Author can't be longer than 50 characters.")]
        public required string Author { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(1800, 2025, ErrorMessage = "Year must be between 1800 and 2025.")]
        public int Year { get; set; }
    }
}
