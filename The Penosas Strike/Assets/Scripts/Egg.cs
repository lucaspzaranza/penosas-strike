using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour 
{
    public Transform target;
    public float speed;

    void Start()
    {
        transform.LookAt(target);
        transform.rotation = new Quaternion(0f, 0f, -transform.rotation.z, 1f);
    }

    void FixedUpdate () 
	{
        var newPos = Vector2.MoveTowards(transform.position, target.position, speed);
        transform.position = newPos;   

        Vector3 targetDir = target.position - transform.position;
        float step = speed * Time.fixedDeltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.position, targetDir, step, 0.0f);
        Debug.DrawRay(transform.position, newDir, Color.red);        
        transform.rotation = Quaternion.LookRotation(newDir);       
    }
}