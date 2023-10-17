using ChessConsole.Boardgame;

namespace ChessConsole.Chess;

internal abstract class ChessPiece : Piece
{
    public Color Color { get; }
    public int MoveCount { get; private set; }

    protected ChessPiece(Board board, Color color) : base(board)
    {
        Color = color;
    }

    protected bool IsThereOpponentPiece(Position position)
    {
        throw new NotImplementedException();
    }

    protected void IncreaseMoveCount()
    {
        MoveCount++;
    }

    protected void DecreaseMoveCount()
    {
        MoveCount--;
    }
}