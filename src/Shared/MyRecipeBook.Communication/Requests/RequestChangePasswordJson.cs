namespace MyRecipeBook.Communication.Requests
{
    public class RequestChangePasswordJson
    {
        public string Passord { get; set; } = string.Empty;
        public string NewPassord { get; set; } = string.Empty;
    }
}
