using BookApp.Domain.Entity;
using FluentValidation;

namespace BookApp.BLL.Validators
{
    public class PublisherValidator : AbstractValidator<Publisher>
    {
        public PublisherValidator() 
        {
            RuleFor(p => p.Name)
                .NotEmpty();                

            RuleFor(p => p.Address)
                .NotEmpty();
        }
    }
}
