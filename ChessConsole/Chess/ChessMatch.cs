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
                chessPieces[i, j] = (ChessPiece)board.Piece(i, j);
            }
        }

        return chessPieces;
    }

    private void InitialSetup()
    {
        PlaceNewPiece('A', 1, new Rook(board, Color.WHITE));
        PlaceNewPiece('a', 8, new Rook(board, Color.WHITE));
        PlaceNewPiece('e', 1, new King(board, Color.WHITE));

        PlaceNewPiece('h', 1, new Rook(board, Color.BLACK));
        PlaceNewPiece('h', 8, new Rook(board, Color.BLACK));
        PlaceNewPiece('e', 8, new King(board, Color.BLACK));
    }

    private void PlaceNewPiece(char column, int row, ChessPiece chessPiece)
    {
        board.PlacePiece(chessPiece, new ChessPosition(column, row).ToPosition());
    }
}