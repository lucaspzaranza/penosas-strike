using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour 
{
	public GameObject explosion;

	private void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.tag == "Player")	
		{		
			SoundManager.instance.PlayAudio(ref SoundManager.instance.explosion);
			SoundManager.instance.PlayChickenDieSound();
			Instantiate(explosion);				

			Destroy(gameObject);
		}
	}
}