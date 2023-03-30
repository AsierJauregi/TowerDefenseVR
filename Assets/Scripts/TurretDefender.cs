using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDefender : MonoBehaviour
{
    public bool isTargetOnRadius = false;
    public Transform aimedEnemy;
    public GameObject canonBall;
    public float shootingForce = 40;
    public float firePower = 25;
    public float cooldownTime = 2;
    private float nextFireTime = 0;
    [SerializeField] private string targetTag = "Enemy";
    private Transform turretCanon;

    void Awake()
    {
        turretCanon = this.gameObject.transform.GetChild(0);
    }

    void FixedUpdate()
    {
        AimAtEnemy();
    }
    void AimAtEnemy()
    {
        if (isTargetOnRadius && aimedEnemy != null)
        {
            Vector3 enemyDirection = aimedEnemy.position - turretCanon.position;
            float enemyDistance = enemyDirection.magnitude;

            Debug.DrawRay(turretCanon.position, enemyDirection * enemyDistance, Color.red);
            ShootTarget(enemyDirection);
        }
        else
        {
            Debug.DrawRay(turretCanon.position, turretCanon.TransformDirection(Vector3.forward) * 100, Color.red);
        }
    }  
    void ShootTarget(Vector3 direction)
    {
        if (Time.time > nextFireTime)
        {
            GameObject newCanonBall = Instantiate(canonBall);
            newCanonBall.GetComponent<CanonBallCollision>().power = firePower;
            newCanonBall.transform.position = turretCanon.position + turretCanon.transform.forward * 1.5f;
            newCanonBall.GetComponent<Rigidbody>().AddForce(direction * shootingForce);
            nextFireTime = Time.time + cooldownTime;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == targetTag)
        {
            isTargetOnRadius = true;
            aimedEnemy = other.transform;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == targetTag)
        {
            isTargetOnRadius = false;
            aimedEnemy = null;
        }
    }

  
}
