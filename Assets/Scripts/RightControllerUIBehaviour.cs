using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RightControllerUIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject coinQuantityTextPanel;
    [SerializeField] private GameObject towerCostText;
    [SerializeField] private GameObject fireballQuantityTextPanel;
    [SerializeField] private GameObject RightControllerCanvas;
    [SerializeField] private int coins;
    [SerializeField] private int fireballSpells;
    [SerializeField] private int towerCost;

    private void Start()
    {
        UpdateCoins();
    }

    public void UpdateCoins()
    {   
        GetCoins();
        TextMeshProUGUI coinQuantityText = coinQuantityTextPanel.GetComponent<TextMeshProUGUI>();
        coinQuantityText.text = "" + coins;
    }
    public void UpdateFireballSpells()
    {
        GetFireballSpells();
        TextMeshProUGUI fireballQuantityText = fireballQuantityTextPanel.GetComponent<TextMeshProUGUI>();
        fireballQuantityText.text = "" + fireballSpells;
    }

    private void GetFireballSpells()
    {
        fireballSpells = GameLogic.GameInstance.FireballSpells;
    }

    private void GetCoins()
    {
        coins = GameLogic.GameInstance.Coins;
    }
    
    public void EnableTowerCostText(bool boolean)
    {   
        if(boolean)UpdateTowerCostText();
        towerCostText.gameObject.SetActive(boolean);
    }
    public void UpdateTowerCostText()
    {
        towerCostText.GetComponent<TextMeshProUGUI>().text = "-" + towerCost;
    }

    public int TowerCost
    {
        set
        {
            towerCost = value;
        }
    }
}
