using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin
{
    public class DoLoginValidator : AbstractValidator<RequestLoginJson>
    {
        public DoLoginValidator()
        {
            RuleFor(user => user.Email).NotEmpty();
            RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6);
            When(user => user.Email.NotEmpty(), () =>
            {
                RuleFor(user => user.Email).EmailAddress();
            });
        }
    }
}
