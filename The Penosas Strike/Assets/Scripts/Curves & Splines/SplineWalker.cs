using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    #region Variables
    public SplineWalkerMode mode;
    public BezierCurve curve;
    public Animator pigeonAnimator;     
    public GameObject crosshair;  
    public GameObject touchFinger; 
    public bool forceStop;
    private float speed;
    private float progress;
    private float deltaPos;
    private bool goingForward = true;
    private bool isTarget;
    private SpriteRenderer sprite;
    private Vector3 position;
    private Vector3 derivative;
    private Vector3 prevPos;   
    #endregion

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        curve = FindObjectOfType(typeof(BezierCurve)) as BezierCurve;
        speed = GameController.instance.pigeonSpeed;  
        isTarget = false;                 
    }

    private void FixedUpdate()
    {
        bool logic = !GameController.instance.IsPaused;        
        logic &= !isTarget;
        logic &= !forceStop;

        if (logic)
        {
            CalculateProgress();

            position = curve.GetPoint(progress);
            derivative = curve.GetDirection(progress);

            if (WillBeUpsideDown()) sprite.flipY = true;

            transform.localPosition = position;

            float newZ = Mathf.Atan2(derivative.y, derivative.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, newZ);
        }

        if(GameController.instance.IsPaused && pigeonAnimator.enabled)        
            pigeonAnimator.enabled = false;
    
        if(!GameController.instance.IsPaused && !pigeonAnimator.enabled)        
            pigeonAnimator.enabled = true;                                 
    }

    private void CalculateProgress()
    {
        deltaPos += (goingForward) ? curve.CalculateDeltaPosition(progress)
                    : -curve.CalculateDeltaPosition(progress);

        float newProgress = (deltaPos * speed) / curve.Length;
        progress = newProgress;

        if (goingForward)
        {
            if (progress > 1f)
            {
                if (mode == SplineWalkerMode.Once)
                {
                    progress = 1f;
                    if(!GameController.instance.IsGameOver)
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
            }
        }
    }

    private bool WillBeUpsideDown()
    {
        bool result = position.x > 0f;
        result &= position.x - transform.position.x < 0f;
        result &= !sprite.flipY;

        return result;
    }

    private void LosePoint()
    {
        if(!GameController.instance.lostLife)
            GameController.instance.lostLife = true;                
            
        EnemySpawner.instance.enemyCount--;
        GameController.instance.Life--;
        GameUI.instance.DeleteUIMessages(x => x.name.Contains("Miss"));        
        StartCoroutine(GameUI.instance.InstantiateUIMessage(GameUI.instance.miss));        
        Destroy(gameObject);
    }

    public void TargetPigeon()
    {
        bool logic = !GameController.instance.IsPaused;
        logic &= !GameController.instance.IsGameOver;        
        if (logic)
        {            
            var circleCollider = GetComponent<Collider2D>();
            if(circleCollider != null)                            
                circleCollider.enabled = false;

            isTarget = true;
            var gObj = gameObject;
            pigeonAnimator.SetTrigger("Targeted");  

            var crshr = Instantiate(crosshair, transform.position, Quaternion.identity);     
            crshr.transform.SetParent(transform, true);               
            
            Penosa.instance.TriggerFireAnimation();            
            Machineggun.instance.StartEggShooting(ref gObj);

            if(GameController.instance.ShowTouchFinger)                          
                Instantiate(touchFinger, transform.position, Quaternion.identity);                                                                        
        }
    }

    void OnMouseDown()
    {
        TargetPigeon();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {        
        bool pigeonIsNextToEnd = progress > 0.5f;
        if(other.tag == "Visible Area" && pigeonIsNextToEnd && !GameController.instance.IsGameOver)                     
            LosePoint();        
        else if(other.tag == "Swoosh Zone" && !pigeonIsNextToEnd)
            SoundManager.instance.PlayAudio(ref SoundManager.instance.birdSwoosh);
    }

    void OnDestroy()
    {
        if (curve != null)    
            Destroy(curve.gameObject);                    
    }
}