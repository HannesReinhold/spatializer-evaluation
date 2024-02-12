using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RotateTowardsCam : MonoBehaviour
{

    public Transform target;
    public float rotationSpeed = 1;

    public bool enableXRot = false;


    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<FollowTarget>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (new Vector3(target.position.x,enableXRot ? target.position.y:0, target.position.z) - new Vector3(transform.position.x, enableXRot ? transform.position.y : 0, transform.position.z)).normalized;

        //create the rotation we need to be in to look at the target
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
