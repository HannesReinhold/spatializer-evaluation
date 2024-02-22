using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    float minTolerance = 0.5f;
    float maxTolerance = 2;

    Vector3 camPos;


    private void Update()
    {
        camPos = Camera.main.transform.position;
        float dist = Vector2.Distance(new Vector2(camPos.x,camPos.z), new Vector2(transform.position.x,transform.position.z));
        Vector3 dir = new Vector3(transform.position.x, 0, transform.position.z)-new Vector3(camPos.x, 0, camPos.z);
        dir.Normalize();
        if (dist < minTolerance) transform.position += dir * Mathf.Abs(dist-minTolerance) / 10;
    }
}
