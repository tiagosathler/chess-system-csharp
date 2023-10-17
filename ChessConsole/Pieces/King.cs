using ChessConsole.Boardgame;
using ChessConsole.Chess;

namespace ChessConsole.Pieces;

internal sealed class King : ChessPiece
{
    public King(Board board, Color color) : base(board, color)
    {
    }

    protected override bool[,] PossibleMoves()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return "K";
    }
}