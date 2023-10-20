using ChessConsole.Boardgame;
using ChessConsole.Chess;

namespace ChessConsole.Pieces;

internal sealed class Knight : ChessPiece
{
    public Knight(Board board, Color color) : base(board, color)
    {
    }

    public override string Symbol => Symbols.Knight;

    protected internal override bool[,] PossibleMoves()
    {
        bool[,] mat = new bool[Board.Rows, Board.Columns];

        Position p = new(0, 0);

        /* r
		 * ^
		 *
		 * 4   * N *
		 * 3 *       *
		 * 2 W   x   E
		 * 1 *       *
		 * 0   * S *
		 *   0 1 2 3 4  > c
		 */

        // clockwise movements
        // 🧭 🔃 N -> E
        p.SetValues(Position!.Row + 2, Position!.Column + 1);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 🔃 E -> S
        p.SetValues(Position.Row - 1, Position.Column + 2);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 🔃 S -> W
        p.SetValues(Position.Row - 2, Position.Column - 1);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 🔃 W -> N
        p.SetValues(Position.Row + 1, Position.Column - 2);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // anti-clockwise movements
        // 🧭 🔄 N -> W
        p.SetValues(Position.Row + 2, Position.Column - 1);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 🔄 W -> S
        p.SetValues(Position.Row - 1, Position.Column - 2);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 🔄 S -> E
        p.SetValues(Position.Row - 2, Position.Column + 1);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 🔄 E -> N
        p.SetValues(Position.Row + 1, Position.Column + 2);
        if (Board.PositionExists(p) && CanMove(p))
        {
            mat[p.Row, p.Column] = true;
        }

        return mat;
    }

    private bool CanMove(Position position)
    {
        return Board.Piece(position) is not ChessPiece chessPiece || chessPiece.Color != Color;
    }
}