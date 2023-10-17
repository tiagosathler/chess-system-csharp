namespace ChessConsole.Boardgame;

internal sealed class Board
{
    public int Rows { get; set; }
    public int Columns { get; set; }

    private readonly Piece[,] pieces;

    public Board(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        pieces = new Piece[Rows, Columns];
    }

    public Piece Piece(int row, int column)
    {
        return pieces[row, column];
    }

    public Piece Piece(Position position)
    {
        return pieces[position.Row, position.Column];
    }

    public void PlacePiece(Piece piece, Position position)
    {
        pieces[position.Row, position.Column] = piece;
    }

    public Piece RemovePiece(Position position)
    {
        throw new NotImplementedException();
    }

    public bool PositionExists(Position position)
    {
        throw new NotImplementedException();
    }

    public bool ThereIsAPiece(Position position)
    {
        throw new NotImplementedException();
    }
}