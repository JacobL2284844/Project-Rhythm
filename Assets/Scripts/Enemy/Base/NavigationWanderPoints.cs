using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationWanderPoints : MonoBehaviour
{
    public Transform[] points;
    public Transform GetRandomPoint()
    {
        return points[Random.Range(0, points.Length)];
    }
}
