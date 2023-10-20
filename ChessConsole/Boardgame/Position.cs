namespace ChessConsole.Boardgame;

internal sealed class Position
{
    public Position()
    {
    }

    public Position(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public int Column { get; set; }
    public int Row { get; set; }

    public void SetValues(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public override string ToString()
    {
        return $"{Row}, {Column}";
    }
}