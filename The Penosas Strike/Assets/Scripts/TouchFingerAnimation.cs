using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchFingerAnimation : MonoBehaviour 
{
	public bool isLoop;
	public float distance;
	public float velocity;
	private float timer = 0f;
	private float newY = 0f;
	private float limit = 0f;
	private float initialPosition;
	private bool isDown;

	void Start() 
	{
		initialPosition = transform.position.y;
		limit = transform.position.y + distance;		
	}
	
	void FixedUpdate()
	{		
		if(newY == limit) isDown = true;
		if(newY == initialPosition && isLoop) isDown = false;
		else if(newY == initialPosition) Destroy(gameObject);
			
		timer += (isDown)? -Time.fixedDeltaTime * velocity : Time.fixedDeltaTime * velocity;
		
		newY = Mathf.Lerp(initialPosition, limit, timer);			
		transform.position = new Vector2(transform.position.x, newY);
	}	
}