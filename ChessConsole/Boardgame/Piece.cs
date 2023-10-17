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

    public virtual bool PossibleMove(Position position)
    {
        throw new NotImplementedException();
    }

    public virtual bool IsThereAnyPossibleMove()
    {
        throw new NotImplementedException();
    }
}