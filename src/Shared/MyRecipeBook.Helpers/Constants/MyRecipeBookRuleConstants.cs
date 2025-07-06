namespace MyRecipeBook.Helpers.Constants
{
    public static class MyRecipeBookRuleConstants
    {
        public const string BEARER = "Bearer";
        public const short PASSWORD_LENGTH = 6;
        public const int MAXIMUM_INGREDIENTS_GENERATE_RECIPE = 5;
        public const int MAXIMUM_IMAGE_URL_LIFETIME_IN_MINUTES = 10;
        public const int REFRESH_TOKEN_EXPIRATION_DAYS = 7;
        public const string CHAT_MODEL = "gpt-4o";
        public const int QUANTITY_RECIPES_RETURNED_TO_DASHBOARD = 5;
        public const string RECIPE_TABLE_NAME = "Recipes";
        public const string USER_TABLE_NAME = "Users";
    }
}
