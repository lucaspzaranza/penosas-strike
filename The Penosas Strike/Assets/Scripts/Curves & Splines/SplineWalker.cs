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
        CalculateProgress();

        position = curve.GetPoint(progress);        
        derivative = curve.GetDirection(progress);   
        transform.localPosition = position;       
            		 
        float newZ = Mathf.Atan2(derivative.y, derivative.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, newZ);

        if(Input.touchCount > 0)
        {
            Touch newTouch = Input.GetTouch(0);
            RaycastHit2D hit = Physics2D.Raycast
                (Camera.main.ScreenToWorldPoint(newTouch.position), Vector2.zero);

            if(hit.collider != null)
            {
                DestroyEnemy();
            }
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

    private void DestroyEnemy()
    {
        EnemySpawner.instance.enemyCount--;                 
        Destroy(curve.gameObject);
        Destroy(gameObject); 
    }

    private void LosePoint()
    {
        DestroyEnemy();
    }

#if UNITY_EDITOR_WIN
    void OnMouseDown()
    {
        DestroyEnemy();
    }
#endif
}