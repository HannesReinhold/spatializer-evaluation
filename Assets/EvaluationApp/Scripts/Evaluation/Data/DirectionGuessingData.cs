using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DirectionGuessingData
{
    public int evaluationID;
    public int spatializerID;
    public double timeToGuessDirection;
    public Vector3 sourceDirection;
    public Vector3 guessedDirection;
    public float azimuthDifference;
    public float elevationDifference;

    public DirectionGuessingData(int id)
    {
        evaluationID = id;
    }
}
