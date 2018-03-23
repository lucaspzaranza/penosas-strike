using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor 
{
    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;
    private const int lineSteps = 20;
    private int selectedIndex = -1;
	private const float directionScale = 2f;

    private void OnSceneGUI()
	{
        curve = target as BezierCurve;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                         handleTransform.rotation : Quaternion.identity;
						 
		Vector3 p0 = ShowPoint(0);
		Vector3 p1 = ShowPoint(1);
		Vector3 p2 = ShowPoint(2);
		Vector3 p3 = ShowPoint(3);

        Handles.color = Color.gray;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p2, p3);

        ShowDirections();
        Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);

        Handles.color = Color.white;        
    }

    public override void OnInspectorGUI()
    {
        if(curve == null)
            curve = target as BezierCurve;
            
        base.DrawDefaultInspector();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Length");
        EditorGUILayout.LabelField(curve.Length.ToString(), GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();
    }

	private void ShowDirections()
	{
		Handles.color = Color.green;
        Vector3 point = curve.GetPoint(0f);
		Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
        for (int i = 1; i <= lineSteps; i++)
		{
            point = curve.GetPoint(i / (float)lineSteps);			
            Handles.DrawLine(point, point + 
			curve.GetDirection(i / (float)lineSteps) * directionScale);
        }
    }

	private Vector3 ShowPoint(int index)
	{
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);
        float size = HandleUtility.GetHandleSize(point);
        if (index == 0)
			size *= 2f;

        if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap)) 
		{
			selectedIndex = index;
            Repaint();
        }

        if(selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
		    if(EditorGUI.EndChangeCheck())
		    {
                Undo.RecordObject(curve, "Move Object");
                EditorUtility.SetDirty(curve);
                curve.points[index] = handleTransform.InverseTransformPoint(point);
            }
        }
        return point;
    }
}
