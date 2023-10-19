using ChessConsole.Boardgame;
using ChessConsole.Chess;

namespace ChessConsole.Pieces;

internal sealed class King : ChessPiece
{
    private readonly ChessMatch chessMatch;
    public override string Symbol => Symbols.King;

    public King(Board board, Color color, ChessMatch chessMatch) : base(board, color)
    {
        this.chessMatch = chessMatch;
    }

    private bool TestRookCastling(Position postion)
    {
        ChessPiece? chessPiece = Board.Piece(postion) as ChessPiece;
        return chessPiece is Rook && chessPiece.Color.Equals(Color) && chessPiece.MoveCount == 0;
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

        // # special move castling
        if (MoveCount == 0 && !chessMatch.Check)
        {
            // to king-side rook
            Position postT1 = new(Position.Row, Position.Column + 3);
            if (TestRookCastling(postT1))
            {
                Position p1 = new(Position.Row, Position.Column + 1);
                Position p2 = new(Position.Row, Position.Column + 2);
                if (Board.Piece(p1) == null && Board.Piece(p2) == null)
                {
                    mat[Position.Row, Position.Column + 2] = true;
                }
            }

            // to queen-side rook
            Position postT2 = new(Position.Row, Position.Column - 4);
            if (TestRookCastling(postT2))
            {
                Position p1 = new(Position.Row, Position.Column - 1);
                Position p2 = new(Position.Row, Position.Column - 2);
                Position p3 = new(Position.Row, Position.Column - 3);
                if (Board.Piece(p1) == null && Board.Piece(p2) == null && Board.Piece(p3) == null)
                {
                    mat[Position.Row, Position.Column - 2] = true;
                }
            }
        }
        return mat;
    }

    private bool CanMove(Position position)
    {
        return Board.Piece(position) is not ChessPiece p || !p.Color.Equals(Color);
    }
}