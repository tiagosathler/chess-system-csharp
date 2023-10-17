using ChessConsole.Chess;
using ChessConsole.UserInterface;

namespace ChessConsole;

internal static class Program
{
    private static void Main(string[] args)
    {
        ChessMatch chessMatch = new();
        UI.PrintBoard(chessMatch.GetPieces());
    }
}