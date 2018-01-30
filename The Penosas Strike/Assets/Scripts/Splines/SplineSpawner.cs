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

		SetRandomPositionInSpawnZone(out initialPosition);
        SetRandomPositionInSpawnZone(out finalPosition);
		splineComp.SetSplineExtension(initialPosition, finalPosition);
        
		Vector2 newPoint;
        for (int i = 1; i < splineComp.ControlPointCount - 1; i++)
		{
            SetRandomPositionInGameZone(out newPoint);
            splineComp.SetSplineNewPoint(i, ref newPoint);
        }

        splineComp.Loop = isLoop;
        SplineCount++;
    }

	private void SetRandomPositionInSpawnZone(out Vector2 point)
	{		
		float xCoord = spawnZone.bounds.extents.x;
        float yCoord;
		point.x = Random.Range(-xCoord, xCoord);		
		if(Mathf.Abs(initialPosition.x) < Mathf.Abs(spawnZone.points[7].x))
		{
			// If the value is less than the x collider limit, set it to the 
			// upper rectangle area.
            yCoord = Random.Range(spawnZone.points[7].y, spawnZone.points[1].y);
        }
		else
		{
			// Otherwise, set it to the side rectangle areas.
            yCoord = Random.Range(spawnZone.points[0].y, spawnZone.points[1].y);
        }
        point.y = yCoord;    
    }

	private void SetRandomPositionInGameZone(out Vector2 point)
	{
        float xCoord = Random.Range(-spawnZone.points[7].x, spawnZone.points[7].x);
        float yCoord = Random.Range(spawnZone.points[0].y, spawnZone.points[7].y);
        point.x = xCoord;
        point.y = yCoord;
    }
}