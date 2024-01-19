using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCue : MonoBehaviour
{
    public float disableRadius = 1.0f;
    public float disableDirection = 0.3f;

    public GameObject emitter;

    private Transform camTransform;

    private bool alreadyDisabled = false;

    private void Start()
    {
        camTransform = Camera.main.transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(camTransform.position,transform.position);
        Vector3 direction = (transform.position - camTransform.position).normalized;
        float dot = Vector3.Dot(camTransform.forward, direction);
        
        if (distance < disableRadius && dot < disableDirection)
        {
            if (!alreadyDisabled)
            {
                Invoke("DisableHint", 2);
                alreadyDisabled = true;
            }
        }
        else
        {
            CancelInvoke("DisableHint");
            alreadyDisabled = false;

        }
    }

    private void DisableHint()
    {
        emitter.SetActive(false);
    }
}
