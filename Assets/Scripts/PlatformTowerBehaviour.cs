using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTowerBehaviour : MonoBehaviour
{
    private const float PlatformHeight = 4.295614f;
    [SerializeField] private GameObject xrOrigin;
    
    public void TransportPlayer()
    {
        Vector3 platformPosition = transform.position;
        platformPosition.y = PlatformHeight;
        xrOrigin.transform.rotation = transform.rotation;
        xrOrigin.transform.position = platformPosition;
    }
}
