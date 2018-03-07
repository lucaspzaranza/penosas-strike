using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CurveSpawner))]
public class CurveSpawnerInspector : Editor {
private CurveSpawner spawner;
    public override void OnInspectorGUI()
	{           
        spawner = target as CurveSpawner;           
		base.OnInspectorGUI();
        if(GUILayout.Button("Generate Curve")) 
		{
            spawner.GenerateCurve();           
        }        
    }
}
