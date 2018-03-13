using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySpawner))]
public class CurveSpawnerInspector : Editor {
private EnemySpawner spawner;
    public override void OnInspectorGUI()
	{           
        spawner = target as EnemySpawner;           
		base.OnInspectorGUI();
        if(GUILayout.Button("Generate Curve")) 
		{
            spawner.GenerateCurve();           
        }        
    }
}
