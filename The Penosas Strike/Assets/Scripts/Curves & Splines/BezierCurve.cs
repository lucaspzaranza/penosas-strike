using UnityEngine;

public class BezierCurve : MonoBehaviour 
{
    public Vector3[] points;
    private const float deltaPosRate = 1.0f;
    private int curveSteps = 50;
    [HideInInspector] public Vector2[] curvePoints;   

	public float Length 
	{
		get 
		{ 
            return CurveLength();
        } 
	}

	public BezierCurve()
	{
        Reset();
    }

    public void Reset () 
	{
		points = new Vector3[] 
		{
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f),
		};
	}

	public Vector3 GetPoint (float t) 
	{
		return transform.TransformPoint(
			Bezier.GetPointMath(points[0], points[1], points[2], points[3], t));
	}

	public Vector3 GetVelocity (float t) 
	{
		return transform.TransformPoint(
			Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) -
			transform.position;
	}

	public Vector3 GetDirection(float t)
	{
        return GetVelocity(t).normalized;
    }

	public void UpdateCurvePoint(int index, ref Vector2 newPoint)
	{
		points[index] = newPoint;
	}	

	private float CurveLength()
	{
        float deltaDistance = 0f;
        curvePoints = new Vector2[curveSteps];
		for (int i = 0; i < curveSteps; i++)
		{
            float t = i / (float)curveSteps;
            curvePoints[i] = GetPoint(t);
			if(i > 0)
                deltaDistance += Vector2.Distance(curvePoints[i], curvePoints[i - 1]);               
        }

        return deltaDistance;
    }

	public float CalculateDeltaPosition(float t)
	{
		float velocity = GetVelocity(t).magnitude;
        float newDeltaPos = deltaPosRate / velocity;

        return newDeltaPos;
    }
}