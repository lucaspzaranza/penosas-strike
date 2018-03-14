using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
	#region Variables
    public static EnemySpawner instance;
    public GameObject pigeon;
    public GameObject pointPrefab;
    public float timeToSpawnEnemy;
    public SpawnLimits spawnLimits;    
    private GameObject newCurve;
    private float timer;
    #endregion

    #region Properties
    private static int _curveCount; 
    public int CurveCount
	{
        get { return EnemySpawner._curveCount; }
        private set { EnemySpawner._curveCount = value; }
    }

	#endregion

    private void Awake()
	{
		if(instance == null)		
            instance = this;                
		else if(instance != this)
            Destroy(this.gameObject);
    }   

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        // if(timer > timeToSpawnEnemy)
        // {
        //     timer = 0f;
        //     GenerateCurve();
        //     ObjectPooler.Instance.SpawnFromPool("pigeon");
        // }
    }

    public void GenerateCurve()
	{
		newCurve = new GameObject("New Curve " + CurveCount, typeof(BezierCurve));
        BezierCurve curveComp = newCurve.GetComponent<BezierCurve>();

        bool isLeft = Random.Range(0, 2) == 1;	        
		for (int i = 0; i < curveComp.points.Length; i++)
		{            
            Vector2 newPoint;
            if(i == 0) 
				SetRandomPositionInSpawnZone(out newPoint, isLeft);
			else if(i == curveComp.points.Length - 1) 
				SetRandomPositionInSpawnZone(out newPoint, !isLeft);	
			else
			 	SetRandomPositionInGameZone(out newPoint);

            curveComp.UpdateCurvePoint(i, ref newPoint);
        }			

        CurveCount++;
    }

	private void SetRandomPositionInGameZone(out Vector2 point)
	{
        float xCoord;
        float yCoord;

        xCoord = Random.Range(spawnLimits.lowerLeftMin.position.x, 
                -spawnLimits.lowerLeftMin.position.x);
       
        yCoord = Random.Range(spawnLimits.lowerLeftMin.position.y, 
                spawnLimits.upperLeftMin.position.y);        
        
        point.x = xCoord;
        point.y = yCoord;        
    }    

	private void SetRandomPositionInSpawnZone(out Vector2 point, bool isLeftSide)
	{				                       
        point.x = isLeftSide ? spawnLimits.lowerLeftMax.position.x : 
                -spawnLimits.lowerLeftMax.position.x;                   

        point.y = Random.Range(spawnLimits.lowerLeftMin.position.y, 
            spawnLimits.upperLeftMin.position.y);                    
    }
}
