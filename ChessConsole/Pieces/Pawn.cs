using ChessConsole.Boardgame;
using ChessConsole.Chess;

namespace ChessConsole.Pieces;

internal class Pawn : ChessPiece
{
    public override string Symbol => Symbols.Pawn;

    private readonly ChessMatch chessMatch;

    public Pawn(Board board, Color color, ChessMatch chessMatch) : base(board, color)
    {
        this.chessMatch = chessMatch;
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

            // special move en passant
            if (Position.Row == 3)
            {
                Position left = new(Position.Row, Position.Column - 1);
                if (Board.PositionExists(left) && IsThereOpponentPiece(left)
                        && Board.Piece(left) == chessMatch.EnPassantVulnerable)
                {
                    mat[left.Row - 1, left.Column] = true;
                }

                Position right = new(Position.Row, Position.Column + 1);
                if (Board.PositionExists(right) && IsThereOpponentPiece(right)
                        && Board.Piece(right) == chessMatch.EnPassantVulnerable)
                {
                    mat[right.Row - 1, right.Column] = true;
                }
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

            // special move en passant
            if (Position.Row == 4)
            {
                Position left = new(Position.Row, Position.Column - 1);
                if (Board.PositionExists(left) && IsThereOpponentPiece(left)
                        && Board.Piece(left) == chessMatch.EnPassantVulnerable)
                {
                    mat[left.Row + 1, left.Column] = true;
                }

                Position right = new(Position.Row, Position.Column + 1);
                if (Board.PositionExists(right) && IsThereOpponentPiece(right)
                        && Board.Piece(right) == chessMatch.EnPassantVulnerable)
                {
                    mat[right.Row + 1, right.Column] = true;
                }
            }
        }

        return mat;
    }
}