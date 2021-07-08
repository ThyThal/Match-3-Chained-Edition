using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingSystem : MonoBehaviour
{
    [SerializeField] public Dictionary<string, Queue<GameObject>> poolDictionary;
    [SerializeField] public List<Pool> pools;

    [System.Serializable]
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }

    public void PoolCreator()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        pools[0].size = GameManager.Instance.LevelController.NodesArray.Count;

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject currentObject = Instantiate(pool.prefab);
                currentObject.SetActive(false);
                objectPool.Enqueue(currentObject);
            }

            poolDictionary.Add(pool.name, objectPool);
        }
    }


    public GameObject SpawnFromPool(string name, Transform parent)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning($"Pool with name [{name}], does not exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[name].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetParent(parent);
        objectToSpawn.transform.localPosition = Vector3.zero;
        objectToSpawn.transform.localScale = Vector3.one;

        return objectToSpawn;
    }

    public void AddToPool(string name, GameObject current)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning($"Pool with name [{name}], does not exist.");
        }

        else
        {
            poolDictionary[name].Enqueue(current);
        }
    }
}
