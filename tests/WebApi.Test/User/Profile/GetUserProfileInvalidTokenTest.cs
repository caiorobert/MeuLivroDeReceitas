using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Profile
{
    public class GetUserProfileInvalidTokenTest : MyRecipeBookClassFixture
    {
        private readonly string METHOD = "user";

        public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var response = await DoGet(METHOD, token: "tokenInvalid");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Token()
        {
            var response = await DoGet(METHOD, token: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet(METHOD, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Test_Messages_Resources(string culture)
        {
            var response = await DoGet(METHOD, token: "tokenInvalid", culture);

            /* SHOULDLY */
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var erros = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesExceptions.ResourceManager.GetString("USER_WITHOUT_PERMISSION_ACCESS_RESOURCE", new CultureInfo(culture));

            /* SHOULDLY */
            erros.ShouldSatisfyAllConditions(
                er => er.ShouldHaveSingleItem(),
                er => er.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
                );
        }
    }
}
