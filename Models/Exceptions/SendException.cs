namespace pgm3.Models.Exceptions;

public class SendException : Exception
{
    public SendException() : base($"Couldn't send the message")
    {
    }
}