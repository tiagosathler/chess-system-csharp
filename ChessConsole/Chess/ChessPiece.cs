﻿using ChessConsole.Boardgame;

namespace ChessConsole.Chess;

internal abstract class ChessPiece : Piece
{
    public Color Color { get; }
    public int MoveCount { get; private set; }

    protected ChessPiece(Board board, Color color) : base(board)
    {
        Color = color;
    }

    protected bool ThereIsOpponentPiece(Position position)
    {
        return Board.Piece(position) is ChessPiece otherPiece && !otherPiece.Color.Equals(Color);
    }

    protected void IncreaseMoveCount()
    {
        MoveCount++;
    }

    protected void DecreaseMoveCount()
    {
        MoveCount--;
    }
}