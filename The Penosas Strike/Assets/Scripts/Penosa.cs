using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penosa : MonoBehaviour 
{
	public static Penosa instance;
	public GameObject roastedChicken;
	public GameObject ghostChicken;
	public Animator penosaAnimator;
	public float timeToInvokeFunction;

	private GameController GCtrl { get { return GameController.instance; }}
	
	private void Awake() 
	{	
		if(instance == null)
			instance = this;
		else
			Destroy(this.gameObject);
	}

	public void TransformChickenInRoastedChicken()
	{
		if(!GCtrl.IsChristmas) roastedChicken.SetActive(true);
		ghostChicken.SetActive(true);		
		gameObject.SetActive(false);
	}

	public void TriggerFireAnimation()
	{
		int fireHash = Animator.StringToHash("Fire!");
		penosaAnimator.SetTrigger(fireHash);
	}

	private void OnTriggerEnter2D(Collider2D other) 
	{		
		if(other.tag == "Bomb")
		{
			if(GCtrl.IsChristmas) timeToInvokeFunction = 0f;
			Invoke("TransformChickenInRoastedChicken", timeToInvokeFunction);		
		}
	}
}