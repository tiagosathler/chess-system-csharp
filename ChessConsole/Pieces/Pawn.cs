using ChessConsole.Boardgame;
using ChessConsole.Chess;

namespace ChessConsole.Pieces;

internal class Pawn : ChessPiece
{
    public Pawn(Board board, Color color) : base(board, color)
    {
    }

    protected internal override bool[,] PossibleMoves()
    {
        bool[,] mat = new bool[Board.BOARD_SIZE, Board.BOARD_SIZE];

        Position p = new(0, 0);

        // white
        if (Color == Color.WHITE)
        {
            // ahead
            p.SetValues(Position!.Row - 1, Position!.Column);
            if (Board.PositionExists(p) && !Board.IsThereAPiece(p))
            {
                mat[p.Row, p.Column] = true;
                // first movement
                if (MoveCount == 0)
                {
                    p.SetValues(Position.Row - 2, Position.Column);
                    Position p2 = new(Position.Row - 2, Position.Column);
                    if (Board.PositionExists(p2) && !Board.IsThereAPiece(p2))
                    {
                        mat[p2.Row, p2.Column] = true;
                    }
                }
            }

            // diagonals
            p.SetValues(Position.Row - 1, Position.Column - 1);
            if (Board.PositionExists(p) && IsThereOpponentPiece(p))
            {
                mat[p.Row, p.Column] = true;
            }
            p.SetValues(Position.Row! - 1, Position.Column! + 1);
            if (Board.PositionExists(p) && IsThereOpponentPiece(p))
            {
                mat[p.Row, p.Column] = true;
            }
        }

        // black
        else
        {
            p.SetValues(Position!.Row + 1, Position!.Column);
            if (Board.PositionExists(p) && !Board.IsThereAPiece(p))
            {
                mat[p.Row, p.Column] = true;
                // first movement
                if (MoveCount == 0)
                {
                    p.SetValues(Position.Row + 2, Position.Column);
                    Position p2 = new(Position.Row + 2, Position.Column);
                    if (Board.PositionExists(p2) && !Board.IsThereAPiece(p2))
                    {
                        mat[p2.Row, p2.Column] = true;
                    }
                }
            }

            // diagonals
            p.SetValues(Position.Row + 1, Position.Column - 1);
            if (Board.PositionExists(p) && IsThereOpponentPiece(p))
            {
                mat[p.Row, p.Column] = true;
            }
            p.SetValues(Position.Row + 1, Position.Column + 1);
            if (Board.PositionExists(p) && IsThereOpponentPiece(p))
            {
                mat[p.Row, p.Column] = true;
            }
        }

        return mat;
    }

    public override string ToString()
    {
        return "P";
    }
}