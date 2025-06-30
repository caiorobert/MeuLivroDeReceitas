using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
//using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            /* SHOULDLY */
            result.ShouldNotBeNull();
            result.Tokens.ShouldNotBeNull();
            result.Name.ShouldSatisfyAllConditions(
                name => name.ShouldNotBeNullOrWhiteSpace(),
                name => name.ShouldBe(request.Name)
                );
            result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();

            /* FLUENT ASSERTIONS */
            //result.Should().NotBeNull();
            //result.Tokens.Should().NotBeNull();
            //result.Name.Should().Be(request.Name);
            //result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase(request.Email);

            Func<Task> act = async () => await useCase.Execute(request);

            /* SHOULDLY */
            var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();
            exception.ShouldSatisfyAllConditions(
                ex => ex.GetErrorMessages().ShouldHaveSingleItem(),
                ex => ex.GetErrorMessages().ShouldContain(ResourceMessagesExceptions.EMAIL_ALREADY_REGISTERED)
            );

            /* FLUENT ASSERTIONS */
            //(await act.Should().ThrowAsync<ErrorOnValidationException>())
            //    .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesExceptions.EMAIL_ALREADY_REGISTERED));
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);

            /* SHOULDLY */
            var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();
            exception.ShouldSatisfyAllConditions(
                ex => ex.GetErrorMessages().ShouldHaveSingleItem(),
                ex => ex.GetErrorMessages().ShouldContain(ResourceMessagesExceptions.NAME_EMPTY)
            );

            /* FLUENT ASSERTIONS */
            //(await act.Should().ThrowAsync<ErrorOnValidationException>())
            //    .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesExceptions.NAME_EMPTY));
        }

        private static RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var passwordEcripter = PasswordEcripterBuilder.Build();
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

            if (string.IsNullOrEmpty(email).IsFalse())
                readRepositoryBuilder.ExistActiveUserWithEmail(email!);

            return new RegisterUserUseCase(writeRepository, readRepositoryBuilder.Build(), unitOfWork, mapper, passwordEcripter, accessTokenGenerator);
        }
    }
}
