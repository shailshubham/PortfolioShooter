using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;
    public Dictionary<string,Queue<GameObject>> poolDistionary = new Dictionary<string,Queue<GameObject>>();

    [System.Serializable]
    public class Pool
    {
        public string key;
        public GameObject prefab;
        public int count;
    }

    public List<Pool> pools = new List<Pool>();

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else if(instance!=this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach(Pool pool in pools)
        {
            Queue<GameObject> poolObjects = new Queue<GameObject>();
            for(int i = 0;i<pool.count;i++)
            {
                GameObject poolObject = Instantiate(pool.prefab);
                poolObject.SetActive(false);
                poolObjects.Enqueue(poolObject);
            }
            poolDistionary.Add(pool.key, poolObjects);
        }
    }

    public GameObject SpawnFromPool(string key,Vector3 position,Quaternion rotation)
    {
        if(!poolDistionary.ContainsKey(key))
        {
            Debug.LogWarning("ObjectPooler Doesnt Contain " + key);
            return null;
        }
        GameObject poolObject = poolDistionary[key].Dequeue();
        poolObject.transform.position = position;
        poolObject.transform.rotation = rotation;
        poolObject.SetActive(true);
        IPoolObject iPoolObject = poolObject.GetComponent<IPoolObject>();
        if (iPoolObject != null)
        {
            iPoolObject.OnSpawn();
        }
        poolDistionary[key].Enqueue(poolObject);
        return poolObject;
    }
}
