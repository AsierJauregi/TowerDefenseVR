using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> neighbors;

    public Waypoint previous
    {
        get;
        set;
    }

    public float distance
    {
        get;
        set;
    }

    private void OnDrawGizmos()
    {
        if(neighbors == null)
        {
            return;
        }
        Gizmos.color = new Color(0f, 0f, 0f);
        foreach(var neighbor in neighbors)
        {
            if(neighbor != null)
            {
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
            }
        }
    }
}
