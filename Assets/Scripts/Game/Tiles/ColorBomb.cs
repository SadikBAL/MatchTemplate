﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Tiles
{
    internal class ColorBomb : Tile
    {
        public override List<GameObject> Explode()
        {
            return new List<GameObject>() { gameObject };
        }

        public override void ExplodeFx()
        {
            this.Board.TilePool.GetColorBombFx().gameObject.transform.position = this.gameObject.transform.position;
        }

        public override bool Moveable()
        {
            return true;
        }
    }
}
