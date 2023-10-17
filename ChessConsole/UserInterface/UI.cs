using ChessConsole.Boardgame;
using ChessConsole.Chess;
using System.Text;

namespace ChessConsole.UserInterface;

internal static class UI

{
    private static readonly int SLEEP_TIME = 3000;

    internal static void PrintBoard(ChessPiece[,] chessPieces)
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

    internal static ChessPosition ReadChessPosition(string message)
    {
        Console.Write($"\n{message}: ");

        try
        {
            string position = Console.ReadLine()!;

            char column = char.Parse(position.Trim().ToLower()[..1]);
            int row = int.Parse(position.Trim()[1..]);

            return new ChessPosition(column, row);
        }
        catch (SystemException e)
        {
            throw new FormatException(e.Message);
        }
    }

    internal static void PrintErrorMessage(string message)
    {
        Console.WriteLine(message);
        Console.WriteLine("Press any key to continue...");
        Console.ReadLine();
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