using ChessConsole.Chess;
using ChessConsole.UserInterface;

namespace ChessConsole;

internal static class Program
{
    private static void Main(string[] args)
    {
        ChessMatch chessMatch = new();

        while (true)
        {
            UI.PrintBoard(chessMatch.GetPieces());

            ChessPosition source = UI.ReadChessPosition("Source");
            ChessPosition target = UI.ReadChessPosition("Target");

            ChessPiece? capturedChessPiece = chessMatch.PerformChessMove(source, target);
        }
    }
}