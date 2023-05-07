using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    private float firstWaveEnemyDistribution = 6;
    private float secondWaveEnemyDistribution = 3;
    [SerializeField] private GameObject normalEnemyPrefab;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private GameObject cityHall;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float cooldownTime;

    private int remainningSpawns;
    [SerializeField] private int totalEnemySpawns = 6;
    private int enemyWaveQuantity = 3;
    [SerializeField] private int currentEnemyWave = 1;
    [SerializeField] private List<GameObject> aliveEnemies;
    private GameLogic game;

    private void Awake()
    {
        aliveEnemies = new List<GameObject>();
        remainningSpawns = totalEnemySpawns;
        GameObject waypointList = GameObject.FindGameObjectWithTag("Waypoint");
        for (int i = 0; i < waypointList.transform.childCount; i++)
        {
            Array.Resize(ref waypoints, waypoints.Length + 1);
            waypoints[i] = waypointList.transform.GetChild(i);
        }
    }

    private void Update()
    {
        if (IsEnemyWaveDead())
        {
            StartCoroutine(StartEnemyWave());            
        }

    }

    private IEnumerator StartEnemyWave()
    {
        
        if (currentEnemyWave <= enemyWaveQuantity && IsEnemyWaveDead())
        {   
            int enemiesPerWave = GetEnemyNumberPerWave();
            
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy(normalEnemyPrefab);
                yield return new WaitForSeconds(1f);
            }
            Debug.Log(enemiesPerWave + " enemies in this wave, " + remainningSpawns + " enemies remainning");
            currentEnemyWave++;
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
    public void KillEnemy(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);
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
}
