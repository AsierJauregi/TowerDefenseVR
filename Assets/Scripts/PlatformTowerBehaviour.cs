using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTowerBehaviour : MonoBehaviour
{
    private const float PlatformHeight = 4.295614f;
    private const float xFireballSpawnOffset = -0.9f;
    private const float yFireballSpawnOffset = 1f;
    private const float zFireballSpawnOffset = 0.7f;
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private string playerTag = "Player";
    private bool isFireballAlive = false;
        
    public void TransportPlayer()
    {
        Vector3 platformPosition = transform.position;
        platformPosition.y = PlatformHeight;
        xrOrigin.transform.rotation = transform.rotation;
        xrOrigin.transform.position = platformPosition;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == playerTag)
        {   
            if(GameLogic.GameInstance.FireballSpells > 0 && !isFireballAlive)
            {
                
                SpawnFireball();
            }

        }
    }

    private void SpawnFireball()
    {
        isFireballAlive = true;
        Vector3 fireballPosition = new Vector3(xrOrigin.transform.position.x + xFireballSpawnOffset, xrOrigin.transform.position.y + yFireballSpawnOffset, xrOrigin.transform.position.z + zFireballSpawnOffset);
        GameObject newFireball = Instantiate(fireballPrefab, fireballPosition, Quaternion.identity);
        newFireball.GetComponent<FireballBehaviour>().originPlatformTower = this.gameObject;
        GameLogic.GameInstance.UseFireballSpell();
    }

    public bool IsFireballAlive
    {
        set
        {
            isFireballAlive = value;
        }
    }
}
