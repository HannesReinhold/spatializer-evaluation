using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour
{

    public Transform origin;
    public Transform target;
    public Transform rotTrans;

    public LineRenderer lineActual;
    public LineRenderer lineCalc;

    public float a = 0;
    public float e = 0;

    public Vector3 rot;
    

    
    void Update()
    {
        Calc2();
        rotTrans.rotation = origin.rotation;
        rotTrans.Rotate(new Vector3(a, e, 0), Space.Self);
    }


    private void CalcAngles()
    {
        // get guessed and actual direction
        Vector3 guessedDirection = origin.forward;
        Vector3 actualDirection = (target.position - origin.position).normalized;

        // calculate Error (azimuth, elevation)
        Vector3 d = new Vector3(Vector3.Dot(actualDirection, Vector3.right), Vector3.Dot(actualDirection, Vector3.up), Vector3.Dot(actualDirection, Vector3.forward));
        //Debug.Log("X: "+d.x+",Y: "+d.y+",Z: "+d.z);
        float azimuth = Mathf.Atan2(d.x, d.z) / Mathf.PI;
        Vector3 proj = new Vector3(d.x, 0, d.z).normalized;
        float elevation = Mathf.Acos(Vector3.Dot(d, proj)) / Mathf.PI;
        if (d.z < 0) elevation = 1 - elevation;
        if (d.x < 0 || d.y < 0) elevation = -elevation;

        a = azimuth;
        e = elevation;

        Vector3 dir2 = Quaternion.Euler(-e*180,a*180,0)* origin.forward;
        //Vector3 r = Quaternion.Angle(origin.rotation, Quaternion.);
        //Quaternion r = Quaternion.FromToRotation(origin.forward, actualDirection);
        //Vector3 r2 = r.eulerAngles;

        lineCalc.SetColors(Color.red, Color.red);
        lineCalc.positionCount = 2;
        lineCalc.SetPosition(0, origin.position);
        lineCalc.SetPosition(1, origin.position + dir2);

        //Debug.Log("Azimuth: "+azimuth+ ", Elevation: "+elevation);


    }

    void Calc2()
    {
        // use this if rotate
        Quaternion look = Quaternion.FromToRotation(origin.forward, (target.position-origin.position).normalized);
        Vector3 d2 = look.eulerAngles;

        // use these as relative difference
        Quaternion dif = Quaternion.LookRotation(origin.InverseTransformPoint(target.position), Vector3.up);


        Vector3 calcVec = look * origin.forward;

        lineCalc.SetColors(Color.red, Color.red);
        lineCalc.positionCount = 2;
        lineCalc.SetPosition(0, origin.position);
        lineCalc.SetPosition(1, origin.TransformPoint(origin.InverseTransformPoint(target.position)));

        lineActual.SetColors(Color.red, Color.green);
        lineActual.positionCount = 2;
        lineActual.SetPosition(0, origin.position);
        lineActual.SetPosition(1, origin.position+calcVec);

        Debug.Log(dif.eulerAngles);

    }
}
