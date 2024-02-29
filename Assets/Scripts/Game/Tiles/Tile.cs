using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [HideInInspector] public GameBoard Board;
    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public bool Destroyed = false;
    public abstract List<GameObject> Explode();
    public abstract bool Moveable();

    public void SetGameBoard(GameBoard Board)
    {
        if(Board != null)
        {
            this.Board = Board;
            int IdA = Board.Tiles.FindIndex(x => x == this.gameObject);
            x = IdA % Board.Width;
            y = IdA / Board.Width;
        }
    }
    public int GetIndex()
    {
        if (Board != null)
            return x + y * Board.Width;
        else
            return -1;
    }

}
