using Assets.Scripts.TileEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Tiles
{
    internal class CandyStripe : Candy
    {
        public CandyStripeType StripeType;
        public override List<GameObject> Explode()
        {
            List<GameObject> Tiles = new List<GameObject>();

            if (StripeType == CandyStripeType.Horizontal)
            {
                for (int i = 0; i < Board.Width; i++)
                {
                    GameObject BoardItem = Board.GetTile(i, y);
                    if(BoardItem != null)
                        Tiles.Add(BoardItem);
                }
            }
            else
            {
                for (int j = 0; j < Board.Height; j++)
                {
                    GameObject BoardItem = Board.GetTile(x, j);
                    if (BoardItem != null)
                        Tiles.Add(BoardItem);
                }
            }
            return Tiles;
        }
        public override void ExplodeFx()
        {
            this.Board.TilePool.GetStripeCandyFx(Type,StripeType).gameObject.transform.position = this.gameObject.transform.position;
        }
    }
}
