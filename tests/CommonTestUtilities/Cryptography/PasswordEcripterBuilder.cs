using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEcripterBuilder
    {
        public static IPasswordEncripter Build() => new Sha512Ecripter("abc1234");
    }
}
