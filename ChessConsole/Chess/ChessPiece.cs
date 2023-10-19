using ChessConsole.Boardgame;

namespace ChessConsole.Chess;

internal abstract class ChessPiece : Piece
{
    public Color Color { get; }
    public int MoveCount { get; private set; }
    public abstract string Symbol { get; }

    protected ChessPiece(Board board, Color color) : base(board)
    {
        Color = color;
    }

    protected internal bool IsThereOpponentPiece(Position position)
    {
        return Board.Piece(position) is ChessPiece otherPiece && !otherPiece.Color.Equals(Color);
    }

    protected internal void IncreaseMoveCount()
    {
        MoveCount++;
    }

    protected internal void DecreaseMoveCount()
    {
        MoveCount--;
    }

    public override sealed string ToString()
    {
        return Symbol;
    }
}