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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(enemyPathLayer) ||
           collision.gameObject.layer == LayerMask.NameToLayer(pathLayer) ||
           collision.gameObject.tag == enemyTag ||
           collision.gameObject.tag == fastEnemyTag ||
           collision.gameObject.tag == slowEnemyTag)
        {

            GameObject newFireExplosion = Instantiate(fireExplosion, collision.collider.ClosestPoint(transform.position), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
