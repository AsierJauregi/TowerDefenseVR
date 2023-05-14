using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamageScript : MonoBehaviour
{
    private GameObject[] explodedEnemies;
    private bool enemiesExploded = false;

    private void Update()
    {
        if (enemiesExploded)
        {
            StartCoroutine(killEnemies());
        }
    }

    private IEnumerator killEnemies()
    {
        yield return new WaitForSeconds(2f);
        foreach(GameObject enemy in explodedEnemies)
        {
            enemy.GetComponent<Enemy>().ReceiveDamage(enemy.GetComponent<Enemy>().MaxHealth);
        }
    }
    public GameObject[] ExplodedEnemies
    {
        set
        {
            explodedEnemies = value;
            enemiesExploded = true;
        }
    }
}
