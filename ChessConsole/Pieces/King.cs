using ChessConsole.Boardgame;
using ChessConsole.Chess;

namespace ChessConsole.Pieces;

internal sealed class King : ChessPiece
{
    public King(Board board, Color color) : base(board, color)
    {
    }

    protected internal override bool[,] PossibleMoves()
    {
        bool[,] possibleMovesMatrix = new bool[Board.BOARD_SIZE, Board.BOARD_SIZE];
        return possibleMovesMatrix;
    }

    public override string ToString()
    {
        return "K";
    }
}