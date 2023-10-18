using ChessConsole.Boardgame;
using ChessConsole.Pieces;

namespace ChessConsole.Chess;

internal sealed class ChessMatch

{
    public int Turn { get; private set; }
    public Color CurrentPlayer { get; private set; }
    public bool Check { get; private set; }
    public bool CheckMate { get; private set; }
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

        if (!CheckMate)
        {
            NextTurn();
        }
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
        ChessPiece sourcePiece = (ChessPiece)board.Piece(source)!;

        if (!sourcePiece.PossibleMove(target))
        {
            throw new ChessException("The chosen piece can't move to target position");
        }
        if (sourcePiece is King && TestCheck(CurrentPlayer, target))
        {
            throw new ChessException("You can't put yourself in check!");
        }
    }

    private void MakeMove(Position source, Position target)
    {
        ChessPiece sourcePiece = (ChessPiece)board.RemovePiece(source)!;
        Piece? capturedPiece = board.RemovePiece(target);

        board.PlacePiece(sourcePiece, target);
        sourcePiece.IncreaseMoveCount();

        if (capturedPiece != null)
        {
            CapturedChessPieces.Add((ChessPiece)capturedPiece);
            ChessPiecesOnTheBoard.Remove((ChessPiece)capturedPiece);
        }

        if (Check)
        {
            CheckMate = TestCheckMate(CurrentOpponentPlayer());
        }

        Check = TestCheck(CurrentOpponentPlayer());
    }

    private void NextTurn()
    {
        Turn++;
        CurrentPlayer = CurrentPlayer.Equals(Color.WHITE) ? Color.BLACK : Color.WHITE;
    }

    private bool TestCheck(Color color, Position position)
    {
        foreach (ChessPiece p in ChessPiecesOnTheBoard.FindAll(p => !p.Color.Equals(color)))
        {
            bool[,] mat = p.PossibleMoves();

            if (mat[position.Row, position.Column])
            {
                return true;
            }
        }
        return false;
    }

    private bool TestCheck(Color color)
    {
        ChessPiece? king = ChessPiecesOnTheBoard.Find(p => p is King && p.Color.Equals(color))
            ?? throw new ChessException($"There is not {CurrentPlayer} king on the board!");

        return TestCheck(color, king.Position!);
    }

    private bool TestCheckMate(Color color)
    {
        foreach (ChessPiece chessPiece in ChessPiecesOnTheBoard.FindAll(p => p.Color.Equals(color)))
        {
            bool[,] possibleMoves = chessPiece.PossibleMoves();

            for (int i = 0; i < Board.BOARD_SIZE; i++)
            {
                for (int j = 0; j < Board.BOARD_SIZE; j++)
                {
                    if (possibleMoves[i, j])
                    {
                        Position target = new(i, j);
                        ChessPiece? foundChessPiece = board.Piece(target) as ChessPiece;

                        if (foundChessPiece is King && !foundChessPiece.Color.Equals(color))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private Color CurrentOpponentPlayer()
    {
        return CurrentPlayer.Equals(Color.WHITE) ? Color.BLACK : Color.WHITE;
    }

    public ChessPiece ReplacePromotedPiece(string type)
    {
        throw new NotImplementedException();
    }

    private void PlaceNewPiece(char column, int row, ChessPiece chessPiece)
    {
        board.PlacePiece(chessPiece, new ChessPosition(column, row).ToPosition());
        ChessPiecesOnTheBoard.Add(chessPiece);
    }

    private void InitialSetup()
    {
        PlaceNewPiece('a', 1, new Rook(board, Color.WHITE));
        PlaceNewPiece('b', 1, new Knight(board, Color.WHITE));
        PlaceNewPiece('c', 1, new Bishop(board, Color.WHITE));
        PlaceNewPiece('e', 1, new King(board, Color.WHITE));
        PlaceNewPiece('f', 1, new Bishop(board, Color.WHITE));
        PlaceNewPiece('g', 1, new Knight(board, Color.WHITE));
        PlaceNewPiece('h', 1, new Rook(board, Color.WHITE));
        PlaceNewPiece('a', 2, new Pawn(board, Color.WHITE));
        PlaceNewPiece('b', 2, new Pawn(board, Color.WHITE));
        PlaceNewPiece('c', 2, new Pawn(board, Color.WHITE));
        PlaceNewPiece('d', 2, new Pawn(board, Color.WHITE));
        PlaceNewPiece('e', 2, new Pawn(board, Color.WHITE));
        PlaceNewPiece('f', 2, new Pawn(board, Color.WHITE));
        PlaceNewPiece('g', 2, new Pawn(board, Color.WHITE));
        PlaceNewPiece('h', 2, new Pawn(board, Color.WHITE));

        PlaceNewPiece('a', 8, new Rook(board, Color.BLACK));
        PlaceNewPiece('b', 8, new Knight(board, Color.BLACK));
        PlaceNewPiece('c', 8, new Bishop(board, Color.BLACK));
        PlaceNewPiece('e', 8, new King(board, Color.BLACK));
        PlaceNewPiece('f', 8, new Bishop(board, Color.BLACK));
        PlaceNewPiece('g', 8, new Knight(board, Color.BLACK));
        PlaceNewPiece('h', 8, new Rook(board, Color.BLACK));
        PlaceNewPiece('a', 7, new Pawn(board, Color.BLACK));
        PlaceNewPiece('b', 7, new Pawn(board, Color.BLACK));
        PlaceNewPiece('c', 7, new Pawn(board, Color.BLACK));
        PlaceNewPiece('d', 7, new Pawn(board, Color.BLACK));
        PlaceNewPiece('e', 7, new Pawn(board, Color.BLACK));
        PlaceNewPiece('f', 7, new Pawn(board, Color.BLACK));
        PlaceNewPiece('g', 7, new Pawn(board, Color.BLACK));
        PlaceNewPiece('h', 7, new Pawn(board, Color.BLACK));
    }
}