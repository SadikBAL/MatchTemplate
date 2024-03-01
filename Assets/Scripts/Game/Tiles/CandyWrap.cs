using Assets.Scripts.MatchEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Tiles
{
    internal class CandyWrap : Candy
    {
        public override List<GameObject> Explode()
        {
            List<GameObject> Tiles = new List<GameObject>();
            Tiles.Add(gameObject);
            Tiles.Add(Board.GetTile(x - 1, y - 1));
            Tiles.Add(Board.GetTile(x, y - 1));
            Tiles.Add(Board.GetTile(x + 1, y - 1));
            Tiles.Add(Board.GetTile(x - 1, y));
            Tiles.Add(Board.GetTile(x + 1, y));
            Tiles.Add(Board.GetTile(x - 1, y + 1));
            Tiles.Add(Board.GetTile(x, y + 1));
            Tiles.Add(Board.GetTile(x + 1, y + 1));
            return Tiles;
        }
        public override void ExplodeFx()
        {
            this.Board.TilePool.GetWrappedCandyFx(Type).gameObject.transform.position = this.gameObject.transform.position;
        }
    }
    
}
