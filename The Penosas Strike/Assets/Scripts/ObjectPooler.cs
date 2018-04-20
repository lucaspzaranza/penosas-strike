using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour 
{
	[System.Serializable]
	public class Pool
	{
        public string tag;      
        public GameObject prefab;
		public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public static ObjectPooler Instance;

    #region Singleton
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(this.gameObject);
    }
    #endregion

    void Start()
	{
        poolDictionary = new Dictionary<string, Queue<GameObject>>();    

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);               
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }        
    }

    public GameObject SpawnFromPool(string tag)
    {
        GameObject objectToSpawn = null;
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag: " + tag + " doesn't exist!");
            return null;
        }
        
        objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        poolDictionary[tag].Enqueue(objectToSpawn);        
              
        return objectToSpawn;
    }

    public void ReturnToPool(GameObject obj)
    {
        if(!poolDictionary.ContainsKey(obj.tag))
        {
            Debug.LogWarning("Pool with tag: " + obj.tag + " doesn't exist!");
            return;
        }

        poolDictionary[obj.tag].Enqueue(obj);
        obj.SetActive(false);
    }
}