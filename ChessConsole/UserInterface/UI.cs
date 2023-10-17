using ChessConsole.Boardgame;
using ChessConsole.Chess;
using System.Text;

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
            Console.Write($"{Colors.GREEN}-{Colors.RESET}");
        }
        else
        {
            StringBuilder sb = new("\x1b[1m");

            switch (chessPiece.Color)
            {
                case Color.WHITE: { sb.Append(Colors.WHITE); break; }
                case Color.BLACK: { sb.Append(Colors.YELLOW); break; }
            }

            sb.Append(chessPiece);
            sb.Append(Colors.RESET);

            Console.Write(sb);
        }
        Console.Write(" ");
    }
}