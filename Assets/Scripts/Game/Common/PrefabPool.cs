using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

public class PrefabPool : MonoBehaviour
{
    public GameObject Prefab;
    public int InitSize;

    private readonly Stack<GameObject> Instances = new Stack<GameObject>();
    private void Awake()
    {
        Assert.IsNotNull(Prefab);
    }
    private void Start()
    {
        for (var i = 0; i < InitSize; i++)
        {
            var Object = CreateInstance();
            Object.SetActive(false);
            Instances.Push(Object);
        }
    }
    private GameObject CreateInstance()
    {
        var Object = Instantiate(Prefab);
        var PooledPrefab = Object.AddComponent<PooledPrefab>();
        PooledPrefab.Pool = this;
        Object.transform.SetParent(transform);
        return Object;
    }
    public void Reset()
    {
        var ObjectList = new List<GameObject>();
        foreach (var instance in transform.GetComponentsInChildren<PooledPrefab>())
        {
            if (instance.gameObject.activeSelf)
            {
                ObjectList.Add(instance.gameObject);
            }
        }
        foreach (var instance in ObjectList)
        {
            ReturnObject(instance);
        }
    }
    public void ReturnObject(GameObject ObjectRef)
    {
        var Object = ObjectRef.GetComponent<PooledPrefab>();
        Assert.IsNotNull(Object);
        Assert.IsTrue(Object.Pool == this);
        ObjectRef.SetActive(false);
        if (!Instances.Contains(ObjectRef))
        {
            Instances.Push(ObjectRef);
        }
    }
    public GameObject GetObject()
    {
        var Object = Instances.Count > 0 ? Instances.Pop() : CreateInstance();
        Object.SetActive(true);
        Object.transform.localScale = Vector3.one;
        return Object;
    }
}
public class PooledPrefab : MonoBehaviour
{
    public PrefabPool Pool;
}