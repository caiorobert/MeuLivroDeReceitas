using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _repository;
        private readonly PasswordEcripter _passwordEcripter;

        public DoLoginUseCase(IUserReadOnlyRepository repository, PasswordEcripter passwordEcripter)
        {
            _repository = repository;
            _passwordEcripter = passwordEcripter;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
        {
            Validate(request);

            var encriptedPassword = _passwordEcripter.Encrypt(request.Password);

            var user = await _repository.GetByEmailAndPassword(request.Email, encriptedPassword) ?? throw new InvalidLoginException();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name
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
