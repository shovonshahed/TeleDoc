namespace TeleDoc.DAL.Exceptions;

public class FailedException : Exception
{
    public FailedException(string? message) : base(message)
    {
    }

    public FailedException(string? action, string? message) : base($"{action} failed for {message}")
    {
    }
}