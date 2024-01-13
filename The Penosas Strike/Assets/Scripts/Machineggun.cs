using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machineggun : MonoBehaviour 
{
    #region Variables
    public static Machineggun instance;
    public Transform eggShotPosition;
    public Transform smokePosition;
    [SerializeField] private GameObject egg;
    private Egg eggScript;
    private GameObject newEgg;
    public GameObject cannon;
    public GameObject smoke;
    public GameObject crosshair; 
    private GameObject currentCrosshair;
    public Animator animator;
    #endregion

    void Awake()
	{
		if(instance == null)
            instance = this;
		else if(instance != this)
            Destroy(this.gameObject);
    }  

	public void StartEggShooting(ref GameObject target)
	{                               
        StartCoroutine(RotateCannonAndShotEgg(target));        
    }

    private void ShotEgg(ref GameObject target)
    {        
        newEgg = Instantiate(egg, eggShotPosition.position, Quaternion.identity) as GameObject;
            
        eggScript = newEgg.GetComponent<Egg>();
        eggScript.target = target;
        newEgg.transform.position = eggShotPosition.position;                  
    }
    
    private IEnumerator RotateCannonAndShotEgg(GameObject target)
    {
        Vector2 direction = target.transform.position - cannon.transform.position;        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;   
        float current = cannon.transform.rotation.eulerAngles.z;
        while(current != angle)          
        { 
            yield return new WaitForEndOfFrame();
            if(!GameController.instance.IsGameOver)
            {
                current = Mathf.MoveTowardsAngle(current, angle, 15f);
                Quaternion rotation = Quaternion.AngleAxis(current, Vector3.forward);
                cannon.transform.rotation = rotation;
            }
            else yield break;
        }   

        // Disparar animação da fumacinha e um recuo do canhão
        int triggerHash = Animator.StringToHash("Ricochet");
        animator.SetTrigger(triggerHash);                            
        var newSmoke = ObjectPooler.Instance.SpawnFromPool("Cannon Smoke");
        newSmoke.transform.position = smokePosition.position;
        ShotEgg(ref target);

        SoundManager.instance.PlayAudio(ref SoundManager.instance.shot);        
    }
}