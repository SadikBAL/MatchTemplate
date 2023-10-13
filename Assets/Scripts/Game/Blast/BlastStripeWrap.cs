using Assets.Scripts.Game.Tiles;
using Assets.Scripts.MatchEnums;
using Assets.Scripts.TileEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Blast
{
    internal class BlastStripeWrap : BlastFinder
    {
        public override Match Find(GameBoard Board, GameObject SelectedTile, GameObject SwappingTile)
        {
            Match Temp = new Match();
            if (!IsBlast(Board, SelectedTile, SwappingTile))
            {
                return Temp;
            }

            GameObject BoardItem;
            for (int i = 0; i < Board.Width; i++)
            {
                BoardItem = Board.GetTile(i, SelectedTile.GetComponent<Tile>().y);
                if (BoardItem != null && !Temp.Tiles.Contains(BoardItem))
                    Temp.Tiles.Add(BoardItem);
                BoardItem = Board.GetTile(i, SelectedTile.GetComponent<Tile>().y - 1);
                if (BoardItem != null && !Temp.Tiles.Contains(BoardItem))
                    Temp.Tiles.Add(BoardItem);
                BoardItem = Board.GetTile(i, SelectedTile.GetComponent<Tile>().y + 1);
                if (BoardItem != null && !Temp.Tiles.Contains(BoardItem))
                    Temp.Tiles.Add(BoardItem);
            }

            for (int j = 0; j < Board.Height; j++)
            {
                BoardItem = Board.GetTile(SelectedTile.GetComponent<Tile>().x, j);
                if (BoardItem != null && !Temp.Tiles.Contains(BoardItem))
                    Temp.Tiles.Add(BoardItem);
                BoardItem = Board.GetTile(SelectedTile.GetComponent<Tile>().x - 1, j);
                if (BoardItem != null && !Temp.Tiles.Contains(BoardItem))
                    Temp.Tiles.Add(BoardItem);
                BoardItem = Board.GetTile(SelectedTile.GetComponent<Tile>().x + 1, j);
                if (BoardItem != null && !Temp.Tiles.Contains(BoardItem))
                    Temp.Tiles.Add(BoardItem);
            }
    
            return Temp;
        }

        protected override bool IsBlast(GameBoard Board, GameObject SelectedTile, GameObject SwappingTile)
        {
            if ((SelectedTile.GetComponent<CandyStripe>() && SwappingTile.GetComponent<CandyWrap>()) || (SelectedTile.GetComponent<CandyWrap>() && SwappingTile.GetComponent<CandyStripe>()))
            {
                return true;
            }
            return false;
        }
    }
}
