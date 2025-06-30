using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.Recipe
{
    public class RecipeValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Error_Invalid_Coocking_Time()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = (MyRecipeBook.Communication.Enums.CookingTime?)1000;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                r => r.ShouldHaveSingleItem(),
                r => r.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.COOKING_TIME_NOT_SUPPORTED))
                );
        }

        [Fact]
        public void Error_Invalid_Difficulty()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Difficulty = (MyRecipeBook.Communication.Enums.Difficulty?)1000;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                r => r.ShouldHaveSingleItem(),
                r => r.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.DIFFICULTY_LEVEL_NOT_SUPPORTED))
                );
        }

        [Theory]
        [InlineData(null)]
        [InlineData("       ")]
        [InlineData("")]
        public void Error_Empty_Title(string title)
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = title;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                r => r.ShouldHaveSingleItem(),
                r => r.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.RECIPE_TITLE_EMPTY))
                );
        }

        [Fact]
        public void Success_Coocking_Time_Null()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = null;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Success_Difficulty_Null()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Difficulty = null;

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Success_DishTypes_Empty()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Clear();

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
        }
        
        [Fact]
        public void Error_Invalid_DishTypes()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Add((DishType)1000);

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                r => r.ShouldHaveSingleItem(),
                r => r.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.DISH_TYPE_NOT_SUPPORTED))
                );
        }
        
        [Fact]
        public void Error_Empty_Ingredients()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients.Clear();

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                r => r.ShouldHaveSingleItem(),
                r => r.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.AT_LEAST_ONE_INGREDIENT))
                );
        }
        
        [Fact]
        public void Error_Empty_Instructions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.Clear();

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                r => r.ShouldHaveSingleItem(),
                r => r.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.AT_LEAST_ONE_INSTRUCTION))
                );
        }
        
        [Theory]
        [InlineData("    ")]
        [InlineData("")]
        [InlineData(null)]
        public void Error_Empty_Value_Ingredients(string ingredient)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients.Add(ingredient);

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                r => r.ShouldHaveSingleItem(),
                r => r.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.INGREDIENT_EMPTY))
                );
        }
        
        [Fact]
        public void Error_Same_Step_Instructions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = request.Instructions.Last().Step;

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                r => r.ShouldHaveSingleItem(),
                r => r.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER))
                );
        }
        
        [Fact]
        public void Error_Negative_Step_Instructions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = -1;

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                r => r.ShouldHaveSingleItem(),
                r => r.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.NON_NEGATIVE_INSTRUCTION_STEP))
                );
        }
        
        [Theory]
        [InlineData("   ")]
        [InlineData("")]
        public void Error_Empty_Value_Instructions(string instruction)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Text = instruction;

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                r => r.ShouldHaveSingleItem(),
                r => r.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesExceptions.INSTRUCTION_EMPTY))
                );
        }
    }
}
