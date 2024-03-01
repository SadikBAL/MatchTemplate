using Assets.Scripts.TileEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Tile
{
    public CollectableType Type;
    public override List<GameObject> Explode()
    {
        //Play Animation
        return new List<GameObject> {};
    }

    public override void ExplodeFx()
    {
       
    }

    public override bool Moveable()
    {
        return false;
    }
}