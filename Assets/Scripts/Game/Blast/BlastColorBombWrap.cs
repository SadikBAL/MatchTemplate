using Assets.Scripts.Game.Tiles;
using Assets.Scripts.TileEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Blast
{
    internal class BlastColorBombWrap : BlastFinder
    {
        public override Match Find(GameBoard Board, GameObject SelectedTile, GameObject SwappingTile)
        {
            Match Temp = new Match();
            if (!IsBlast(Board, SelectedTile, SwappingTile))
            {
                return Temp;
            }
            Temp.Type = MatchEnums.MatchType.Blast;

            GameObject Selected = SelectedTile.GetComponent<CandyWrap>() != null ? SelectedTile : SwappingTile;
            CandyType Type = Selected.GetComponent<CandyWrap>().Type;
            
            for (int Index = 0; Index < Board.Tiles.Count(); Index++)
            {
                if(Board.Tiles[Index].GetComponent<Candy>() &&
                   Board.Tiles[Index].GetComponent<Candy>().Type == Type)
                {
                    Board.UpgradeCandyToWrapCandy(Board.Tiles[Index]);
                }
            }
            
            Temp.Tiles.Add(SelectedTile);
            Temp.Tiles.Add(SwappingTile);
            foreach (GameObject Tile in Board.Tiles)
            {
                if (Tile != null && Tile.GetComponent<Candy>() != null &&
                    Tile.GetComponent<Candy>().Type == Type)
                {
                    if (!Temp.Tiles.Contains(Tile))
                    {
                        Temp.Tiles.Add(Tile);
                    }
                }
            }
            return Temp;
        }

        protected override bool IsBlast(GameBoard Board, GameObject SelectedTile, GameObject SwappingTile)
        {
            if ((SelectedTile.GetComponent<ColorBomb>() && SwappingTile.GetComponent<CandyWrap>()) || (SelectedTile.GetComponent<CandyWrap>() && SwappingTile.GetComponent<ColorBomb>()))
            {
                return true;
            }
            return false;
        }
    }
}
