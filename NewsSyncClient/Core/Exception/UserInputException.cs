namespace NewsSyncClient.Core.Exceptions;

public class UserInputException : Exception
{
    public UserInputException(string message) : base(message) { }
}
