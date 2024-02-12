using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesSpawner : MonoBehaviour
{
    public GameObject eyePrefab;
    public int numEyes;

    public List<GameObject> eyeList = new List<GameObject>();

    private int leftEye=0;

    void Start()
    {
        
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }



    public void SpawnEyes()
    {

        for(int i = 0; i < numEyes; i++)
        {
            Invoke("SpawnEye",Random.value*5);
        }
    }

    private void SpawnEye()
    {
        GameObject eye = Instantiate(eyePrefab);
        eye.transform.position = Random.onUnitSphere * 5 + new Vector3(Random.value, Random.value + Random.value) * 0.3f;
        eye.transform.parent = transform;
        eyeList.Add(eye);
    }

    public void RemoveEyes()
    {
        for (int i = 0; i < numEyes; i++)
        {
            Invoke("RemoveEye", Random.value*2);
        }

    }

    private void RemoveEye()
    {
        Debug.Log(leftEye);
        GameObject eye = eyeList[leftEye];
        Vector3 pos = eye.transform.position;
        Destroy(eye);
        leftEye++;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spooky/Plop",pos);

    }
}
