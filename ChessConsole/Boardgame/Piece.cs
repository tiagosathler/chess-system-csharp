namespace ChessConsole.Boardgame;

internal abstract class Piece
{
    protected Piece(Board board)
    {
        Board = board;
        Position = null;
    }

    public Board Board { get; }
    public Position? Position { get; set; }

    public bool IsThereAnyPossibleMove()
    {
        bool[,] possibleMovesMatrix = PossibleMoves();

        for (int i = 0; i < Board.BOARD_SIZE; i++)
        {
            for (int j = 0; j < Board.BOARD_SIZE; j++)
            {
                if (possibleMovesMatrix[i, j])
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool PossibleMove(Position position)
    {
        return PossibleMoves()[position.Row, position.Column];
    }

    protected internal abstract bool[,] PossibleMoves();
}