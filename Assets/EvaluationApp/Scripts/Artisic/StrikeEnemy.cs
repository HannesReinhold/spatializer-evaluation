using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeEnemy : MonoBehaviour
{
    public Transform rightControllerTransform;

    public GameObject sword;
    public GameObject enemy;



    private float currentOpacity = 0;
    private bool equipped = false;
    private Renderer rend;

    public AudioSource source;
    public AudioClip clip;
    private Vector3 lastPosition = Vector3.zero;
    private bool alreadySwung = false;

    private void Start()
    {
        GameObject anchor = GameObject.Find("RightHandAnchor");
        if (anchor != null) rightControllerTransform = anchor.transform;
        rend = sword.GetComponentInChildren<Renderer>();

        Invoke("EquipSword",3);
        rend.material.SetFloat("Opacity", 0.0f);

        lastPosition = rightControllerTransform.position;
        source = sword.GetComponent<AudioSource>();
        source.clip = clip; 

        
    }

    private void OnEnable()
    {
        Invoke("EquipSword", 3);
    }



    private void Update()
    {
        sword.transform.position = rightControllerTransform.position;
        sword.transform.rotation = rightControllerTransform.rotation;

        if(equipped && currentOpacity<1)
        {
            currentOpacity += Time.deltaTime * 0.4f;
            rend.material.SetFloat("_Opacity", currentOpacity);
        }

        float d = Vector3.Distance(sword.transform.position, lastPosition)/Time.deltaTime;
        if (d > 10f)
        {
            if (!alreadySwung)
            {
                source.PlayOneShot(clip);
                alreadySwung = true;
            }
        }
        else if(d<1)
        {
            alreadySwung = false;
        }
        lastPosition = sword.transform.position;
    }

    private void ResetSwing()
    {
        alreadySwung=false;
    }


    public void EquipSword()
    {
        sword.SetActive(true);
        equipped = true;
        GameObject laser = GameObject.Find("LaserPointer");
        laser.SetActive(false);
    }

}
