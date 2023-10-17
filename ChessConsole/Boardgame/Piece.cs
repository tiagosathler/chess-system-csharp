namespace ChessConsole.Boardgame;

internal abstract class Piece
{
    public Position? Position { get; set; }

    public Board Board { get; }

    protected Piece(Board board)
    {
        Board = board;
        Position = null;
    }

    protected abstract bool[,] PossibleMoves();

    public bool PossibleMove(Position position)
    {
        return PossibleMoves()[position.Row, position.Column];
    }

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
}