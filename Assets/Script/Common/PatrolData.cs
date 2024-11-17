using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrolData
{
    public List<Transform> Points = new List<Transform>();
    public int Index;
    public int Order;
    public Vector3 NextPoint;
}
