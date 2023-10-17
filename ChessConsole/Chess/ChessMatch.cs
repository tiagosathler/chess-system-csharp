using ChessConsole.Boardgame;
using ChessConsole.Pieces;

namespace ChessConsole.Chess;

internal sealed class ChessMatch

{
    public int Turn { get; set; }
    public Color CurrentPlayer { get; set; }

    private readonly Board board;

    public ChessMatch()
    {
        board = new Board(Board.BOARD_SIZE, Board.BOARD_SIZE);
        InitialSetup();
    }

    public ChessPiece[,] GetPieces()
    {
        ChessPiece[,] chessPieces = new ChessPiece[board.Rows, board.Columns];

        for (int i = 0; i < Board.BOARD_SIZE; i++)
        {
            for (int j = 0; j < Board.BOARD_SIZE; j++)
            {
                chessPieces[i, j] = (ChessPiece)board.Piece(i, j)!;
            }
        }

        return chessPieces;
    }

    public bool[,] PossibleMoves(ChessPosition sourcePosition)
    {
        throw new NotImplementedException();
    }

    public ChessPiece? PerformChessMove(ChessPosition sourcePosition, ChessPosition targetPosition)
    {
        Position source = sourcePosition.ToPosition();
        Position target = targetPosition.ToPosition();

        ValidateSourcePosition(source);

        Piece? capturedPiece = MakeMove(source, target);

        return capturedPiece as ChessPiece;
    }

    public ChessPiece ReplacePromotedPiece(string type)
    {
        throw new NotImplementedException();
    }

    private void ValidateSourcePosition(Position position)
    {
        if (!board.IsThereAPiece(position))
        {
            throw new ChessException($"There is no piece on the source position '{ChessPosition.FromPosition(position)}'!");
        }
        if (!board.Piece(position)!.IsThereAnyPossibleMove())
        {
            throw new ChessException("There is no possible moves for the chosen piece!");
        }
    }

    private Piece? MakeMove(Position source, Position target)
    {
        Piece sourcePiece = board.RemovePiece(source)!;
        Piece? capturedPiece = board.RemovePiece(target);

        board.PlacePiece(sourcePiece, target);

        return capturedPiece;
    }

    private void InitialSetup()
    {
        PlaceNewPiece('a', 1, new Rook(board, Color.WHITE));
        PlaceNewPiece('h', 1, new Rook(board, Color.WHITE));
        PlaceNewPiece('e', 1, new King(board, Color.WHITE));

        PlaceNewPiece('a', 8, new Rook(board, Color.BLACK));
        PlaceNewPiece('h', 8, new Rook(board, Color.BLACK));
        PlaceNewPiece('e', 8, new King(board, Color.BLACK));
    }

    private void PlaceNewPiece(char column, int row, ChessPiece chessPiece)
    {
        board.PlacePiece(chessPiece, new ChessPosition(column, row).ToPosition());
    }
}