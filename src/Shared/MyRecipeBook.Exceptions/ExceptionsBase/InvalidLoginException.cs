namespace MyRecipeBook.Exceptions.ExceptionsBase
{
    public class InvalidLoginException : MyRecipeBookException
    {
        public InvalidLoginException() : base(ResourceMessagesExceptions.EMAIL_OR_PASSWORD_INVALID) { }
    }
}
