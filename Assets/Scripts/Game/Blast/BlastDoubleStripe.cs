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
    internal class BlastDoubleStripe : BlastFinder
    {
        public override Match Find(GameBoard Board, GameObject SelectedTile, GameObject SwappingTile)
        {
            Match Temp = new Match();
            if (!IsBlast(Board, SelectedTile, SwappingTile))
            {
                return Temp;
            }
            Temp.Tiles.AddRange(SelectedTile.GetComponent<CandyStripe>().Explode());
            Temp.Tiles.AddRange(SwappingTile.GetComponent<CandyStripe>().Explode());
            return Temp;
        }

        protected override bool IsBlast(GameBoard Board, GameObject SelectedTile, GameObject SwappingTile)
        {
            if (SelectedTile.GetComponent<CandyStripe>() && SwappingTile.GetComponent<CandyStripe>())
            {
                return true;
            }
            return false;
        }
    }
}
