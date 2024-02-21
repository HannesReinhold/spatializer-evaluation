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
    private bool alreadyPlaying = false;

    public Animator animator;

    public FMODUnity.StudioEventEmitter emitter;

    public ParticleSystem particles;
    public Renderer meshRendererHelmet;
    public Renderer meshRendererBody;
    public Renderer meshRendererSword;

    public StrikeEnemy exampleRef;

    public bool inactive = false;

    private Vector3 StartPosition;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cameraTransform = Camera.main.transform;
        StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (inactive) return;

        
        Vector3 dir = (cameraTransform.position-transform.position).normalized;
        Vector3 dest = cameraTransform.position - dir*(strikeRadius-0.3f);
        Debug.DrawLine(cameraTransform.position,dest);
        
        agent.destination = dest;

        if(Vector3.Distance(cameraTransform.position,transform.position)<strikeRadius)
        {
            if (!alreadyStriked)
            {
                alreadyStriked = true;
                Invoke("Strike", 0.2f);
                agent.isStopped = true;
                emitter.Stop();
                alreadyPlaying = false;
            }
        }
        else
        {
            agent.isStopped = false;
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
            if (!alreadyPlaying)
            {
                emitter.Play();
                alreadyPlaying = true;
            }
            
        }
    }

    private void Strike()
    {
        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);
        Debug.Log("Strike");
        PlaySwing((int)Random.Range(0,3));

        Invoke("ResetStrike", 1f/0.77f);
    }

    private void PlaySwing(int i)
    {
        switch (i)
        {
            case 0:
                FMODUnity.RuntimeManager.PlayOneShot("event:/Knight/SwordSwing/Swing1", transform.position);
                break;
            case 1:
                FMODUnity.RuntimeManager.PlayOneShot("event:/Knight/SwordSwing/Swing2", transform.position);
                break;
            case 2:
                FMODUnity.RuntimeManager.PlayOneShot("event:/Knight/SwordSwing/Swing3", transform.position);
                break;
            case 3:
                FMODUnity.RuntimeManager.PlayOneShot("event:/Knight/SwordSwing/Swing4", transform.position);
                break;
        }

    }

    private void ResetStrike()
    {
        alreadyStriked = false;
    }

    public void ResetValues()
    {
        meshRendererHelmet.enabled = true;
        meshRendererBody.enabled = true;
        meshRendererSword.enabled = true;
        emitter.enabled = true;
        //animator.StartPlayback();
        inactive = false;
        transform.position = StartPosition;
        alreadyStriked = false;
    }

    public void Defeat()
    {
        particles.Play();
        meshRendererHelmet.enabled = false;
        meshRendererBody.enabled = false;
        meshRendererSword.enabled = false;
        emitter.enabled = false;
        //animator.StopPlayback();
        inactive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        if (other.tag == "Sword")
        {
            Defeat();
            exampleRef.OnKill();
            Invoke("Destroy",2);
        }
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ResetValues();
    }
}
