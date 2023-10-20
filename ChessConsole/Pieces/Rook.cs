using ChessConsole.Boardgame;
using ChessConsole.Chess;

namespace ChessConsole.Pieces;

internal sealed class Rook : ChessPiece
{
    public override string Symbol => Symbols.Rook;

    public Rook(Board board, Color color) : base(board, color)
    {
    }

    protected internal override bool[,] PossibleMoves()
    {
        bool[,] mat = new bool[Board.BOARD_SIZE, Board.BOARD_SIZE];

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

        // 🧭 W:
        p.SetValues(Position!.Row, Position!.Column - 1);
        while (Board.PositionExists(p) && !Board.IsThereAPiece(p))
        {
            mat[p.Row, p.Column] = true;
            p.Column--;
        }
        if (Board.PositionExists(p) && IsThereOpponentPiece(p))
        {
            mat[p.Row, p.Column] = true;
        }

        // 🧭 E:
        p.SetValues(Position!.Row, Position!.Column + 1);
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

        return mat;
    }
}