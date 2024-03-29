using Assets.Scripts.TileEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : Tile
{
    public CandyType Type;

    public override List<GameObject> Explode()
    {
        return new List<GameObject> { gameObject };
    }

    public override void ExplodeFx()
    {
        this.Board.TilePool.GetCandyFx(Type).gameObject.transform.position = this.gameObject.transform.position;
    }

    public override bool Moveable()
    {
        return true;
    }
}
