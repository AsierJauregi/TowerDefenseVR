using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDefender : MonoBehaviour
{
    private const string towerCollisionTag = "TowerCollision";
    private const string towerTag = "Tower";
    private const int damageIncrement = 15;
    private const float cooldownDecrement = 0.25f;
    public List<GameObject> aimedEnemies;
    [SerializeField]private GameObject aimedEnemy;
    [SerializeField] private GameObject canonBall;
    [SerializeField] private float shootingForce = 40;
    [SerializeField] private float firePower = 25;
    [SerializeField] private float cooldownTime = 2;
    [SerializeField] private float rotationSpeed = 10f;
    private float nextFireTime = 0;
    [SerializeField] private string preferredTargetTag = "FastEnemy";
    private string[] allTargetTag = {"Enemy", "FastEnemy" };
    private Transform turretCanon;
    private Transform towerCanvas;
    private int towerLevel = 1;
    private int maxTowerLevel = 3;
    private int buildedTurn = 1;
    private int levelUpCost = 20;
    private int levelUpcostIncrement = 25;
    private int towerCost;

    void Awake()
    {
        turretCanon = this.gameObject.transform.GetChild(0);
        aimedEnemies = new List<GameObject>();
        
    }
    private void Start()
    {
        buildedTurn = GameLogic.GameInstance.TurnNumber;
    }

    void AimAtEnemy()
    {
        if (aimedEnemies.Count > 0 && aimedEnemy != null)
        {
            Vector3 enemyPosition = aimedEnemy.transform.position;
            Vector3 enemyDirection = enemyPosition - turretCanon.position;
            float enemyDistance = enemyDirection.magnitude;

            Vector3 enemyDirectionXZ = Vector3.ProjectOnPlane(aimedEnemy.transform.position - transform.position, Vector3.up).normalized;
            Vector3 turretDirection = Vector3.Cross(transform.up, Vector3.up);
            float angle = Vector3.Angle(turretDirection, enemyDirectionXZ);
            if (angle < 90f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(enemyDirectionXZ, Vector3.up);
                Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                transform.rotation = Quaternion.Euler(0f, newRotation.eulerAngles.y, 0f);
            }

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
            newCanonBall.GetComponent<CanonBallCollision>().originTurret = this.gameObject; 
            nextFireTime = Time.time + cooldownTime;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (Array.IndexOf(allTargetTag, other.gameObject.tag) != -1)
        {
            aimedEnemies.Add(other.gameObject);
            if (aimedEnemies.Count > 1)
            {
                if(aimedEnemy != null) 
                {
                    if(aimedEnemy.tag != preferredTargetTag) ChooseTarget();
                }
                else
                {
                    ChooseTarget();
                }
            }
            else
            {
                aimedEnemy = other.gameObject;
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        //Check if enemy tag is in allTargetTags
        if (Array.IndexOf(allTargetTag, other.gameObject.tag) != -1)
        {
            aimedEnemies.Remove(other.gameObject);
            if(aimedEnemies.Count > 1)
            {
                ChooseTarget();
            }
            else if(aimedEnemies.Count == 1)
            {
                aimedEnemy = aimedEnemies[0];
            }
            else
            {
                aimedEnemy = null;
            }
        }

        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != towerCollisionTag && other.gameObject.tag != towerTag)
        {
            AimAtEnemy();
        }
    }
    private void ChooseTarget()
    {
        if (aimedEnemies.Count > 1)
        {
            foreach (GameObject enemy in aimedEnemies)
            {
                if (enemy.tag == preferredTargetTag && enemy != null)
                {
                    aimedEnemy = enemy;
                    break; 
                }
            }
            if (aimedEnemy == null)
            {
                aimedEnemy = aimedEnemies[0];
            }
        }
        else if(aimedEnemies.Count == 1)
        {
            aimedEnemy = aimedEnemies[0];
        }
    }
    public void KillEnemy(GameObject enemy)
    {
        aimedEnemies.Remove(enemy);
        aimedEnemy = null;
        ChooseTarget();
    }

    public void RemoveDeadEnemy(GameObject enemy)
    {
        if (aimedEnemies.Contains(enemy))
        {
            aimedEnemies.Remove(enemy);
            ChooseTarget();
        }
    }

    

    public void LevelUpTower()
    {
        if(GameLogic.GameInstance.Coins < levelUpCost)
        {
            Debug.Log("Not enough Coins to Level Up Turret");
        }
        else if (towerLevel >= maxTowerLevel)
        {
            Debug.Log("Max level reached");
        }
        else
        {
            GameLogic.GameInstance.SpendCoins(levelUpCost);
            towerLevel++;
            firePower += damageIncrement;
            cooldownTime -= cooldownDecrement;
            levelUpCost += levelUpcostIncrement;
            GetComponentInChildren<TowerUI>().UpdateNameText();
            GetComponentInChildren<TowerUI>().UpdateUpgradeButtonText();
        }
        
    }

    public void UnBuild()
    {
        if(GameLogic.GameInstance.Turn == GameLogic.TurnPhase.Building)
        {
            if(buildedTurn == GameLogic.GameInstance.TurnNumber) 
            {
                GameLogic.GameInstance.GetCoins(towerCost);
            }
            else if(buildedTurn < GameLogic.GameInstance.TurnNumber)
            {
                GameLogic.GameInstance.GetCoins(towerCost / 2);
            }
            Destroy(this.gameObject);
        }
    }
   
    public int TowerLevel
    {
        get
        {
            return towerLevel;
        }
    }
    public int TowerLevelUpCost
    {
        get
        {
            return levelUpCost;
        }
    }

    public int BuildedTurn
    {
        set
        {
            buildedTurn = value;
        }
    }

    public int TowerCost
    {
        set
        {
            towerCost = value;
        }
    }

    public string PreferredTargetTag
    {
        get
        {
            return preferredTargetTag;

        }
        set
        {
            preferredTargetTag = value;
            Debug.Log(name + " preferred tag: " + value);
        }
    }
}
