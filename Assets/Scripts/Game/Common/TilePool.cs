using Assets.Scripts.TileEnums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class TilePool : MonoBehaviour
{
    public List<CandyPrefabs> CandyList = new List<CandyPrefabs>();
    public List<CandyPrefabs> HorizontalCandyList = new List<CandyPrefabs>();
    public List<CandyPrefabs> VerticalCandyList = new List<CandyPrefabs>();
    public List<CandyPrefabs> WrappedCandyList = new List<CandyPrefabs>();
    public List<BlockPrefabs> BlockList = new List<BlockPrefabs>();
    public List<CollectablePrefabs> CollectableList = new List<CollectablePrefabs>();

    public Dictionary<CandyType,        PrefabPool> CandyPools = new Dictionary<CandyType, PrefabPool>();
    public Dictionary<CandyType,        PrefabPool> HorizontalCandyPools = new Dictionary<CandyType, PrefabPool>();
    public Dictionary<CandyType,        PrefabPool> VerticalCandyPools = new Dictionary<CandyType, PrefabPool>();
    public Dictionary<CandyType,        PrefabPool> WrappedCandyPools = new Dictionary<CandyType, PrefabPool>();
    public Dictionary<BlockType,        PrefabPool> BlockPools = new Dictionary<BlockType, PrefabPool>();
    public Dictionary<CollectableType,  PrefabPool> CollectablePools = new Dictionary<CollectableType, PrefabPool>();
    public PrefabPool ColorBombPool = new PrefabPool();

    public GameObject GetRandomCandy()
    {
        int Index = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(CandyType)).Length);
        return CandyPools[(CandyType)Index].GetObject();
    }
    public GameObject GetColorBomb()
    {
        return ColorBombPool.GetObject();
    }
    public GameObject GetStripeCandy(CandyType CandyType,CandyStripeType CandyStripeType)
    {
        if(CandyStripeType == CandyStripeType.Horizontal)
        {
            return HorizontalCandyPools[CandyType].GetObject();
        }
        else
        {
            return VerticalCandyPools[CandyType].GetObject();
        }
    }
    public GameObject GetWrappedCandy(CandyType CandyType)
    {
        return WrappedCandyPools[CandyType].GetObject();
    }
    void Awake()
    {
        foreach (CandyPrefabs Item in CandyList)
        {
            CandyPools.Add(Item.Key, Item.Value);
        }
        foreach (CandyPrefabs Item in HorizontalCandyList)
        {
            HorizontalCandyPools.Add(Item.Key, Item.Value);
        }
        foreach (CandyPrefabs Item in VerticalCandyList)
        {
            VerticalCandyPools.Add(Item.Key, Item.Value);
        }
        foreach (CandyPrefabs Item in WrappedCandyList)
        {
            WrappedCandyPools.Add(Item.Key, Item.Value);
        }
        foreach (BlockPrefabs Item in BlockList)
        {
            BlockPools.Add(Item.Key, Item.Value);
        }
        foreach (CollectablePrefabs Item in CollectableList)
        {
            CollectablePools.Add(Item.Key, Item.Value);
        }
    }
}
[Serializable]
public class CandyPrefabs
{
    public CandyType Key;
    public PrefabPool Value;
}
[Serializable]
public class BlockPrefabs
{
    public BlockType Key;
    public PrefabPool Value;
}
[Serializable]
public class CollectablePrefabs
{
    public CollectableType Key;
    public PrefabPool Value;
}
