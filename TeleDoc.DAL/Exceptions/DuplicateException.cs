namespace TeleDoc.DAL.Exceptions;

public class DuplicateException : Exception
{
    public DuplicateException(string? message) : base($"user with {message} already exists")
    {
    }
}