using ChessConsole.Boardgame;
using ChessConsole.Pieces;

namespace ChessConsole.Chess;

internal sealed class ChessMatch

{
    public int Turn { get; private set; }
    public Color CurrentPlayer { get; private set; }
    public Color CurrentOpponentPlayer { get; private set; }
    public bool IsItCheck { get; private set; }
    public bool IsItCheckMate { get; private set; }
    public ChessPiece? EnPassantVulnerable { get; private set; }
    public ChessPiece? Promoted { get; private set; }
    public List<ChessPiece> CapturedChessPieces { get; }

    private readonly List<ChessPiece> chessPiecesOnTheBoard;

    private readonly Board board;

    public ChessMatch()
    {
        board = new Board(Board.BOARD_SIZE, Board.BOARD_SIZE);
        CapturedChessPieces = new();
        chessPiecesOnTheBoard = new();
        Turn = 1;
        (CurrentPlayer, CurrentOpponentPlayer) = (Color.WHITE, Color.BLACK);
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

        ChessPiece? capturedPiece = MakeMove(source, target);

        if (TestCheck(CurrentPlayer))
        {
            UndoMove(source, target, capturedPiece);
            throw new ChessException("You can't put yourself in check!");
        }

        CheckTheMovedChessPiece(source, target);

        AnalyzeCheckmateStatus();
    }

    public ChessPiece ReplacePromotedPiece(string typeOfChessPiece)
    {
        if (Promoted == null)
        {
            throw new ChessException("There is no piece to be promoted!");
        }

        if (!typeOfChessPiece.Equals(Symbols.Bishop)
            && !typeOfChessPiece.Equals(Symbols.Knight)
            && !typeOfChessPiece.Equals(Symbols.Rook)
            && !typeOfChessPiece.Equals(Symbols.Queen))
        {
            return Promoted;
        }

        Position position = Promoted.Position!;
        ChessPiece removedPiece = (ChessPiece)board.RemovePiece(position)!;
        chessPiecesOnTheBoard.Remove(removedPiece);

        ChessPiece newPiece = NewPiece(typeOfChessPiece, Promoted.Color);
        board.PlacePiece(newPiece, position);
        chessPiecesOnTheBoard.Add(newPiece);

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

    private ChessPiece? MakeMove(Position source, Position target)
    {
        ChessPiece sourcePiece = (ChessPiece)board.RemovePiece(source)!;
        ChessPiece? capturedPiece = board.RemovePiece(target) as ChessPiece;

        board.PlacePiece(sourcePiece, target);
        sourcePiece.IncreaseMoveCount();

        if (capturedPiece != null)
        {
            UpdateChessPieces(capturedPiece);
        }

        capturedPiece = AnalizeSpecialMoves(source, target, sourcePiece, capturedPiece);

        if (capturedPiece != null && !CapturedChessPieces.Contains(capturedPiece))
        {
            UpdateChessPieces(capturedPiece);
        }

        return capturedPiece;
    }

    private ChessPiece? AnalizeSpecialMoves(Position source, Position target, ChessPiece sourcePiece, ChessPiece? capturedPiece)
    {
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

            capturedPiece = (ChessPiece)board.RemovePiece(pawnPosition)!;
        }

        return capturedPiece;
    }

    private void CheckTheMovedChessPiece(Position source, Position target)
    {
        ChessPiece movedPiece = (ChessPiece)board.Piece(target)!;

        // special move - promotion - defaulting to the Queen chess piece
        Promoted = null;

        if (board.Piece(target) is Pawn &&
            (
                (movedPiece.Color.Equals(Color.WHITE) && target.Row == 0) ||
                (movedPiece.Color.Equals(Color.BLACK) && target.Row == 7)
            ))
        {
            Promoted = board.Piece(target) as ChessPiece;
            Promoted = ReplacePromotedPiece(Symbols.Queen);
        }

        // special move - en passant - set EnPassantVulnerable
        EnPassantVulnerable = (movedPiece is Pawn && Math.Abs(source.Row - target.Row) == 2) ? movedPiece : null;
    }

    private void AnalyzeCheckmateStatus()
    {
        IsItCheck = TestCheck(CurrentOpponentPlayer);

        if (IsItCheck)
        {
            IsItCheckMate = TestCheckMate(CurrentOpponentPlayer);
        }

        if (!IsItCheckMate)
        {
            NextTurn();
        }
    }

    private void UpdateChessPieces(ChessPiece capturedPiece)
    {
        CapturedChessPieces.Add(capturedPiece);
        chessPiecesOnTheBoard.Remove(capturedPiece);
    }

    private void UndoMove(Position source, Position target, ChessPiece? capturedPiece)
    {
        ChessPiece sourcePiece = (ChessPiece)board.RemovePiece(target)!;
        sourcePiece.DecreaseMoveCount();

        board.PlacePiece(sourcePiece, source);

        if (capturedPiece != null)
        {
            board.PlacePiece(capturedPiece, target);
            CapturedChessPieces.Remove(capturedPiece);
            chessPiecesOnTheBoard.Add(capturedPiece);
        }

        // reverting special move: castling to king-side rook
        if (sourcePiece is King && target.Column == source.Column + 2)
        {
            Position sourceT = new(source.Row, source.Column + 3);
            Position targetT = new(source.Row, source.Column + 1);
            ChessPiece rook = (ChessPiece)board.RemovePiece(targetT)!;
            board.PlacePiece(rook, sourceT);
            rook.DecreaseMoveCount();
        }

        // reverting special move: castling to queen-side rook
        if (sourcePiece is King && target.Column == source.Column - 2)
        {
            Position sourceT = new(source.Row, source.Column - 4);
            Position targetT = new(source.Row, source.Column - 1);
            ChessPiece rook = (ChessPiece)board.RemovePiece(targetT)!;
            board.PlacePiece(rook, sourceT);
            rook.DecreaseMoveCount();
        }

        // reverting special move en passant
        if (sourcePiece is Pawn && source.Column != target.Column && capturedPiece == EnPassantVulnerable)
        {
            ChessPiece pawn = (ChessPiece)board.RemovePiece(target)!;

            Position pawnPosition;

            if (sourcePiece.Color.Equals(Color.WHITE))
            {
                pawnPosition = new Position(3, target.Column);
            }
            else
            {
                pawnPosition = new Position(4, target.Column);
            }

            board.PlacePiece(pawn, pawnPosition);
        }
    }

    private bool TestCheck(Color color)
    {
        ChessPiece king = FindTheKing(color);

        foreach (ChessPiece p in chessPiecesOnTheBoard.FindAll(p => !p.Color.Equals(color)))
        {
            bool[,] mat = p.PossibleMoves();

            if (mat[king.Position!.Row, king.Position!.Column])
            {
                return true;
            }
        }
        return false;
    }

    private bool TestCheckMate(Color color)
    {
        if (!TestCheck(color))
        {
            return false;
        }

        foreach (ChessPiece chessPiece in chessPiecesOnTheBoard.FindAll(p => p.Color.Equals(color)))
        {
            bool[,] possibleMoves = chessPiece.PossibleMoves();

            for (int i = 0; i < Board.BOARD_SIZE; i++)
            {
                for (int j = 0; j < Board.BOARD_SIZE; j++)
                {
                    if (possibleMoves[i, j])
                    {
                        Position source = chessPiece.Position!;
                        Position target = new(i, j);

                        ChessPiece? capturedChessPiece = MakeMove(source, target);

                        bool isItCheck = TestCheck(color);

                        UndoMove(source, target, capturedChessPiece);

                        if (!isItCheck)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    private ChessPiece FindTheKing(Color color)
    {
        return chessPiecesOnTheBoard.Find(p => p is King && p.Color.Equals(color))
            ?? throw new ChessException($"There is not {CurrentPlayer} king on the board!");
    }

    private void NextTurn()
    {
        Turn++;
        (CurrentPlayer, CurrentOpponentPlayer) = (CurrentOpponentPlayer, CurrentPlayer);
    }

    private void PlaceNewPiece(char column, int row, ChessPiece chessPiece)
    {
        board.PlacePiece(chessPiece, new ChessPosition(column, row).ToPosition());
        chessPiecesOnTheBoard.Add(chessPiece);
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