using ChessConsole.Boardgame;
using ChessConsole.Pieces;

namespace ChessConsole.Chess;

internal sealed class ChessMatch

{
    public int Turn { get; private set; }
    public Color CurrentPlayer { get; private set; }
    public bool Check { get; private set; }
    public bool CheckMate { get; private set; }
    public ChessPiece? EnPassantVulnerable { get; private set; }
    public ChessPiece? Promoted { get; private set; }
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

    public ChessPiece ReplacePromotedPiece(string type)
    {
        if (Promoted == null)
        {
            throw new ChessException("There is no piece to be promoted!");
        }

        if (!type.Equals("B") && !type.Equals("N") && !type.Equals("R") && !type.Equals("Q"))
        {
            return Promoted;
        }

        Position position = Promoted.Position!;
        ChessPiece removedPiece = (ChessPiece)board.RemovePiece(position)!;
        ChessPiecesOnTheBoard.Remove(removedPiece);

        ChessPiece newPiece = NewPiece(type, Promoted.Color);
        board.PlacePiece(newPiece, position);
        ChessPiecesOnTheBoard.Add(newPiece);

        return newPiece;
    }

    private ChessPiece NewPiece(string type, Color color)
    {
        return type switch
        {
            "B" => new Bishop(board, color),
            "N" => new Knight(board, color),
            "Q" => new Queen(board, color),
            _ => new Rook(board, color),
        };
    }

    private void MakeMove(Position source, Position target)
    {
        ChessPiece sourcePiece = (ChessPiece)board.RemovePiece(source)!;
        ChessPiece? capturedPiece = board.RemovePiece(target) as ChessPiece;

        board.PlacePiece(sourcePiece, target);
        sourcePiece.IncreaseMoveCount();

        // special move: castling to king-side rook
        if (sourcePiece is King && target.Column == source.Column + 2)
        {
            Position sourceT = new(source.Row, source.Column + 3);
            Position targetT = new(source.Row, source.Column + 1);
            ChessPiece rook = (ChessPiece)board.RemovePiece(sourceT)!;
            board.PlacePiece(rook, targetT);
            rook.IncreaseMoveCount();
        }

        // special move: castling to queen-side rook
        if (sourcePiece is King && target.Column == source.Column - 2)
        {
            Position sourceT = new(source.Row, source.Column - 4);
            Position targetT = new(source.Row, source.Column - 1);
            ChessPiece rook = (ChessPiece)board.RemovePiece(sourceT)!;
            board.PlacePiece(rook, targetT);
            rook.IncreaseMoveCount();
        }

        // special move en passant - capture opponent piece
        if (sourcePiece is Pawn && source.Column != target.Column && capturedPiece == null)
        {
            Position pawnPosition = sourcePiece.Color.Equals(Color.WHITE)
                ? new Position(target.Row + 1, target.Column)
                : new Position(target.Row - 1, target.Column);

            capturedPiece = board.RemovePiece(pawnPosition) as ChessPiece;
        }

        // special move en passant - set EnPassantVulnerable
        ChessPiece movedPiece = (ChessPiece)board.Piece(target)!;
        EnPassantVulnerable = (movedPiece is Pawn && Math.Abs(source.Row - target.Row) == 2) ? movedPiece : null;

        // special move promotion
        Promoted = null;
        if (movedPiece is Pawn &&
            (
                (movedPiece.Color.Equals(Color.WHITE) && target.Row == 0) ||
                (movedPiece.Color.Equals(Color.BLACK) && target.Row == 7)
            ))
        {
            Promoted = (ChessPiece)board.Piece(target)!;
            Promoted = ReplacePromotedPiece("Q");
        }

        if (capturedPiece != null)
        {
            CapturedChessPieces.Add(capturedPiece);
            ChessPiecesOnTheBoard.Remove(capturedPiece);
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
        PlaceNewPiece('d', 1, new Queen(board, Color.WHITE));
        PlaceNewPiece('e', 1, new King(board, Color.WHITE, this));
        PlaceNewPiece('f', 1, new Bishop(board, Color.WHITE));
        PlaceNewPiece('g', 1, new Knight(board, Color.WHITE));
        PlaceNewPiece('h', 1, new Rook(board, Color.WHITE));
        PlaceNewPiece('a', 2, new Pawn(board, Color.WHITE, this));
        PlaceNewPiece('b', 2, new Pawn(board, Color.WHITE, this));
        PlaceNewPiece('c', 2, new Pawn(board, Color.WHITE, this));
        PlaceNewPiece('d', 2, new Pawn(board, Color.WHITE, this));
        PlaceNewPiece('e', 2, new Pawn(board, Color.WHITE, this));
        PlaceNewPiece('f', 2, new Pawn(board, Color.WHITE, this));
        PlaceNewPiece('g', 2, new Pawn(board, Color.WHITE, this));
        PlaceNewPiece('h', 2, new Pawn(board, Color.WHITE, this));

        PlaceNewPiece('a', 8, new Rook(board, Color.BLACK));
        PlaceNewPiece('b', 8, new Knight(board, Color.BLACK));
        PlaceNewPiece('c', 8, new Bishop(board, Color.BLACK));
        PlaceNewPiece('d', 8, new Queen(board, Color.BLACK));
        PlaceNewPiece('e', 8, new King(board, Color.BLACK, this));
        PlaceNewPiece('f', 8, new Bishop(board, Color.BLACK));
        PlaceNewPiece('g', 8, new Knight(board, Color.BLACK));
        PlaceNewPiece('h', 8, new Rook(board, Color.BLACK));
        PlaceNewPiece('a', 7, new Pawn(board, Color.BLACK, this));
        PlaceNewPiece('b', 7, new Pawn(board, Color.BLACK, this));
        PlaceNewPiece('c', 7, new Pawn(board, Color.BLACK, this));
        PlaceNewPiece('d', 7, new Pawn(board, Color.BLACK, this));
        PlaceNewPiece('e', 7, new Pawn(board, Color.BLACK, this));
        PlaceNewPiece('f', 7, new Pawn(board, Color.BLACK, this));
        PlaceNewPiece('g', 7, new Pawn(board, Color.BLACK, this));
        PlaceNewPiece('h', 7, new Pawn(board, Color.BLACK, this));
    }
}