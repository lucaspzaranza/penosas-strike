using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineSpawner : MonoBehaviour 
{
    #region Variables
   	public static SplineSpawner instance;    
	public PolygonCollider2D spawnZone;
    public BoxCollider2D gameZone;
    private Vector2 initialPosition;
    private Vector2 finalPosition;	
	[SerializeField] private int numOfCurves;
	[SerializeField] private bool isLoop;	    
    private BezierControlPointMode controlPointMode;
    private GameObject newSpline;
    public SpawnLimits spawnLimits;

    #endregion

    #region Properties

    private static int _splineCount;
    public int SplineCount
	{
        get { return SplineSpawner._splineCount; }
        private set { SplineSpawner._splineCount = value; }
    }

	#endregion

    private void Awake()
	{
		if(instance == null)		
            instance = this;                
		else if(instance != this)
            Destroy(this.gameObject);
    }

    public void GenerateSpline()
	{		
        newSpline = new GameObject("New Spline " + SplineCount, typeof(BezierSpline));
        BezierSpline splineComp = newSpline.GetComponent<BezierSpline>();
    		        
		for (int i = 0; i < numOfCurves; i++)		
            splineComp.AddCurve();        
        
		SetRandomPositionInSpawnZone(out initialPosition, false);
        SetRandomPositionInSpawnZone(out finalPosition, true);
		splineComp.SetSplineExtension(initialPosition, finalPosition);
        
		Vector2[] newPoints = new Vector2[splineComp.ControlPointCount];
        int middlePoint = splineComp.ControlPointCount / 2;
        for (int i = 1; i < splineComp.ControlPointCount - 1; i++)
		{
            bool pointIsNotTheMiddlePoint = i < middlePoint - 1;
            pointIsNotTheMiddlePoint |= i > middlePoint + 1;
            if(pointIsNotTheMiddlePoint)
            {               
                SetRandomPositionInGameZone(out newPoints[i]);    
                splineComp.SetSplineNewPoint(i, ref newPoints[i], BezierControlPointMode.Aligned);            
            }    
            else //if (i == middlePoint)
            {
                newPoints[i].x = Random.Range(spawnLimits.lowerLeftMin.position.x, 
                spawnLimits.lowerRightMin.position.x);
       
                newPoints[i].y = Random.Range(spawnLimits.lowerLeftMin.position.y, 
                spawnLimits.upperLeftMin.position.y); 
                splineComp.SetSplineNewPoint(i, ref newPoints[i], BezierControlPointMode.Mirrored);       
            }            
        }           

        splineComp.Loop = isLoop;
        SplineCount++;
    }

	private void SetRandomPositionInSpawnZone(out Vector2 point, bool isFinal)
	{				                       
        point.x = isFinal ? spawnLimits.lowerRightMax.position.x : 
            spawnLimits.lowerLeftMax.position.x;                   

        point.y = Random.Range(spawnLimits.lowerLeftMin.position.y, 
            spawnLimits.upperLeftMin.position.y);         
    }

	private void SetRandomPositionInGameZone(out Vector2 point)
	{
        float xCoord;
        float yCoord;

        xCoord = Random.Range(spawnLimits.lowerLeftMin.position.x, 
            spawnLimits.lowerRightMin.position.x);
       
        yCoord = Random.Range(spawnLimits.lowerLeftMin.position.y, 
                spawnLimits.upperLeftMin.position.y);        
        
        point.x = xCoord;
        point.y = yCoord;
    }    
}