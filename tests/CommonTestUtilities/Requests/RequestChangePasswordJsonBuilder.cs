using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestChangePasswordJsonBuilder
    {
        public static RequestChangePasswordJson Build(int passwordLenght = 10)
        {
            return new Faker<RequestChangePasswordJson>()
                .RuleFor(password => password.Password, (f) => f.Internet.Password())
                .RuleFor(password => password.NewPassword, (f) => f.Internet.Password(passwordLenght));
        }
    }
}