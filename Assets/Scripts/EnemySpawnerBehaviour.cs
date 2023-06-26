using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBehaviour : MonoBehaviour
{
    private const float groundSurfaceY = 0.856959f;
    private float firstWaveEnemyDistribution = 6;
    private float secondWaveEnemyDistribution = 3;
    [SerializeField] private GameObject redDemon;
    [SerializeField] private GameObject blueCyclops;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private GameObject cityHall;
    private Transform[] waypoints;
    [SerializeField] private float cooldownTime;
    private int remainningSpawns;
    [SerializeField] private int totalEnemySpawns = 6;
    [SerializeField] private int totalEnemiesIncrementPerLevel = 10;
    [SerializeField] private float redDemonProbability = 0.5f;
    private int enemyWaveQuantity = 3;
    [SerializeField] private int currentEnemyWave = 1;
    [SerializeField] private List<GameObject> aliveEnemies;
    [SerializeField] private GameObject fireballBonusPrefab;
    [SerializeField] private string groundTag = "Ground";
    [SerializeField] private float boundsOffset = 1.5f;
    private bool isBonusAlive = false;
    [SerializeField] private bool defenseTurnOn = false;

    [SerializeField] private GameObject closeController;

    private GameLogic game;

    private void Awake()
    {
        aliveEnemies = new List<GameObject>();
        remainningSpawns = totalEnemySpawns;
        
        GameObject waypointList = GameObject.FindGameObjectWithTag("Waypoint");
        waypoints = new Transform[waypointList.transform.childCount];
        for (int i = 0; i < waypointList.transform.childCount; i++)
        {
            waypoints[i] = waypointList.transform.GetChild(i);
        }
    }

    private void Update()
    {
        StartLevel();
    }

    private void StartLevel()
    {
        if (defenseTurnOn) 
        {
            if (IsEnemyWaveDead())
            {
                StartCoroutine(StartEnemyWave());
            }
            if (!isBonusAlive && GameLogic.GameInstance.FireballSpells == 0)
            {
                SpawnBonus();
            }
        }
        
    }

    private IEnumerator StartEnemyWave()
    {
        
        if (currentEnemyWave <= enemyWaveQuantity && IsEnemyWaveDead())
        {   
            int enemiesPerWave = GetEnemyNumberPerWave();
            
            for (int i = 0; i < enemiesPerWave; i++)
            {
                float random = UnityEngine.Random.value;
                if (random < redDemonProbability)
                {
                    SpawnEnemy(redDemon);
                }
                else
                {
                    SpawnEnemy(blueCyclops);
                }
                
                yield return new WaitForSeconds(1f);
            }
            Debug.Log(enemiesPerWave + " enemies in this wave, " + remainningSpawns + " enemies remainning");
            currentEnemyWave++;
        }
        else if (currentEnemyWave > enemyWaveQuantity && IsEnemyWaveDead())
        {
            PrepareNextLevel();
        }
    }

    private void PrepareNextLevel()
    {
        if (GameLogic.GameInstance.Turn == GameLogic.TurnPhase.Defense)
        {
            Debug.Log("Level " + GameLogic.GameInstance.TurnNumber + " surpassed!");
            GameLogic.GameInstance.PassTurn();
            currentEnemyWave = 1;
            totalEnemySpawns += totalEnemiesIncrementPerLevel * GameLogic.GameInstance.TurnNumber;
            remainningSpawns = totalEnemySpawns;
        }
    }

    private bool IsEnemyWaveDead() 
    {
        if (aliveEnemies.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int GetEnemyNumberPerWave()
    {
        float result;
        if (currentEnemyWave == 1)
        {
            result = (float)totalEnemySpawns / (float)firstWaveEnemyDistribution;
            return Mathf.CeilToInt(result);
        }
        else if (currentEnemyWave == 2)
        {
            result = (float)totalEnemySpawns / (float)secondWaveEnemyDistribution;
            return Mathf.CeilToInt(result);
        }
        else
        {
            return remainningSpawns;
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        Vector3 spawnPosition = transform.position + spawnOffset;
        GameObject newEnemy = Instantiate(prefab, spawnPosition, transform.localRotation);
        newEnemy.GetComponent<Enemy>().SetCityHall(cityHall);
        newEnemy.GetComponent<Enemy>().SetSpawner(this.gameObject);
        newEnemy.GetComponent<Enemy>().Game = game;
        newEnemy.GetComponent<FollowThePath>().GetWaypointList(waypoints);
        
        remainningSpawns--;
        aliveEnemies.Add(newEnemy);
    }
    public void RemoveEnemyFromWave(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);
    }

    private void SpawnBonus()
    {
        isBonusAlive = true;
        GameObject[] grounds = GameObject.FindGameObjectsWithTag(groundTag);
        Bounds[] groundBounds = new Bounds[grounds.Length];
        for(int i = 0; i < grounds.Length; i++)
        {
            groundBounds[i] = grounds[i].GetComponent<MeshRenderer>().bounds;
            groundBounds[i].Expand(new Vector3(-boundsOffset, groundSurfaceY, -boundsOffset));
        }
        System.Random rnd = new System.Random();
        int randomIndex = rnd.Next(0, groundBounds.Length);
        Vector3 randomPosition = groundBounds[randomIndex].center + Vector3.Scale(UnityEngine.Random.insideUnitSphere, groundBounds[randomIndex].size * 0.5f);
        randomPosition.y = groundSurfaceY;
        GameObject fireballBonus = Instantiate(fireballBonusPrefab, randomPosition, Quaternion.identity);
        fireballBonus.GetComponent<BonusBehaviour>().enemySpawner = this.gameObject;
        fireballBonus.GetComponent<BonusBehaviour>().closeController = closeController;

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
    public bool IsBonusAlive
    {
        set
        {
            isBonusAlive = value;
        }
    }

    public bool DefenseTurnOn
    {
        set
        {
            defenseTurnOn = value;
        }
    }
}
