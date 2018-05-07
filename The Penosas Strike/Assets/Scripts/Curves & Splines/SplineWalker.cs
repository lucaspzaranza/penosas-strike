using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    #region Variables
    public SplineWalkerMode mode;
    public BezierCurve curve;
    public float speed;
    private float progress;
    private float deltaPos;
    private bool goingForward = true;
    private bool isTarget = false;
    private SpriteRenderer sprite;
    private Vector3 position;
    private Vector3 derivative;
    #endregion

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        curve = FindObjectOfType(typeof(BezierCurve)) as BezierCurve;        
    }

    private void FixedUpdate()
    {        
        if (!isTarget)
        {
            CalculateProgress();

            position = curve.GetPoint(progress);
            derivative = curve.GetDirection(progress);
            transform.localPosition = position;

            float newZ = Mathf.Atan2(derivative.y, derivative.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, newZ);
        }
    }

    private void CalculateProgress()
    {      
        deltaPos += (goingForward) ? curve.CalculateDeltaPosition(progress) 
                    : -curve.CalculateDeltaPosition(progress);       

        float newProgress =  (deltaPos * speed) / curve.Length;
        progress = newProgress;

        if (goingForward)
        {                       
            if (progress > 1f)
            {                
                if (mode == SplineWalkerMode.Once)
                {
                    progress = 1f;
                    LosePoint();
                }
                else if (mode == SplineWalkerMode.Loop)
                {
                    deltaPos = 0f;
                    progress -= 1f;
                }
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
            if (progress < 0f)
            {                
                goingForward = true;
                sprite.flipX = false;
                LosePoint();
            }
        }        
    }

    private void LosePoint()
    {
        EnemySpawner.instance.enemyCount--;                                 
        Destroy(gameObject); 
    }
    
    void OnMouseDown()
    {
        if(!isTarget)
        {            
            isTarget = true;
            var gObj = gameObject;
            Machineggun.instance.ShootEgg(ref gObj);
        }        
    }

    void OnDestroy()
    {
        Destroy(curve.gameObject);
    }
}