using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
//using FluentAssertions;
using Shouldly;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(new RequestLoginJson
            {
                Email = user.Email,
                Password = password
            });

            /* SHOULDLY */
            result.ShouldNotBeNull();
            result.Tokens.ShouldNotBeNull();
            result.Name.ShouldSatisfyAllConditions(
                name => name.ShouldNotBeNullOrWhiteSpace(),
                name => name.ShouldBe(user.Name)
                );
            result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();

            /* FLUENT ASSERTIONS */
            //result.Should().NotBeNull();
            //result.Tokens.Should().NotBeNull();
            //result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
            //result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Invalid_User()
        {
            var request = RequestLoginJsonBuilder.Build();

            var useCase = CreateUseCase();

            Func<Task> act = async () => { await useCase.Execute(request); };

            /* SHOULDLY */
            var exception = await act.ShouldThrowAsync<InvalidLoginException>();
            exception.Message.ShouldBe(ResourceMessagesExceptions.EMAIL_OR_PASSWORD_INVALID);

            /* FLUENT ASSERTIONS */
            //await act.Should().ThrowAsync<InvalidLoginException>()
            //    .WithMessage(ResourceMessagesExceptions.EMAIL_OR_PASSWORD_INVALID);
        }

        private static DoLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
        {
            var passwordEncripter = PasswordEcripterBuilder.Build();
            var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

            if (user is not null)
                userReadOnlyRepositoryBuilder.GetByEmailAndPassword(user);

            return new DoLoginUseCase(userReadOnlyRepositoryBuilder.Build(), passwordEncripter, accessTokenGenerator);
        }
    }
}
