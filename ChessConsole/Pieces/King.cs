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
        bool[,] mat = new bool[Board.BOARD_SIZE, Board.BOARD_SIZE];

        Position p = new(0, 0);

        // 🧭 N:
        p.SetValues(Position!.Row - 1, Position!.Column);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 S:
        p.SetValues(Position!.Row + 1, Position!.Column);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 E:
        p.SetValues(Position!.Row, Position!.Column - 1);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 W:
        p.SetValues(Position!.Row, Position!.Column + 1);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 NW:
        p.SetValues(Position!.Row - 1, Position!.Column - 1);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 NE:
        p.SetValues(Position!.Row - 1, Position!.Column + 1);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 SW:
        p.SetValues(Position!.Row + 1, Position!.Column - 1);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 SE:
        p.SetValues(Position!.Row + 1, Position!.Column + 1);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        return mat;
    }

    private bool CanMove(Position position)
    {
        return Board.Piece(position) is not ChessPiece p || !p.Color.Equals(Color);
    }

    public override string ToString()
    {
        return "K";
    }
}