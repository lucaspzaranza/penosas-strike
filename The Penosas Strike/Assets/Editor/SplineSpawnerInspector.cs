using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplineSpawner))]
public class SplineSpawnerInspector : Editor 
{
    private SplineSpawner spawner;
    public override void OnInspectorGUI()
	{           
        spawner = target as SplineSpawner;           
		base.OnInspectorGUI();
        if(GUILayout.Button("Generate Spline")) 
		{
            spawner.GenerateSpline();           
        }        
    }
}
