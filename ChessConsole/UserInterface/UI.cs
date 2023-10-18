﻿using ChessConsole.Boardgame;
using ChessConsole.Chess;
using System.Text;

namespace ChessConsole.UserInterface;

internal sealed class UI
{
    private const string COLUMNS = "  a b c d e f g h";

    private readonly ChessMatch chessMatch;

    public UI(ChessMatch chessMatch)
    {
        this.chessMatch = chessMatch;
    }

    public void PrintFirstActOfTheMatch()
    {
        PrintBoard();
        PrintHead();
    }

    public void PrintSecondActOfTheMatch(bool[,] possibleMoves)
    {
        PrintBoard(possibleMoves);
        PrintHead();
    }

    private void PrintHead()
    {
        Console.WriteLine($"\n\x1b[1mTurn: {chessMatch.Turn}\x1b[0m");
        string message = $"\n\x1b\x1b[1mWaiting player: {chessMatch.CurrentPlayer}\x1b[0m";

        switch (chessMatch.CurrentPlayer)
        {
            case Color.WHITE: { Console.WriteLine($"{Colors.WHITE}{message}{Colors.RESET}"); break; }
            case Color.BLACK: { Console.WriteLine($"{Colors.YELLOW}{message}{Colors.RESET}"); break; }
        }
    }

    private void PrintBoard()
    {
        ChessPiece[,] pieces = chessMatch.GetPieces();

        Console.Clear();

        for (int i = 0; i < Board.BOARD_SIZE; i++)
        {
            Console.Write($"{Board.BOARD_SIZE - i} ");
            for (int j = 0; j < Board.BOARD_SIZE; j++)
            {
                PrintPiece(pieces[i, j], false);
            }
            Console.WriteLine();
        }

        Console.WriteLine(COLUMNS);
    }

    private void PrintBoard(bool[,] possibleMoves)
    {
        ChessPiece[,] pieces = chessMatch.GetPieces();

        Console.Clear();

        for (int i = 0; i < Board.BOARD_SIZE; i++)
        {
            Console.Write($"{Board.BOARD_SIZE - i} ");

            for (int j = 0; j < Board.BOARD_SIZE; j++)
            {
                PrintPiece(pieces[i, j], possibleMoves[i, j]);
            }

            Console.WriteLine();
        }

        Console.WriteLine(COLUMNS);
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
        Console.Clear();
    }

    private static void PrintPiece(ChessPiece chessPiece, bool background)
    {
        if (background)
        {
            Console.Write($"{BackgroundColors.BLUE}");
        }
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