using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class WireSphereDrawer
{
    public static Vector3[] CreateCircle(Vector3 position, Quaternion rotation, float radius, int segments, Color color)
    {
        Vector3[] circle = new Vector3[segments+1];

        // If either radius or number of segments are less or equal to 0, skip drawing
        if (radius <= 0.0f || segments <= 0)
        {
            return new Vector3[0];
        }

        // Single segment of the circle covers (360 / number of segments) degrees
        float angleStep = (360.0f / segments);

        // Result is multiplied by Mathf.Deg2Rad constant which transforms degrees to radians
        // which are required by Unity's Mathf class trigonometry methods

        angleStep *= Mathf.Deg2Rad;

        // lineStart and lineEnd variables are declared outside of the following for loop
        Vector3 lineStart = Vector3.zero;
        Vector3 lineEnd = Vector3.zero;

        for (int i = 0; i < segments; i++)
        {
            // Line start is defined as starting angle of the current segment (i)
            lineStart.x = Mathf.Cos(angleStep * i);
            lineStart.y = Mathf.Sin(angleStep * i);
            lineStart.z = 0.0f;

            // Line end is defined by the angle of the next segment (i+1)
            lineEnd.x = Mathf.Cos(angleStep * (i + 1));
            lineEnd.y = Mathf.Sin(angleStep * (i + 1));
            lineEnd.z = 0.0f;

            // Results are multiplied so they match the desired radius
            lineStart *= radius;
            lineEnd *= radius;

            // Results are multiplied by the rotation quaternion to rotate them 
            // since this operation is not commutative, result needs to be
            // reassigned, instead of using multiplication assignment operator (*=)
            lineStart = rotation * lineStart;
            lineEnd = rotation * lineEnd;

            // Results are offset by the desired position/origin 
            lineStart += position;
            lineEnd += position;

            // Points are connected using DrawLine method and using the passed color
            circle[i] = lineStart;
        }
        circle[segments] = circle[0];

        return circle;  
    }


    public static void DrawCircle(Vector3 position, Quaternion rotation, float radius, int segments, Color color)
    {
        // If either radius or number of segments are less or equal to 0, skip drawing
        if (radius <= 0.0f || segments <= 0)
        {
            return;
        }

        // Single segment of the circle covers (360 / number of segments) degrees
        float angleStep = (360.0f / segments);

        // Result is multiplied by Mathf.Deg2Rad constant which transforms degrees to radians
        // which are required by Unity's Mathf class trigonometry methods

        angleStep *= Mathf.Deg2Rad;

        // lineStart and lineEnd variables are declared outside of the following for loop
        Vector3 lineStart = Vector3.zero;
        Vector3 lineEnd = Vector3.zero;

        for (int i = 0; i < segments; i++)
        {
            // Line start is defined as starting angle of the current segment (i)
            lineStart.x = Mathf.Cos(angleStep * i);
            lineStart.y = Mathf.Sin(angleStep * i);
            lineStart.z = 0.0f;

            // Line end is defined by the angle of the next segment (i+1)
            lineEnd.x = Mathf.Cos(angleStep * (i + 1));
            lineEnd.y = Mathf.Sin(angleStep * (i + 1));
            lineEnd.z = 0.0f;

            // Results are multiplied so they match the desired radius
            lineStart *= radius;
            lineEnd *= radius;

            // Results are multiplied by the rotation quaternion to rotate them 
            // since this operation is not commutative, result needs to be
            // reassigned, instead of using multiplication assignment operator (*=)
            lineStart = rotation * lineStart;
            lineEnd = rotation * lineEnd;

            // Results are offset by the desired position/origin 
            lineStart += position;
            lineEnd += position;

            // Points are connected using DrawLine method and using the passed color
            Debug.DrawLine(lineStart, lineEnd, color);
        }
    }


    public static void DrawSphere(Vector3 position, Quaternion orientation, float radius, Color color, int segments = 4)
    {
        if (segments < 2)
        {
            segments = 2;
        }

        int doubleSegments = segments * 2;

        // Draw meridians

        float meridianStep = 180.0f / segments;

        for (int i = 0; i < segments; i++)
        {
            DrawCircle(position, orientation * Quaternion.Euler(0, meridianStep * i, 0), radius, doubleSegments, color);
        }

        // Draw parallels

        Vector3 verticalOffset = Vector3.zero;
        float parallelAngleStep = Mathf.PI / segments;
        float stepRadius = 0.0f;
        float stepAngle = 0.0f;

        for (int i = 1; i < segments; i++)
        {
            stepAngle = parallelAngleStep * i;
            verticalOffset = (orientation * Vector3.up) * Mathf.Cos(stepAngle) * radius;
            stepRadius = Mathf.Sin(stepAngle) * radius;

            DrawCircle(position + verticalOffset, orientation * Quaternion.Euler(90.0f, 0, 0), stepRadius, doubleSegments, color);
        }
    }
}
