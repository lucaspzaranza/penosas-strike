using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedEggDestroyer : MonoBehaviour 
{
	public float timeToDestroy;
	public Transform[] partsTransform;
	private Vector2[] partsInitialPosition;
	private int length;

	void OnEnable()
	{		
		length = partsTransform.Length;
		partsInitialPosition = new Vector2[length];
		for (int i = 0; i < length; i++)
		{
			partsInitialPosition[i] = partsTransform[i].localPosition;
		}

		//Invoke("ReturnToPool", timeToDestroy);		
	}		

	void Start()	
	{
		Invoke("AutoDestroy", timeToDestroy);
	}
	
	private void ReturnToPool()
	{
		var gObj = gameObject;
		ObjectPooler.Instance.ReturnToPool(ref gObj);
	}	

	private void AutoDestroy()
	{
		Destroy(gameObject);
	}

	void OnDisable() 
	{
		for (int i = 0; i < length; i++)
		{
			partsTransform[i].localPosition = partsInitialPosition[i];
		}	
	}
}