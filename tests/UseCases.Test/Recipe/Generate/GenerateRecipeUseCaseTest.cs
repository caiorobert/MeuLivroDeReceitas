using CommonTestUtilities.Dtos;
using CommonTestUtilities.OpenAI;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe.Generate;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using Xunit;

namespace UseCases.Test.Recipe.Generate;
public class GenerateRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var dto = GeneratedRecipeDtoBuilder.Build();

        var request = RequestGenerateRecipeJsonBuilder.Build();

        var useCase = CreateUseCase(dto);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(dto.Title);
        result.CookingTime.ShouldBe((MyRecipeBook.Communication.Enums.CookingTime)dto.CookingTime);
        result.Difficulty.ShouldBe(MyRecipeBook.Communication.Enums.Difficulty.Low);
    }

    [Fact]
    public async Task Error_Duplicated_Ingredients()
    {
        var dto = GeneratedRecipeDtoBuilder.Build();

        var request = RequestGenerateRecipeJsonBuilder.Build(count: 4);
        request.Ingredients.Add(request.Ingredients[0]);

        var useCase = CreateUseCase(dto);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));
        exception.ShouldNotBeNull();
        var errors = exception.GetErrorMessages();
        errors.Count.ShouldBe(1);
        errors.ShouldContain(ResourceMessagesExceptions.DUPLICATED_INGREDIENTS_IN_LIST);
    }

    private static GenerateRecipeUseCase CreateUseCase(GeneratedRecipeDto dto)
    {
        var generateRecipeAI = GenerateRecipeAIBuilder.Build(dto);

        return new GenerateRecipeUseCase(generateRecipeAI);
    }
}
