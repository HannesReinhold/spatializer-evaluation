using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPathManager : MonoBehaviour
{
    public List<Transform> pathObjects = new List<Transform>();

    public GameObject mover;

    public int targetIndex = 0;
    public float transitionFactor = 0.1f;
    private float interpolatedTarget = 0;
    private int lastTargetIndex;



    public int currentTarget = 0;


    private void Update()
    {
        //if(currentTarget != targetIndex) SetTargetIndex(currentTarget);

        if (targetIndex == lastTargetIndex)
        {
            mover.transform.position = pathObjects[targetIndex].position;
            return;
        }

        interpolatedTarget = Mathf.Lerp(interpolatedTarget, targetIndex, Time.deltaTime*transitionFactor);
        float offset = Mathf.Abs(targetIndex - interpolatedTarget) / Mathf.Abs(targetIndex - lastTargetIndex);

        Vector3 pos = pathObjects[lastTargetIndex].position * (offset) + pathObjects[targetIndex].position * (1-offset);

        mover.transform.position = pos;
    }

    public void SetTargetIndex(int i)
    {
        currentTarget = i;

        lastTargetIndex = targetIndex;
        targetIndex = i;
    }
}
