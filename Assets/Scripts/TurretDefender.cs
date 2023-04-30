using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDefender : MonoBehaviour
{
    public List<GameObject> aimedEnemies;
    [SerializeField] private GameObject canonBall;
    [SerializeField] private float shootingForce = 40;
    [SerializeField] private float firePower = 25;
    [SerializeField] private float cooldownTime = 2;
    private float nextFireTime = 0;
    [SerializeField] private string preferredTargetTag = "FastEnemy";
    private string[] allTargetTag = {"Enemy", "Slow", "FastEnemy" };
    private Transform turretCanon;
    private Transform towerCanvas;
    [SerializeField] public Camera mainCamera;

    void Awake()
    {
        turretCanon = this.gameObject.transform.GetChild(0);
        aimedEnemies = new List<GameObject>();
        towerCanvas = this.gameObject.transform.GetChild(2);
    }

    private void Update()
    {
        towerCanvas.LookAt(mainCamera.transform);
    }
    void AimAtEnemy(GameObject aimedEnemy)
    {
        if (aimedEnemies.Count > 0 && aimedEnemy != null)
        {
            Vector3 enemyPosition = aimedEnemy.transform.position;
            Vector3 enemyDirection = enemyPosition - turretCanon.position;
            float enemyDistance = enemyDirection.magnitude;

            Debug.DrawRay(turretCanon.position, enemyDirection * enemyDistance, Color.red);
            ShootTarget(enemyDirection, aimedEnemy);
        }
        else
        {
            Debug.DrawRay(turretCanon.position, turretCanon.TransformDirection(Vector3.forward) * 100, Color.red);
        }
    }

    void ShootTarget(Vector3 direction, GameObject target)
    {
        if (Time.time > nextFireTime)
        {
            GameObject newCanonBall = Instantiate(canonBall);
            newCanonBall.GetComponent<CanonBallCollision>().power = firePower;
            newCanonBall.GetComponent<CanonBallCollision>().target = target;
            newCanonBall.transform.position = turretCanon.position + turretCanon.transform.forward * 1.5f;
            newCanonBall.GetComponent<Rigidbody>().AddForce(direction * shootingForce);
            nextFireTime = Time.time + cooldownTime;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Tower")
        {
            foreach (string tag in allTargetTag) {
                if (other.gameObject.tag == tag)
                {
                    aimedEnemies.Add(other.gameObject);
                }
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        foreach (string tag in allTargetTag)
        {
            if (other.gameObject.tag == tag)
            {
                aimedEnemies.Remove(other.gameObject);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Tower")
        {
            if (aimedEnemies.Count > 1)
            {
                AimAtEnemy(PreferredTarget());
            }
            else if (aimedEnemies.Count == 1)
            {
                AimAtEnemy(other.gameObject);
            }
        }
    }
    GameObject PreferredTarget()
    {
        if (aimedEnemies.Count > 1)
        {
            foreach (GameObject enemy in aimedEnemies)
            {
                if (enemy.tag == preferredTargetTag && enemy != null) 
                {
                    return enemy; 
                }
            }
        }
        return null;
    }

}
