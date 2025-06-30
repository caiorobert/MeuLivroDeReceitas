using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.ChangePassword
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = password;

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.ShouldNotThrowAsync();

            var passwordEcripter = PasswordEcripterBuilder.Build();

            user.Password.ShouldBe(passwordEcripter.Encrypt(request.NewPassword));
        }

        [Fact]
        public async Task Error_NewPassword_Empty()
        {
            (var user, var password) = UserBuilder.Build();

            var request = new RequestChangePasswordJson
            {
                Password = password,
                NewPassword = string.Empty
            };

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

            exception.ShouldSatisfyAllConditions(
                ex => ex.GetErrorMessages().ShouldHaveSingleItem(),
                ex => ex.GetErrorMessages().ShouldContain(ResourceMessagesExceptions.PASSWORD_EMPTY)
            );

            var passwordEcripter = PasswordEcripterBuilder.Build();

            user.Password.ShouldBe(passwordEcripter.Encrypt(request.Password));
        }

        [Fact]
        public async Task Error_CurrentPassword_Different()
        {
            (var user, var password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

            exception.ShouldSatisfyAllConditions(
                ex => ex.GetErrorMessages().ShouldHaveSingleItem(),
                ex => ex.GetErrorMessages().ShouldContain(ResourceMessagesExceptions.PASSWORD_DIFFERENT_CURRENT_PASSWORD)
            );

            var passwordEcripter = PasswordEcripterBuilder.Build();

            user.Password.ShouldBe(passwordEcripter.Encrypt(password));
        }

        private static ChangePasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();
            var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var passwordEcripter = PasswordEcripterBuilder.Build();

            return new ChangePasswordUseCase
                (
                    loggedUser,
                    userUpdateRepository,
                    unitOfWork,
                    passwordEcripter
                );
        }
    }
}
