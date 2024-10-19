using BookApp.Domain.Entity;
using FluentValidation;

namespace BookApp.BLL.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator() 
        {
            RuleFor(b => b.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(1, 50).WithMessage("The Name must contain at least 1 to 50 characters");

            RuleFor(b => b.Author)
                .NotEmpty().WithMessage("Author is required")
                .Length(1, 50).WithMessage("The Author must contain at least 1 to 50 characters");

            RuleFor(b => b.Year)
                .NotNull().WithMessage("Year is required")                
                .InclusiveBetween(1000, DateTime.Now.Year).WithMessage("Year must be between 1000 and the current year.");

            RuleFor(b => b.Publisher)
                .NotNull().WithMessage("Author is required");

            RuleFor(b => b.Genre)
                .NotNull().WithMessage("Genre is required");
        }
    }
}
