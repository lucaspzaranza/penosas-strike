using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointArea
{
    Left = 0,
    Up = 1,
    Right = 2
};

public class EnemySpawner : MonoBehaviour 
{
	#region Variables
    public static EnemySpawner instance;
    public GameObject pigeon;
    public GameObject curvePrefab;
    public GameObject pointPrefab;
    public int enemyCount;
    public int maxEnemiesCurrent;
    public int maxEnemiesTotal;
    public float timeToSpawnEnemy;
    public float minTimeSpawn;
    public float maxTimeSpawn;
    public float minCurveLength;
    private const float offset = 1f;
    private const float minDeltaPoints = 3f;
    private const float initTimeToSpawnEnemy = 1f;
    public SpawnLimits spawnLimits;    
    private GameObject newCurve;
    private BezierCurve curveComp;
    private PointArea initPointArea;
    private PointArea finalPointArea;
    private float timer;

    #endregion

    private void Awake()
	{
		if(instance == null)		
            instance = this;                
		else if(instance != this)
            Destroy(this.gameObject);
    }           

    void Start()
    {
        timeToSpawnEnemy = initTimeToSpawnEnemy;
    }

    void FixedUpdate()
    {
        if(!GameController.instance.IsGameOver)
        {
            timer += Time.fixedDeltaTime;

            if(timer > timeToSpawnEnemy)        
            {                           
                if(enemyCount < maxEnemiesCurrent)            
                {                                                                                                       
                    GenerateCurve();
                    enemyCount++;

                    timer = 0f;
                    timeToSpawnEnemy = Random.Range(minTimeSpawn, maxTimeSpawn);                    
                }            
            }
        }
    }

    public void GenerateCurve()
	{        
        if(Application.isPlaying)                               
            newCurve = Instantiate(curvePrefab) as GameObject;
        else if(Application.isEditor)
            newCurve = new GameObject("Curve " + enemyCount, typeof(BezierCurve));

        if(newCurve != null)        
        {                        
            GetCurveAndSetPointAreas();
            SetCurveRandomPoints(ref newCurve);                        
        }
        
        Instantiate(pigeon);
        newCurve = null;
    }

    private void GetCurveAndSetPointAreas()
    {
        curveComp = newCurve.GetComponent<BezierCurve>();        
        PointArea[] areas = { PointArea.Left, PointArea.Up, PointArea.Right };

        int index = Random.Range(0, 2); // Left or Up
        initPointArea = areas[index];

        if(initPointArea == PointArea.Up)
            index = Random.Range(0, 2) * 2; //Left Or Right
        else if(initPointArea == PointArea.Left)
            index = Random.Range(1, 3); // Up or Right

        finalPointArea = areas[index];
    }

    private void SetCurveRandomPoints(ref GameObject newCurve)
    {
        while (curveComp.Length < minCurveLength)
        {
            for (int i = 0; i < curveComp.points.Length; i++)
            {            
                Vector2 newPoint = Vector2.zero;
                if(i == 0) 
                    SetRandomPositionInSpawnZone(i, ref newPoint, initPointArea);
                else if(i == curveComp.points.Length - 1)
                {
                    SetRandomPositionInSpawnZone(i, ref newPoint, finalPointArea);	
                    IncreasePointsDeltaYDistance(ref newPoint);
                }  
                else
                    SetRandomPositionInGameZone(ref newPoint, i);

                curveComp.UpdateCurvePoint(i, ref newPoint);
            }			            
        }
    }

    private void IncreasePointsDeltaYDistance(ref Vector2 newPoint)
    {
        int lenght = curveComp.points.Length - 1;
        Vector2 initPointChanged = curveComp.points[0];

        if(Mathf.Abs(finalPointArea - initPointArea) == 1) // Adjacent areas                   
        {
            //print("Init: " + initPointArea + " Final: " + finalPointArea);
            if(curveComp.points[lenght].y - curveComp.points[0].y < minDeltaPoints)
            {
                float newOffset = minDeltaPoints / 2f;

                initPointChanged.y += 
                    (initPointArea == PointArea.Up) ? newOffset : -newOffset;
                curveComp.UpdateCurvePoint(0, ref initPointChanged);

                newPoint.y += 
                    (finalPointArea == PointArea.Up) ? newOffset : -newOffset;                    
            }
        }
    }

	private void SetRandomPositionInGameZone(ref Vector2 point, int index)
	{
        float xCoord = 0f;
        float yCoord = 0f;
        int lenght = curveComp.points.Length;
        const float newOffset = 1.5f;

        if(index < lenght / 2)
        {
            if(curveComp.points[0].x <= 0f)                
                xCoord = Random.Range(spawnLimits.lowerLeftMin.position.x, 0f);                           
            else            
                xCoord = Random.Range(0f, -spawnLimits.lowerLeftMin.position.x);            
        }
        else
        {
            if(curveComp.points[lenght - 1].x <= 0f)
                xCoord = Random.Range(spawnLimits.lowerLeftMin.position.x, 0f);
            else
                xCoord = Random.Range(0f, -spawnLimits.lowerLeftMin.position.x);

            point.x = xCoord;
            point.y = yCoord;

            if(Mathf.Abs(Vector2.Distance(point, curveComp.points[1])) < 1f)
            {
                var offsetVector = new Vector2(Random.Range(0, offset), Random.Range(0, offset));
                point += (curveComp.points[lenght - 1].x <= 0f) ? -offsetVector : offsetVector;
            }
        }

        yCoord = Random.Range(spawnLimits.lowerLeftMin.position.y, 
                spawnLimits.upperLeftMin.position.y - newOffset);        
        
        point.x = xCoord;
        point.y = yCoord;        
    }    

	private void SetRandomPositionInSpawnZone(int index, ref Vector2 point, PointArea area)
	{				                 
        int lenght = curveComp.points.Length;
        float newX;

        newX =  Random.Range(spawnLimits.lowerLeftMax.position.x,
                (spawnLimits.lowerLeftMin.position.x - offset));  

        if(area == PointArea.Left) point.x = newX;

        else if(area == PointArea.Right) point.x = -newX; 

        else if(area == PointArea.Up)
        {
            float newOffset = offset / 2f;
            if(index == 0)
            {
                if(finalPointArea == PointArea.Left)
                    newX = Random.Range(newOffset, -spawnLimits.lowerLeftMax.position.x);
                else 
                    newX = Random.Range(spawnLimits.lowerLeftMax.position.x, newOffset);
            }
            else if(index == lenght - 1)
            {
                if(initPointArea == PointArea.Left)
                    newX = Random.Range(newOffset, -spawnLimits.lowerLeftMax.position.x);
                else
                    newX = Random.Range(spawnLimits.lowerLeftMax.position.x, newOffset);
            }

            point.x = newX;
            point.y = Random.Range(spawnLimits.upperLeftMin.position.y, 
                spawnLimits.upperLeftMax.position.y) + Random.Range(0.5f, offset);
            return;
        }
        
        point.y = Random.Range(spawnLimits.lowerLeftMin.position.y, 
            spawnLimits.upperLeftMin.position.y);                                                        
    }

    void OnDisable()
    {
        timer = 0f;
        enemyCount = 0;
        timeToSpawnEnemy = initTimeToSpawnEnemy;
    }
}