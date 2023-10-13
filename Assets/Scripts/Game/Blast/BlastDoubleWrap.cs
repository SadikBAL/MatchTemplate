using Assets.Scripts.Game.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Blast
{
    internal class BlastDoubleWrap : BlastFinder
    {
        public override Match Find(GameBoard Board, GameObject SelectedTile, GameObject SwappingTile)
        {
            Match Temp = new Match();
            if (!IsBlast(Board, SelectedTile, SwappingTile))
            {
                return Temp;
            }
            Temp.Tiles.AddRange(SelectedTile.GetComponent<CandyWrap>().Explode());
            Temp.Tiles.AddRange(SwappingTile.GetComponent<CandyWrap>().Explode());
            return Temp;
        }

        protected override bool IsBlast(GameBoard Board, GameObject SelectedTile, GameObject SwappingTile)
        {
            if (SelectedTile.GetComponent<CandyWrap>() && SwappingTile.GetComponent<CandyWrap>())
            {
                return true;
            }
            return false;
        }
    }
}
