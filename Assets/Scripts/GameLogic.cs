using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    private const string spawnerName = "EnemySpawner";
    private const string interactionManagerName = "Interaction Manager";
    private const string rightControllerCanvasName = "RightControllerCanvas";
    private const string turretTag = "Tower";
    private const string platformTag = "PlatformTower";
    private static GameLogic gameInstance;

    public enum TurnPhase { Building, Defense } //Each turn consists of a first building phase and then a defense phase
    private TurnPhase turnPhase = TurnPhase.Building;
    private int turnNumber = 1; //Once both phases have elapsed we go into the next turn
    [SerializeField] private const int startingCoins = 100;
    private int coins;
    private int fireballSpells = 0;

    [SerializeField] GameObject enemySpawner;
    [SerializeField] GameObject rightControllerCanvas;
    [SerializeField] GameObject interactionManager;
    

    public static GameLogic GameInstance
    {
        get
        {
            return gameInstance;
        }
    }
    private void Awake()
    {
        if (gameInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        gameInstance = this;
        InitializeGameInstance();
    }

    public void InitializeGameInstance()
    {
        Time.timeScale = 1;
        turnNumber = 1;
        turnPhase = TurnPhase.Building;
        coins = startingCoins;
        enemySpawner = GameObject.Find(spawnerName);
        enemySpawner.GetComponent<EnemySpawnerBehaviour>().Game = gameInstance;
        interactionManager = GameObject.Find(interactionManagerName);
        interactionManager.GetComponent<ExampleInputSystemScript>().Game = gameInstance;
        rightControllerCanvas = GameObject.Find(rightControllerCanvasName);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("VRTower");
        InitializeGameInstance();
    }

    public void PassTurn()
    {
        if (turnPhase == TurnPhase.Building)
        {
            turnPhase = TurnPhase.Defense;
            enemySpawner.GetComponent<EnemySpawnerBehaviour>().enabled = true;
            enemySpawner.GetComponent<EnemySpawnerBehaviour>().DefenseTurnOn = true;
            DisableUIinAllTowers();
            Debug.Log("Defense Turn Starting!");
        }
        else if(turnPhase == TurnPhase.Defense)
        {
            turnPhase = TurnPhase.Building;
            turnNumber++;
            enemySpawner.GetComponent<EnemySpawnerBehaviour>().DefenseTurnOn = false;
            Debug.Log("Level " + turnNumber + "  starting -- Building Turn");

        }
    }

    private static void DisableUIinAllTowers()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag(turretTag);
        if (turrets.Length > 0)
        {
            foreach (GameObject turret in turrets)
            {
                turret.GetComponentInChildren<TowerUI>().DisableUI();
            }
        }
        GameObject platform = GameObject.FindGameObjectWithTag(platformTag);
        if (platform != null) platform.GetComponentInChildren<TowerUI>().DisableUI();
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
