using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour 
{
    #region Variables
    public GameObject target;
    public Vector2 initStartPosition;
    public float speed;
    public bool isFired;
    private bool rotationChanged = false;
    private bool eggUncached = false;    
    #endregion    

    void Start()
    {
        speed = GameController.instance.eggSpeed;
    }

    void FixedUpdate()
	{                
        if(!GameController.instance.IsGameOver)
        {
            if(isFired && target != null)
            {        
                if(Machineggun.instance.eggIsCached && !eggUncached)
                {
                    Machineggun.instance.UncacheEggShot();
                    eggUncached = true;                
                }
                    
                var newPos = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.fixedDeltaTime);
                transform.position = newPos;  

                if(!rotationChanged)
                {
                    Vector2 direction = target.transform.position - transform.position;        
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    angle -= 90f; // To set the top of the egg to be targeting the pigeon
                    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    transform.rotation = rotation;

                    rotationChanged = true;
                }                     
            }
        }
    }

    private void DestroyEnemy(ref GameObject enemy)
    {
        EnemySpawner.instance.enemyCount--;                         
        Destroy(enemy); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Pigeon" && Equals(other.gameObject, target))
        {
            GameController.instance.Score++;            
            var pigeon = other.gameObject;
            var egg = gameObject;
            ObjectPooler.Instance.ReturnToPool(ref egg);
            DestroyEnemy(ref pigeon);
        }
    }     

    void OnDisable()
    {
        transform.rotation = Quaternion.identity;
        transform.position = initStartPosition;
        rotationChanged = false;
        eggUncached = false;
    }   
}