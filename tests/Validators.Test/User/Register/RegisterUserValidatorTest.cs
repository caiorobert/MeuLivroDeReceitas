using CommonTestUtilities.Requests;
//using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.User.Register
{
    public class RegisterUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();

            var result = validator.Validate(request);

            /* SHOULDLY */
            result.IsValid.ShouldBeTrue();

            /* FLUENT ASSERTIONS */
            //result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Name_Empty()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var result = validator.Validate(request);

            /* SHOULDLY */
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                er => er.ShouldHaveSingleItem(),
                er => er.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.NAME_EMPTY))
                );

            /* FLUENT ASSERTIONS */
            //result.IsValid.Should().BeFalse();
            //result.Errors.Should().ContainSingle()
            //    .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.NAME_EMPTY));
        }

        [Fact]
        public void Error_Email_Empty()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = string.Empty;

            var result = validator.Validate(request);

            /* SHOULDLY */
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                er => er.ShouldHaveSingleItem(),
                er => er.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.EMAIL_EMPTY))
                );

            /* FLUENT ASSERTIONS */
            //result.IsValid.Should().BeFalse();
            //result.Errors.Should().ContainSingle()
            //    .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.EMAIL_EMPTY));
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = "email.com";

            var result = validator.Validate(request);

            /* SHOULDLY */
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                er => er.ShouldHaveSingleItem(),
                er => er.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.EMAIL_INVALID))
                );

            /* FLUENT ASSERTIONS */
            //result.IsValid.Should().BeFalse();
            //result.Errors.Should().ContainSingle()
            //    .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.EMAIL_INVALID));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Error_Password_Invalid(int passwordLenght)
        {
            var validator = new RegisterUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build(passwordLenght);

            var result = validator.Validate(request);

            /* SHOULDLY */
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                er => er.ShouldHaveSingleItem(),
                er => er.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.PASSWORD_EMPTY))
                );

            /* FLUENT ASSERTIONS */
            //result.IsValid.Should().BeFalse();
            //result.Errors.Should().ContainSingle()
            //    .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.PASSWORD_EMPTY));
        }
    }
}
