using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Blast
{
    public abstract class BlastFinder
    {
        public abstract Match Find(GameBoard Board,GameObject SelectedTile, GameObject SwappingTile);
        protected abstract bool IsBlast(GameBoard Board, GameObject SelectedTile, GameObject SwappingTile);
    }
}
