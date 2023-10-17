using ChessConsole.Boardgame;

namespace ChessConsole.Chess;

internal sealed class ChessMatch

{
    public static readonly int BOARD_SIZE = 8;

    public int Turn { get; set; }
    public Color CurrentPlayer { get; set; }

    private readonly Board board;

    public ChessMatch()
    {
        board = new Board(BOARD_SIZE, BOARD_SIZE);
    }

    public ChessPiece[,] GetPieces()
    {
        ChessPiece[,] chessPieces = new ChessPiece[board.Rows, board.Columns];

        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                chessPieces[i, j] = (ChessPiece)board.Piece(i, j);
            }
        }

        return chessPieces;
    }
}