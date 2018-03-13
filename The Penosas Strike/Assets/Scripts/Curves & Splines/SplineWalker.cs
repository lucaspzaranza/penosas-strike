using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    #region Variables
    public SplineWalkerMode mode;
    public BezierSpline spline;
    public BezierCurve curve;
    public float speed;
    public float duration;
    private float progress;
    public bool useSplines;
    private bool goingForward = true;
    private SpriteRenderer sprite;
    private Vector3 position;
    private Vector3 derivative;
    #endregion

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        spline = FindObjectOfType(typeof(BezierSpline)) as BezierSpline;
        curve = FindObjectOfType(typeof(BezierCurve)) as BezierCurve;            
    }

    private void FixedUpdate()
    {   
        CalculateProgress();             
        if(useSplines)     
        {
            position = spline.GetPoint(progress);        
            derivative = spline.GetDirection(progress);
        }
        else
        {
            position = curve.GetPoint(progress);        
            derivative = curve.GetDirection(progress);    
        }

        transform.localPosition = position;       

        // 2D		 
        float newZ = Mathf.Atan2(derivative.y, derivative.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, newZ);        
    }

    private void CalculateProgress()
    {
        //float newProgress = Time.fixedDeltaTime / duration;                
        float newProgress = Time.fixedDeltaTime * speed / curve.Length;
        print(newProgress);
        if (goingForward)
        {
            progress += newProgress;           
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
            progress -= newProgress;
            if (progress < 0f)
            {
                progress = -progress;
                goingForward = true;
                sprite.flipX = false;
            }
        }
    }

    void OnMouseDown()
    {
        print("Aloha!");
    }
}