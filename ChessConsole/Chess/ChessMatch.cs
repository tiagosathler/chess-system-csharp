﻿using ChessConsole.Boardgame;
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
        board.PlacePiece(new Rook(board, Color.WHITE), new Position(0, 0));
        board.PlacePiece(new Rook(board, Color.WHITE), new Position(0, 7));
        board.PlacePiece(new King(board, Color.WHITE), new Position(7, 4));

        board.PlacePiece(new Rook(board, Color.BLACK), new Position(7, 7));
        board.PlacePiece(new Rook(board, Color.BLACK), new Position(7, 0));
        board.PlacePiece(new King(board, Color.BLACK), new Position(0, 4));
    }
}