using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawEggDestroyer : MonoBehaviour
{
	public float timeToDestroy;
	
	void Start () 
	{
		Invoke("DestroyRawEgg", timeToDestroy);
	}	

	private void DestroyRawEgg()
	{
		Destroy(gameObject);
	}
}
