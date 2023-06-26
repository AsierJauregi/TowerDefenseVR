using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTowerBehaviour : MonoBehaviour
{
    private const float PlatformHeight = 4.295614f;
    private const int levelUpCostIncrement = 25;
    private Vector3 fireballSpawnOffset = new Vector3(-0.9f, 1f, 0.7f);
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject leftController;
    [SerializeField] private string playerTag = "Player";
    private bool isFireballAlive = false;
    [SerializeField] private Camera mainCamera;
    private int towerLevel = 1;
    private int maxTowerLevel = 3;
    private int buildedTurn = 1;
    private int towerCost;
    private int levelUpCost = 20;

    private void Start()
    {
        buildedTurn = GameLogic.GameInstance.TurnNumber;
        GameLogic.GameInstance.PlatformLevel = 1;
    }
    public void TransportPlayer()
    {
        Vector3 platformPosition = transform.position;
        platformPosition.y = PlatformHeight;
        xrOrigin.transform.rotation = transform.rotation;
        xrOrigin.transform.position = platformPosition;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == playerTag)
        {   
            if(GameLogic.GameInstance.FireballSpells > 0 && !isFireballAlive)
            {
                SpawnFireball();
            }

        }
    }

    private void SpawnFireball()
    {
        isFireballAlive = true;
        //xrOrigin.transform.rotation = Quaternion.Euler(xrOrigin.transform.rotation.eulerAngles.x, mainCamera.transform.rotation.eulerAngles.y, xrOrigin.transform.rotation.eulerAngles.z);
        Quaternion rotation = Quaternion.Euler(xrOrigin.transform.rotation.eulerAngles.x, mainCamera.transform.rotation.eulerAngles.y, xrOrigin.transform.rotation.eulerAngles.z);
        Vector3 position = xrOrigin.transform.position;
        GameObject newFireball = Instantiate(fireballPrefab, position + Quaternion.Euler(0f, rotation.eulerAngles.y, 0f) * fireballSpawnOffset, rotation);
        newFireball.GetComponent<FireballBehaviour>().originPlatformTower = this.gameObject;
        GameLogic.GameInstance.UseFireballSpell();
    }
    public void LevelUpTower()
    {
        if (GameLogic.GameInstance.Coins < levelUpCost)
        {
            Debug.Log("Not enough Coins to Level Up Turret");
        }
        else if(towerLevel >= maxTowerLevel)
        {
            Debug.Log("Max level reached");
        }
        else
        {
            GameLogic.GameInstance.SpendCoins(levelUpCost);
            towerLevel++;
            GameLogic.GameInstance.PlatformLevel++;
            levelUpCost += levelUpCostIncrement;
            GetComponentInChildren<TowerUI>().UpdateNameText();
            
            GetComponentInChildren<TowerUI>().UpdateUpgradeButtonText();
        }

    }
    public void UnBuild()
    {
        if (GameLogic.GameInstance.Turn == GameLogic.TurnPhase.Building)
        {
            ReturnCoinsForTower();
            leftController.GetComponent<TowerBuilder>().PlatformUnbuilt();
            GameLogic.GameInstance.PlatformLevel = 0;
            Destroy(this.gameObject);
        }
    }

    private void ReturnCoinsForTower()
    {
        if (buildedTurn == GameLogic.GameInstance.TurnNumber)
        {
            GameLogic.GameInstance.GetCoins(towerCost);
        }
        else if (buildedTurn < GameLogic.GameInstance.TurnNumber)
        {
            GameLogic.GameInstance.GetCoins(towerCost / 2);
        }
    }

    public bool IsFireballAlive
    {
        set
        {
            isFireballAlive = value;
        }
    }

    public GameObject XROrigin
    {
        set
        {
            xrOrigin = value;
        }
    }
    public Camera MainCamera
    {
        set
        {
            mainCamera = value;
        }
    }
    public int TowerCost
    {
        set
        {
            towerCost = value;
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
    public GameObject LeftController
    {
        set
        {
            leftController = value;
        }
    }
}
