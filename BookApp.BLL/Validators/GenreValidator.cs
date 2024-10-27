using BookApp.Domain.Entity;
using FluentValidation;

namespace BookApp.BLL.Validators
{
    public class GenreValidator : AbstractValidator<Genre>
    {
        public GenreValidator() 
        {
            RuleFor(g => g.Name)
                .NotEmpty()
                .WithMessage("Name is reqierd");
        }
    }
}
