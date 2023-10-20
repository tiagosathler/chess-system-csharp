using ChessConsole.Boardgame;

namespace ChessConsole.Chess;

internal sealed class ChessPosition
{
    private const char FIRST_CHAR = 'a';
    private const char LAST_CHAR = 'h';

    public ChessPosition(char column, int row)
    {
        if (char.ToLower(column) < FIRST_CHAR || char.ToLower(column) > LAST_CHAR || row < 1 || row > Board.BOARD_SIZE)
        {
            throw new ChessException("Error instantiating ChessPosition. Valid values are from " +
                $"{FIRST_CHAR}1 to {LAST_CHAR}{Board.BOARD_SIZE}");
        }
        Row = row;
        Column = char.ToLower(column);
    }

    public char Column { get; }
    public int Row { get; }

    public static ChessPosition FromPosition(Position position)
    {
        char column = (char)(FIRST_CHAR + position.Column);
        int row = Board.BOARD_SIZE - position.Row;
        return new ChessPosition(column, row);
    }

    public Position ToPosition()
    {
        int row = Board.BOARD_SIZE - Row;
        int column = Column - FIRST_CHAR;
        return new Position(row, column);
    }

    public override string ToString()
    {
        return $"{Column}{Row}";
    }
}