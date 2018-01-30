using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour 
{
	#region Variables
	public SplineWalkerMode mode;
	public BezierSpline spline;
	public float duration;
	private float progress;
	public bool lookForward;
	private bool goingForward = true;
    private SpriteRenderer sprite;
	#endregion
	
	void Start()
	{
        sprite = GetComponent<SpriteRenderer>();
        spline = FindObjectOfType(typeof(BezierSpline)) as BezierSpline;
    }

    void FixedUpdate () 
	{
        Move();

        Vector3 position = spline.GetPoint(progress);   
		Vector3 derivative = spline.GetDirection(progress);            
        transform.localPosition = position;

		// 3D
		if (lookForward)		
            transform.LookAt(position + derivative);

		// 2D		 
        float newZ = Mathf.Atan2(derivative.y, derivative.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, newZ);
    }

	private void Move()	
	{
		if(goingForward)
		{			
			progress += Time.fixedDeltaTime / duration;
			if (progress > 1f)
			{
				if(mode == SplineWalkerMode.Once) 		
					progress = 1f;
				else if (mode == SplineWalkerMode.Loop)                
                    progress -= 1f;					                		
                else
                {					
                    progress = 2f - progress;                  
                    goingForward = false;
                    sprite.flipX = true;
                }
            }			
		}		
		else
		{
			progress -= Time.fixedDeltaTime / duration;
			if(progress < 0f)
			{
				progress = -progress;
				goingForward = true;  
				sprite.flipX = false;              
            }
		} 
	}
}