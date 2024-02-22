using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DirectionGameData
{
    public List<RoundData> rounds;
    public List<Vector3> positions;
}

[System.Serializable]
public class RoundData
{
    public int roundID;
    public int positionID;
    public int spatializerID;
}
