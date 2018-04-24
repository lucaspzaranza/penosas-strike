using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour 
{
    public Transform target;
    public float speed;

    void Start()
    {
        Vector2 direction = target.position - transform.position;        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90f; // To set the top of the egg to be targeting the pigeon
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }

    void FixedUpdate()
	{
        var newPos = Vector2.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        transform.position = newPos;                
    }
}