using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.User.Update
{
    public class UpdateUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Error_Name_Empty()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                er => er.ShouldHaveSingleItem(),
                er => er.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.NAME_EMPTY))
                );
        }

        [Fact]
        public void Error_Email_Empty()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = string.Empty;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                er => er.ShouldHaveSingleItem(),
                er => er.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.EMAIL_EMPTY))
                );
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var validator = new UpdateUserValidator();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Email = "email.com";

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                er => er.ShouldHaveSingleItem(),
                er => er.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.EMAIL_INVALID))
                );
        }
    }
}
