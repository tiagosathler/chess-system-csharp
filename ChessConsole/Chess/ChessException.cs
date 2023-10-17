namespace ChessConsole.Chess;

public class ChessException : ApplicationException
{
    public ChessException()
    {
    }

    public ChessException(string? message) : base(message)
    {
    }

    public ChessException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}