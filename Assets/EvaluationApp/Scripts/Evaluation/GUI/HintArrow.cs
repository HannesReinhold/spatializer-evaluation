using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintArrow : MonoBehaviour
{

    public RectTransform bounds;
    public RectTransform arrow;

    public RectTransform target;

    public Vector2 direction = new Vector2(1,0);
    public Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = (target.position-bounds.position).normalized;

        position = bounds.position + ClampPosition(new Vector3(direction.x * (0.2f), direction.y * (0.08f), 0));
        arrow.position = position;
        direction = (target.position-position).normalized;
        float zRot = Vector2.SignedAngle(Vector2.right,direction);
        arrow.eulerAngles =new Vector3(0,0,zRot);
        
    }


    Vector3 ClampPosition(Vector3 pos)
    {
        float w = bounds.sizeDelta.x;
        float h = bounds.sizeDelta.y;
        if (pos.x < -1) pos.x = -1f;
        if (pos.y < -0.45) pos.y = -0.45f;
        if(pos.x > 1) pos.x = 1f;
        if(pos.y > 0.45) pos.y = 0.45f;
        

        return pos;
    }

    private float RoundedBoxSdf()
    {
        float r = 100;
        return Mathf.Sqrt(Mathf.Pow(-bounds.sizeDelta.x + r,2)+ Mathf.Pow(-bounds.sizeDelta.y + r, 2))-r;
    }
}
