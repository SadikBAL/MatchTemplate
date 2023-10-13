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
    internal class BlastDoubleColorBomb : BlastFinder
    {
        public override Match Find(GameBoard Board, GameObject SelectedTile, GameObject SwappingTile)
        {
            Match Temp = new Match();
            if (!IsBlast(Board, SelectedTile, SwappingTile))
            {
                return Temp;
            }
            Temp.Type = MatchEnums.MatchType.Blast;
            foreach (GameObject Tile in Board.Tiles)
            {
                if (!Temp.Tiles.Contains(Tile))
                {
                    Temp.Tiles.Add(Tile);
                }
            }
            return Temp;
        }

        protected override bool IsBlast(GameBoard Board, GameObject SelectedTile, GameObject SwappingTile)
        {
            if(SelectedTile.GetComponent<ColorBomb>() != null && SwappingTile.GetComponent<ColorBomb>() != null)
            {
                return true;
            }
            return false;
        }
    }
}
