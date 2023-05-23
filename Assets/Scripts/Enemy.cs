using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float damage = 50;
    [SerializeField] private float cooldown = 1.5f;
    [SerializeField] private int coins = 10;
    private float nextAttackTime = 0;
    private bool isInAttackRange = false;
    private bool isDying = false;
    
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private Slider slider;
    private Animator animator;
    [SerializeField] private GameObject cityHall;
    private GameObject spawner;
    private GameLogic game;
    

    void Awake()
    {
        health = maxHealth;
        slider.value = CalculateHealth();
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDying) HealthCheck();

        if (isInAttackRange && Time.time > nextAttackTime) 
        {
            StartCoroutine(Attack());
            nextAttackTime = Time.time + cooldown;
        }
    }
    void HealthCheck()
    {
        slider.value = CalculateHealth();

        if (health < maxHealth)
        {
            healthBarUI.SetActive(true);
        }
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }

    private IEnumerator Attack()
    {
        animator.SetTrigger("Attack 01");
        yield return new WaitForSeconds(0.3f);
        cityHall.GetComponent<CityHall>().ReceiveDamage(damage);
    }

    public void IsOnRange()
    {
        isInAttackRange = true;
    }
    public bool ReceiveDamage(float damagePoints)
    {
        Debug.Log(damagePoints + " damage received");
        health -= damagePoints;
        if (health <= 0) return true;
        else return false;
    }

    private IEnumerator Die()
    {
        isDying = true;
        Destroy(GetComponent<BoxCollider>());
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<FollowThePath>());
        healthBarUI.SetActive(false);
        spawner.GetComponent<EnemySpawnerBehaviour>().RemoveEnemyFromWave(this.gameObject);
        game.GetCoins(coins);
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(1.2f);
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Tower");
        foreach(GameObject turret in turrets)
        {
            turret.GetComponent<TurretDefender>().RemoveDeadEnemy(this.gameObject);
        }
        
        Debug.Log("Enemy killed");
        Destroy(this.gameObject);
    }

    public void SetCityHall(GameObject hall)
    {
        cityHall = hall;
    }

    public void SetSpawner(GameObject enemySpawner)
    {
        spawner = enemySpawner;
    }

    public GameLogic Game
    {
        get
        {
            return game;
        }
        set
        {
            game = value;
        }
    }
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }
}
