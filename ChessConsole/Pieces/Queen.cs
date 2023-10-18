using ChessConsole.Boardgame;
using ChessConsole.Chess;

namespace ChessConsole.Pieces;

internal class Queen : ChessPiece
{
    public Queen(Board board, Color color) : base(board, color)
    {
    }

    protected internal override bool[,] PossibleMoves()
    {
        bool[,] mat = new bool[Board.Rows, Board.Columns];

        Position p = new(0, 0);

        // 🧭 N:
        p.SetValues(Position!.Row - 1, Position!.Column);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.Row--;
        }
        if (Board.PositionExists(p) && IsThereOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 E:
        p.SetValues(Position.Row, Position.Column - 1);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.Column--;
        }
        if (Board.PositionExists(p) && IsThereOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 W:
        p.SetValues(Position.Row, Position.Column + 1);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.Column++;
        }
        if (Board.PositionExists(p) && IsThereOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 S:
        p.SetValues(Position.Row + 1, Position.Column);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.Row++;
        }
        if (Board.PositionExists(p) && IsThereOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 NW:
        p.SetValues(Position.Row - 1, Position.Column - 1);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.SetValues(p.Row - 1, p.Column - 1);
        }
        if (Board.PositionExists(p) && IsThereOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 NE:
        p.SetValues(Position.Row - 1, Position.Column + 1);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.SetValues(p.Row - 1, p.Column + 1);
        }
        if (Board.PositionExists(p) && IsThereOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 SW:
        p.SetValues(Position.Row + 1, Position.Column + 1);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.SetValues(p.Row + 1, p.Column + 1);
        }
        if (Board.PositionExists(p) && IsThereOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 SE:
        p.SetValues(Position.Row + 1, Position.Column - 1);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.SetValues(p.Row + 1, p.Column - 1);
        }
        if (Board.PositionExists(p) && IsThereOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        return mat;
    }

    public override string ToString()
    {
        return "Q";
    }
}