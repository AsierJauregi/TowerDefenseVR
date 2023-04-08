using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBallCollision : MonoBehaviour
{
    public float power;
    public GameObject target;
    // Start is called before the first frame update

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy" || other.gameObject.layer == LayerMask.NameToLayer("Path"))
        {
            //Debug.Log("Enemy hitted");
            Damage(target);
            Destroy(this.gameObject);
        }
    }

    void Damage(GameObject enemy)
    {
        //Debug.Log("Enemy health: " + enemy.GetComponent<Enemy>().health);
        enemy.GetComponent<Enemy>().ReceiveDamage(power);
        //Debug.Log("Enemy health: " + enemy.GetComponent<Enemy>().health);
    }
}
