using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machineggun : MonoBehaviour 
{
    public static Machineggun instance;
    public GameObject egg;
    public Vector2 eggStartPosition;

    void Awake()
	{
		if(instance == null)
            instance = this;
		else
            Destroy(this.gameObject);
    }

	public void ShootEgg(Transform target)
	{
        var newEgg = Instantiate
			(egg, eggStartPosition, Quaternion.identity) as GameObject;
        var eggScript = newEgg.GetComponent<Egg>();
        eggScript.target = target;
    }
}
