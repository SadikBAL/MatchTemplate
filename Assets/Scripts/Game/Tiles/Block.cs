using Assets.Scripts.TileEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Tile
{
    public BlockType Type;
    public override List<GameObject> Explode()
    {
        //Play Animation
        return new List<GameObject> {};
    }

    public override bool Moveable()
    {
        return false;
    }
}
