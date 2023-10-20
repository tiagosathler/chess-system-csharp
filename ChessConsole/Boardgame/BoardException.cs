using ChessConsole.Chess;

namespace ChessConsole.Boardgame;

public class BoardException : ChessException
{
    public BoardException()
    {
    }

    public BoardException(string? message) : base(message)
    {
    }

    public BoardException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}