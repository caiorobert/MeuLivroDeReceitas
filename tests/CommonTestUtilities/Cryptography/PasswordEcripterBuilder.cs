using MyRecipeBook.Application.Services.Cryptography;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEcripterBuilder
    {
        public static PasswordEcripter Build() => new PasswordEcripter("abc1234");
    }
}
