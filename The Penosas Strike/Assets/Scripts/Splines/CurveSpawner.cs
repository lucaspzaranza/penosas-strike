using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveSpawner : MonoBehaviour 
{
	#region Variables
    public static CurveSpawner instance;
	public SpawnLimits spawnLimits;
	private GameObject newCurve;
	#endregion

 	#region Properties
    private static int _curveCount; 
    public int CurveCount
	{
        get { return CurveSpawner._curveCount; }
        private set { CurveSpawner._curveCount = value; }
    }

	#endregion

    private void Awake()
	{
		if(instance == null)		
            instance = this;                
		else if(instance != this)
            Destroy(this.gameObject);
    }

    public void GenerateCurve()
	{
		newCurve = new GameObject("New Curve " + CurveCount, typeof(BezierCurve));
        BezierCurve curveComp = newCurve.GetComponent<BezierCurve>();
	        
		for (int i = 0; i < curveComp.points.Length; i++)
		{
            Vector2 newPoint;
            if(i == 0) 
				SetRandomPositionInSpawnZone(out newPoint, false);
			else if(i == curveComp.points.Length - 1) 
				SetRandomPositionInSpawnZone(out newPoint, true);	
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
            spawnLimits.lowerRightMin.position.x);
       
        yCoord = Random.Range(spawnLimits.lowerLeftMin.position.y, 
                spawnLimits.upperLeftMin.position.y);        
        
        point.x = xCoord;
        point.y = yCoord;
    }    

	private void SetRandomPositionInSpawnZone(out Vector2 point, bool isFinal)
	{				                       
        point.x = isFinal ? spawnLimits.lowerRightMax.position.x : 
            spawnLimits.lowerLeftMax.position.x;                   

        point.y = Random.Range(spawnLimits.lowerLeftMin.position.y, 
            spawnLimits.upperLeftMin.position.y);         
    }
}
