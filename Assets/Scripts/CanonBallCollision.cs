using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBallCollision : MonoBehaviour
{
    public float power;
    public GameObject target;
    public GameObject originTurret;
    // Start is called before the first frame update

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy" || other.gameObject.layer == LayerMask.NameToLayer("Path"))
        {
            if(target != null) Damage(target);
            Destroy(this.gameObject);
        }
    }

    void Damage(GameObject enemy)
    {
        bool isEnemyDead = enemy.GetComponent<Enemy>().ReceiveDamage(power);
        if (isEnemyDead && enemy != null)
        {
            originTurret.GetComponent<TurretDefender>().KillEnemy(enemy);
        }
    }
}
