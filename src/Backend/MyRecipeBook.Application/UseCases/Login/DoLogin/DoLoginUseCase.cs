using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _repository;
        private readonly PasswordEcripter _passwordEcripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public DoLoginUseCase
        (
            IUserReadOnlyRepository repository,
            PasswordEcripter passwordEcripter,
            IAccessTokenGenerator accessTokenGenerator
        )
        {
            _repository = repository;
            _passwordEcripter = passwordEcripter;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
        {
            Validate(request);

            var encriptedPassword = _passwordEcripter.Encrypt(request.Password);

            var user = await _repository.GetByEmailAndPassword(request.Email, encriptedPassword) ?? throw new InvalidLoginException();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
                }
            };
        }

        private static void Validate(RequestLoginJson request)
        {
            var validator = new DoLoginValidator();

            var result = validator.Validate(request);

            if (result.IsValid.IsFalse())
                throw new UnauthorizedAccessException();
        }
    }
}
