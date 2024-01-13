using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballTrail : MonoBehaviour
{
    public GameObject trail;
    public int frequency;
    private int frameCount = 0;

    void FixedUpdate()
    {
        if(frameCount == frequency)
        {
            var newTrail = Instantiate(trail, transform.position, transform.rotation) as GameObject;
            frameCount = 0;
            return;
        }
        frameCount++;
    }
}