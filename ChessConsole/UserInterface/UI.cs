using ChessConsole.Boardgame;
using ChessConsole.Chess;

namespace ChessConsole.UserInterface;

internal static class UI
{
    public static void PrintBoard(ChessPiece[,] chessPieces)
    {
        for (int i = 0; i < Board.BOARD_SIZE; i++)
        {
            Console.Write($"{Board.BOARD_SIZE - i} ");
            for (int j = 0; j < Board.BOARD_SIZE; j++)
            {
                PrintPiece(chessPieces[i, j]);
            }
            Console.WriteLine();
        }
        Console.WriteLine("  a b c d e f g h");
    }

    private static void PrintPiece(ChessPiece chessPiece)
    {
        if (chessPiece == null)
        {
            Console.Write("-");
        }
        else
        {
            Console.Write(chessPiece);
        }
        Console.Write(" ");
    }
}