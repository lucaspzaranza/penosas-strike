using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machineggun : MonoBehaviour 
{
    #region Variables
    public static Machineggun instance;
    public Vector2 eggStartPosition;
    private Egg eggScript;
    private GameObject newEgg;
    public bool eggIsCached { get; private set; }
    #endregion

    void Awake()
	{
		if(instance == null)
            instance = this;
		else if(instance != this)
            Destroy(this.gameObject);
    }

    void Start()
    {
        eggIsCached = false;
    }

    void Update()
    {        
        if(!eggIsCached && ObjectPooler.Instance.poolDictionary["Egg"].Count > 0)
        {                        
            CacheEggShot();
        }        
    }

    private void CacheEggShot()
    {
        newEgg = ObjectPooler.Instance.SpawnFromPool("Egg");        
        eggScript = newEgg.GetComponent<Egg>();
        eggIsCached = true;
    }

	public void ShootEgg(ref GameObject target)
	{       
        newEgg.transform.position = eggStartPosition;        
        eggScript.target = target;
        eggScript.isFired = true;
    }

    public void UncacheEggShot()
    {
        newEgg = null;
        eggScript = null;
        eggIsCached = false;
    }
}