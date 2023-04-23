using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand;
}

public class ObjectPooler : Singleton<ObjectPooler>
{
    public List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;

    private void Start()
    {
        pooledObjects = new List<GameObject>();

        foreach(ObjectPoolItem item in itemsToPool)
        {
            for(short c = 0; c < item.amountToPool; c++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        for(short c = 0; c < pooledObjects.Count; c++)
        {
            if(!pooledObjects[c].activeInHierarchy && pooledObjects[c].tag == tag)
            {
                return pooledObjects[c];
            }
        }
        
        foreach(ObjectPoolItem item in itemsToPool)
        {
            if(item.shouldExpand)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }
        }

        return null;
    }
}