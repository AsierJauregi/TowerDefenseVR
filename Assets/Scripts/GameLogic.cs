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
    [SerializeField] private int fireballSpells = 0;

    [SerializeField] GameObject enemySpawner;
    [SerializeField] GameObject rightControllerCanvas;
    

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
        enemySpawner.GetComponent<EnemySpawnerBehaviour>().Game = gameInstance;
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
            enemySpawner.GetComponent<EnemySpawnerBehaviour>().enabled = true;
            Debug.Log("Defense Turn Starting!");
        }
        else
        {
            turnPhase = TurnPhase.Building;
            Debug.Log("Building Turn");
            turnNumber++; 
        }
    }
    
    public void GetCoins(int coinQuantity)
    {
        coins += coinQuantity;
        rightControllerCanvas.GetComponent<RightControllerUIBehaviour>().UpdateCoins();
    }

    public void SpendCoins(int coinQuantity)
    {
        coins -= coinQuantity;
        rightControllerCanvas.GetComponent<RightControllerUIBehaviour>().UpdateCoins();
    }

    public void GetFireballSpells(int spellQuantity)
    {
        fireballSpells += spellQuantity;
        rightControllerCanvas.GetComponent<RightControllerUIBehaviour>().UpdateFireballSpells();
    }
    public void UseFireballSpell()
    {
        fireballSpells--;
        rightControllerCanvas.GetComponent<RightControllerUIBehaviour>().UpdateFireballSpells();
    }
    public int Coins
    {
        get
        {
            return coins;
        }
    }
    public int FireballSpells
    {
        get
        {
            return fireballSpells;
        }
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
