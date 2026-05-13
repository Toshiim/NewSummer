namespace Presentation.Bots.Abstractions;

public class CommandValidationException : Exception
{
    public CommandValidationException(string message) : base(message) { }
}