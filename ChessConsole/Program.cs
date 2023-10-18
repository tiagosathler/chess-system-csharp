using ChessConsole.Chess;
using ChessConsole.UserInterface;

namespace ChessConsole;

internal static class Program
{
    private static void Main(string[] args)
    {
        ChessMatch chessMatch = new();
        UI ui = new(chessMatch);

        while (true)
        {
            try
            {
                ui.PrintFirstActOfTheMatch();

                ChessPosition source = UI.ReadChessPosition("Source");

                bool[,] possibleMoves = chessMatch.PossibleMoves(source);

                ui.PrintSecondActOfTheMatch(possibleMoves);

                ChessPosition target = UI.ReadChessPosition("Target");

                chessMatch.PerformChessMove(source, target);
            }
            catch (ChessException e)
            {
                UI.PrintErrorMessage(e.Message);
            }
            catch (SystemException e)
            {
                UI.PrintErrorMessage(e.Message);
            }
        }
    }
}