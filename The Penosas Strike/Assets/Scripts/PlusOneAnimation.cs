using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusOneAnimation : MonoBehaviour 
{
	public float distance;
	public float speed;
	public float timeToDestroy;
	private float current;		

	void Update () 
	{
		if(!GameController.instance.IsPaused)
		{
			if(current <= distance)
			{
				current += Time.smoothDeltaTime;
				transform.Translate(0f, Time.smoothDeltaTime * speed, 0f);
			}
			else
			{
				current = 0f;					
				Invoke("ReturnToPool", timeToDestroy);
			}
		}
	}

	private void AutoDestroy()
	{
		Destroy(gameObject);
	}

	private void ReturnToPool()
	{
		GameObject gObj = gameObject;
		ObjectPooler.Instance.ReturnToPool(ref gObj);
	}
}
