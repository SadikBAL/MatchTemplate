using Assets.Scripts.MatchEnums;
using Assets.Scripts.TileEnums;
using System.Collections.Generic;
using UnityEngine;

public class Match
{
    public MatchType Type;
    public StripeType StripeType = StripeType.None;

    public readonly List<GameObject> Tiles = new List<GameObject>();

    public void AddTile(GameObject tile)
    {
        Tiles.Add(tile);
    }

}
