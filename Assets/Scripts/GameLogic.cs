using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    private const string spawnerName = "EnemySpawner";
    private const string interactionManagerName = "Interaction Manager";
    private static GameLogic gameInstance;

    public enum TurnPhase { Building, Defense } //Each turn consists of a first building phase and then a defense phase
    private TurnPhase turnPhase = TurnPhase.Building;
    private int turnNumber = 1; //Once both phases have elapsed we go into the next turn
    [SerializeField] private int coins = 60;

    [SerializeField] GameObject enemySpawner;
    

    public static GameLogic GameInstance
    {
        get
        {
            return gameInstance;
        }
    }
    private void Awake()
    {
        if(gameInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        gameInstance = this;
        DontDestroyOnLoad(gameObject);
        
       
        turnPhase = TurnPhase.Building;
        GameObject enemySpawner = GameObject.Find(spawnerName);
        enemySpawner.GetComponent<SpawnerBehaviour>().Game = gameInstance;
        GameObject interactionManager = GameObject.Find(interactionManagerName);
        interactionManager.GetComponent<ExampleInputSystemScript>().Game = gameInstance;


    }

    public void LoadGame()
    {
        SceneManager.LoadScene("VRTower");
    }

    public void PassTurn()
    {
        if (turnPhase == TurnPhase.Building)
        {
            turnPhase = TurnPhase.Defense;
            enemySpawner.GetComponent<SpawnerBehaviour>().enabled = true;
            Debug.Log("Defense Turn Starting!");
        }
        else
        {
            turnPhase = TurnPhase.Building;
            Debug.Log("Building Turn");
            turnNumber++; 
        }
    }
    public int Coins
    {
        get
        {
            return coins;
        }
    }
    public void GetCoins(int coinQuantity)
    {
        coins += coinQuantity;
        Debug.Log("+" + coinQuantity + " coins!");
    }

    public void SpendCoins(int coinQuantity)
    {
        coins -= coinQuantity;
        Debug.Log("You used " + coinQuantity + " coins");
    }

    public TurnPhase Turn
    {
        get
        {
            return turnPhase;
        }
    }
    public int TurnNumber
    {
        get
        {
            return turnNumber;
        }
    }
}
