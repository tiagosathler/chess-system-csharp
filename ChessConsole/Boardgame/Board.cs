namespace ChessConsole.Boardgame;

internal sealed class Board
{
    public static readonly int BOARD_SIZE = 8;

    public int Rows { get; }
    public int Columns { get; }

    private readonly Piece?[,] pieces;

    public Board(int rows, int columns)
    {
        if (rows != BOARD_SIZE || columns != BOARD_SIZE)
        {
            throw new BoardException($"Error creating board: it must be {BOARD_SIZE} rows by {BOARD_SIZE} columns!");
        }
        Rows = rows;
        Columns = columns;
        pieces = new Piece[Rows, Columns];
    }

    public Piece? Piece(Position position)
    {
        if (!PositionExists(position))
        {
            throw new BoardException("Position not on the board");
        }
        return pieces[position.Row, position.Column];
    }

    public Piece? Piece(int row, int column)
    {
        return Piece(new Position(row, column));
    }

    public void PlacePiece(Piece piece, Position position)
    {
        if (IsThereAPiece(position))
        {
            throw new BoardException($"There is already a piece on position {position}");
        }
        pieces[position.Row, position.Column] = piece;
        piece.Position = position;
    }

    public Piece? RemovePiece(Position position)
    {
        throw new NotImplementedException();
    }

    public bool PositionExists(int row, int column)
    {
        return row >= 0
            && row < Rows
            && column >= 0
            && column <= Columns;
    }

    public bool PositionExists(Position position)
    {
        return PositionExists(position.Row, position.Column);
    }

    public bool IsThereAPiece(Position position)
    {
        return Piece(position) != null;
    }
}