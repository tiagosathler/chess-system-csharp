using ChessConsole.Boardgame;
using ChessConsole.Chess;

namespace ChessConsole.Pieces;

internal sealed class Rook : ChessPiece
{
    public Rook(Board board, Color color) : base(board, color)
    {
    }

    protected internal override bool[,] PossibleMoves()
    {
        bool[,] mat = new bool[Board.BOARD_SIZE, Board.BOARD_SIZE];

        Position p = new(0, 0);

        // above:
        p.SetValues(Position!.Row - 1, Position!.Column);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.Row--;
        }
        if (Board.PositionExists(p) && ThereIsOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // left:
        p.SetValues(Position!.Row, Position!.Column - 1);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.Column--;
        }
        if (Board.PositionExists(p) && ThereIsOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // left:
        p.SetValues(Position!.Row, Position!.Column + 1);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.Column++;
        }
        if (Board.PositionExists(p) && ThereIsOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // bellow:
        p.SetValues(Position.Row + 1, Position.Column);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.Row++;
        }
        if (Board.PositionExists(p) && ThereIsOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        return mat;
    }

    public override string ToString()
    {
        return "R";
    }
}