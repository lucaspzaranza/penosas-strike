using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour 
{
	public SplineWalkerMode mode;
	public BezierSpline spline;
	public float duration;
	private float progress;
	public bool lookForward;
	private bool goingForward = true;

	void FixedUpdate () 
	{
		progress += Time.deltaTime / duration;
		if (progress > 1f) 
			progress = 1f;
		Vector3 position = spline.GetPoint(progress);
		transform.localPosition = position;
		if (lookForward)
            //transform.LookAt(position + spline.GetDirection(progress));	 		
            transform.rotation = Quaternion.LookRotation(position + spline.GetDirection(progress));
    }
}
