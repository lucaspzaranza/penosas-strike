using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour 
{
    #region Variables
    public GameObject target;    
    public GameObject eggParts;
    public GameObject rawEgg;
    public Vector2 initStartPosition;
    public float speed;
    #endregion    

    void Start()
    {
        speed = GameController.instance.eggSpeed;
    }

    void FixedUpdate()
	{                
        if(!GameController.instance.IsGameOver && !GameController.instance.IsPaused)
        {
            if(target != null)
            {                           
                var newPos = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.fixedDeltaTime);
                transform.position = newPos;  

                Vector2 direction = target.transform.position - transform.position;        
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                angle -= 90f; // To set the top of the egg to be targeting the pigeon
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = rotation;      
                
                Vector2 eggPosition = transform.position;
                Vector2 targetPosition = target.transform.position;

                if(eggPosition == targetPosition)
                {                    
                    SoundManager.instance.PlayAudio(ref SoundManager.instance.hitSFX);             
                    GameController.instance.Score++;                                      
                    DestroyEnemy(ref target);    
                    Destroy(gameObject);
                }
            }
        }        
    }

    private void DestroyEnemy(ref GameObject enemy)
    {
        EnemySpawner.instance.enemyCount--;     

        Animator enemyAnimator = enemy.GetComponent<Animator>();
        int explodeTrigger = Animator.StringToHash("Explode");        
        Destroy(enemy.transform.GetChild(0).gameObject);
        enemyAnimator.SetTrigger(explodeTrigger);             

        if(!GameController.instance.IsChristmas)                
        {
            var crackedEgg = Instantiate(eggParts, 
            transform.position, Quaternion.identity) as GameObject;
            if(crackedEgg != null) crackedEgg.transform.position = enemy.transform.position;
        }

        ScoreInstantiation(enemy.transform);
        //SoundManager.instance.PlayAudio(ref SoundManager.instance.pointScore);
    }

    private void ScoreInstantiation(Transform enemyTransform)
    {
        int randomNumber = Random.Range(0, 100);
               
        if(randomNumber == 2) // Instancia um ovo cru às vezes (1% de chance)
        {
            GameController.instance.Score++;  
            var newPlusTwoScore = ObjectPooler.Instance.SpawnFromPool("+2");
            newPlusTwoScore.transform.position = enemyTransform.position;
            Instantiate(rawEgg, enemyTransform.position, Quaternion.identity);
        }
        else
        {        
            var newPlusOneScore = ObjectPooler.Instance.SpawnFromPool("+1");
            newPlusOneScore.transform.position = enemyTransform.position;  
        }
    }
}