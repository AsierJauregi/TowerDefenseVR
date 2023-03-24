using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDefender : MonoBehaviour
{
    public bool isTargetOnRadius = false;
    public Transform aimedEnemy;
    [SerializeField]
    private string targetTag = "Enemy";
    private Transform turretCanon;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake()
    {
        Transform turretCanon = this.gameObject.transform.GetChild(0);
        turretCanon.position = turretCanon.position;
    }

    private void FixedUpdate()
    {
        AimAtEnemy();
    }
    void AimAtEnemy()
    {
        
        
        if (isTargetOnRadius && aimedEnemy != null)
        {
            float enemyDistance = Mathf.Abs(Vector3.Distance(this.transform.position, aimedEnemy.position));
            

            Debug.DrawRay(turretCanon.position, turretCanon.TransformDirection(aimedEnemy.position) * enemyDistance, Color.red);
            Debug.Log("Tower is aiming at enemy");
        }
        else
        {
            Debug.DrawRay(turretCanon.position, turretCanon.TransformDirection(Vector3.forward) * 100, Color.red);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == targetTag)
        {
            isTargetOnRadius = true;
            aimedEnemy = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == targetTag)
        {
            isTargetOnRadius = false;
            aimedEnemy = null;
        }
    }
}
