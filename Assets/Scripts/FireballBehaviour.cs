using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehaviour : MonoBehaviour
{
    
    [SerializeField] private string enemyPathLayer = "EnemyPath";
    [SerializeField] private string pathLayer = "Path";
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string fastEnemyTag = "FastEnemy";
    [SerializeField] private string slowEnemyTag = "SlowEnemy";

    [SerializeField] private GameObject fireExplosion;
    [SerializeField] private GameObject parentBonus;
    public GameObject originPlatformTower;
    private bool fireballCaught = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(enemyPathLayer) ||
           collision.gameObject.layer == LayerMask.NameToLayer(pathLayer) ||
           collision.gameObject.tag == enemyTag ||
           collision.gameObject.tag == fastEnemyTag ||
           collision.gameObject.tag == slowEnemyTag)
        {

            GameObject newFireExplosion = Instantiate(fireExplosion, collision.collider.ClosestPoint(transform.position), Quaternion.identity);
            originPlatformTower.GetComponent<PlatformTowerBehaviour>().IsFireballAlive = false;
            Destroy(this.gameObject);
        }
    }

    public void GainFireball()
    {
        if(!fireballCaught)
        {
            Debug.Log("Fireball caught");
            fireballCaught = true;
            GameLogic.GameInstance.HoldingFireball = false;
            parentBonus.GetComponent<BonusBehaviour>().FireballGrabbed();
            GameLogic.GameInstance.GetFireballSpells();
            Destroy(this.gameObject);
        }
    }

    public void HoldingFireball(bool boolValue)
    {
        if (!GameLogic.GameInstance.HoldingFireball)
        {
            GameLogic.GameInstance.HoldingFireball = boolValue;
        }   
    }

    public void RealeasingFireball()
    {
        Debug.Log("Release");
        GameLogic.GameInstance.HoldingFireball = false;
    }

    public bool FireballCaught
    {
        set
        {
            fireballCaught = value;
        }
    }
}
