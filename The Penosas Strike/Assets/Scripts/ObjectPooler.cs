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
            var poolParent = new GameObject(pool.tag + " Pool");
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, poolParent.transform);               
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }        
    }

    public GameObject SpawnFromPool(string tag)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag: " + tag + " doesn't exist!");
            return null;
        }    

        var objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetParent(null);
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}