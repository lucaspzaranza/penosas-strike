using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    #region Variables
    public SplineWalkerMode mode;
    public BezierSpline spline;
    public BezierCurve curve;
    public float duration;
    private float progress;
    private float initRotation, finalRotation;
    public bool lookForward;
    private bool goingForward = true;
    private SpriteRenderer sprite;
    #endregion

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        spline = FindObjectOfType(typeof(BezierSpline)) as BezierSpline;
        curve = FindObjectOfType(typeof(BezierCurve)) as BezierCurve;
    }

    private void FixedUpdate()
    {
        Move();

        // Spline
        // Vector3 position = spline.GetPoint(progress);        
        // Vector3 derivative = spline.GetDirection(progress);    

        // Curve
        Vector3 position = curve.GetPoint(progress);        
        Vector3 derivative = curve.GetDirection(progress);    

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
        if (goingForward)
        {
            progress += Time.fixedDeltaTime / duration;
            if (progress > 1f)
            {
                if (mode == SplineWalkerMode.Once)
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
            if (progress < 0f)
            {
                progress = -progress;
                goingForward = true;
                sprite.flipX = false;
            }
        }
    }
}