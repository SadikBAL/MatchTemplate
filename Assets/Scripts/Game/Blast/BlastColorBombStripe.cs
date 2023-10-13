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
    internal class BlastColorBombStripe : BlastFinder
    {
        public override Match Find(GameBoard Board, GameObject SelectedTile, GameObject SwappingTile)
        {
            Match Temp = new Match();
            if (!IsBlast(Board, SelectedTile, SwappingTile))
            {
                return Temp;
            }
            Temp.Type = MatchEnums.MatchType.Blast;
            GameObject Selected = SelectedTile.GetComponent<CandyStripe>() != null ? SelectedTile : SwappingTile;
            CandyType CandyType = Selected.GetComponent<CandyStripe>().Type;
            CandyStripeType CandyStripe = Selected.GetComponent<CandyStripe>().StripeType;
            for (int Index = 0; Index < Board.Tiles.Count(); Index++)
            {
                if (Board.Tiles[Index] &&
                    Board.Tiles[Index].GetComponent<Candy>() &&
                    Board.Tiles[Index].GetComponent<Candy>().Type == CandyType)
                {
                    if (CandyStripe == CandyStripeType.Horizontal)
                        Board.UpgradeCandyToStripeCandy(Board.Tiles[Index], StripeType.Horizontal);
                    else
                        Board.UpgradeCandyToStripeCandy(Board.Tiles[Index], StripeType.Vertical);
                }
            }

            Temp.Tiles.Add(SelectedTile);
            Temp.Tiles.Add(SwappingTile);
            foreach (GameObject Tile in Board.Tiles)
            {
                if (Tile != null && Tile.GetComponent<Candy>() != null &&
                    Tile.GetComponent<Candy>().Type == CandyType)
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
            if ((SelectedTile.GetComponent<ColorBomb>() && SwappingTile.GetComponent<CandyStripe>()) || (SelectedTile.GetComponent<CandyStripe>() && SwappingTile.GetComponent<ColorBomb>()))
            {
                return true;
            }
            return false;
        }
    }
}
