using ChessConsole.Boardgame;

namespace ChessConsole.Chess;

internal abstract class ChessPiece : Piece
{
    protected ChessPiece(Board board, Color color) : base(board)
    {
        Color = color;
    }

    public Color Color { get; }
    public int MoveCount { get; private set; }
    public abstract string Symbol { get; }

    public override sealed string ToString()
    {
        return Symbol;
    }

    protected internal void DecreaseMoveCount()
    {
        MoveCount--;
    }

    protected internal void IncreaseMoveCount()
    {
        MoveCount++;
    }

    protected internal bool IsThereOpponentPiece(Position position)
    {
        return Board.Piece(position) is ChessPiece otherPiece && !otherPiece.Color.Equals(Color);
    }
}