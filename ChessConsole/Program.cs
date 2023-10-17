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
            try
            {
                UI.PrintBoard(chessMatch.GetPieces());

                ChessPosition source = UI.ReadChessPosition("Source");

                bool[,] possibleMoves = chessMatch.PossibleMoves(source);

                UI.PrintBoard(chessMatch.GetPieces(), possibleMoves);

                ChessPosition target = UI.ReadChessPosition("Target");

                ChessPiece? capturedChessPiece = chessMatch.PerformChessMove(source, target);
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