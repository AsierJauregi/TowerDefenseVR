using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBallCollision : MonoBehaviour
{
    public float power = 25;
    // Start is called before the first frame update

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy hitted");
            Damage(other);
            Destroy(this.gameObject);
        }
    }

    void Damage(Collider enemy)
    {
        Debug.Log("Enemy health: " + enemy.GetComponent<EnemyHealth>().health);
        enemy.GetComponent<EnemyHealth>().ReceiveDamage(power);
        Debug.Log("Enemy health: " + enemy.GetComponent<EnemyHealth>().health);
    }
}
