using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    private Transform cameraTransform;

    public float strikeRadius;

    private bool alreadyStriked = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 dir = (cameraTransform.position-transform.position).normalized;
        Vector3 dest = cameraTransform.position - dir*(strikeRadius-0.3f);
        Debug.DrawLine(cameraTransform.position,dest);
        
        agent.destination = dest;

        if(Vector3.Distance(cameraTransform.position,transform.position)<strikeRadius && !alreadyStriked)
        {
            alreadyStriked = true;
            Strike();
        }
    }

    private void Strike()
    {

    }
}
