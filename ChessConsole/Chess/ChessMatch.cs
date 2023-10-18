using ChessConsole.Boardgame;
using ChessConsole.Pieces;

namespace ChessConsole.Chess;

internal sealed class ChessMatch

{
    public int Turn { get; private set; }
    public Color CurrentPlayer { get; private set; }
    public List<ChessPiece> CapturedChessPieces { get; }
    public List<ChessPiece> ChessPiecesOnTheBoard { get; }

    private readonly Board board;

    public ChessMatch()
    {
        board = new Board(Board.BOARD_SIZE, Board.BOARD_SIZE);
        CapturedChessPieces = new();
        ChessPiecesOnTheBoard = new();
        Turn = 1;
        CurrentPlayer = Color.WHITE;
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
        Position position = sourcePosition.ToPosition();
        ValidadeSourcePosition(position);
        return board.Piece(position)!.PossibleMoves();
    }

    public void PerformChessMove(ChessPosition sourcePosition, ChessPosition targetPosition)
    {
        Position source = sourcePosition.ToPosition();
        Position target = targetPosition.ToPosition();

        ValidadeSourcePosition(source);
        ValidadeTargetPosition(source, target);

        MakeMove(source, target);

        NextTurn();
    }

    public ChessPiece ReplacePromotedPiece(string type)
    {
        throw new NotImplementedException();
    }

    private void ValidadeSourcePosition(Position source)
    {
        if (!board.IsThereAPiece(source))
        {
            throw new ChessException($"There is no piece on the source position '{ChessPosition.FromPosition(source)}'!");
        }

        ChessPiece sourcePiece = (ChessPiece)board.Piece(source)!;

        if (!sourcePiece.Color.Equals(CurrentPlayer))
        {
            throw new ChessException("The chosen piece is not yours!");
        }

        if (!sourcePiece.IsThereAnyPossibleMove())
        {
            throw new ChessException("There is no possible moves for the chosen piece!");
        }
    }

    private void ValidadeTargetPosition(Position source, Position target)
    {
        if (!board.Piece(source)!.PossibleMove(target))
        {
            throw new ChessException("The chosen piece can't move to target position");
        }
    }

    private void MakeMove(Position source, Position target)
    {
        Piece sourcePiece = board.RemovePiece(source)!;
        Piece? capturedPiece = board.RemovePiece(target);

        board.PlacePiece(sourcePiece, target);

        if (capturedPiece != null)
        {
            CapturedChessPieces.Add((ChessPiece)capturedPiece);
            ChessPiecesOnTheBoard.Remove((ChessPiece)capturedPiece);
        }
    }

    private void NextTurn()
    {
        Turn++;
        CurrentPlayer = CurrentPlayer.Equals(Color.WHITE) ? Color.BLACK : Color.WHITE;
    }

    private void PlaceNewPiece(char column, int row, ChessPiece chessPiece)
    {
        board.PlacePiece(chessPiece, new ChessPosition(column, row).ToPosition());
        ChessPiecesOnTheBoard.Add(chessPiece);
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
}