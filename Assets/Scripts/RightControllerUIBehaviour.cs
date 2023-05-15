using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RightControllerUIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject coinQuantityTextPanel;
    [SerializeField] private GameObject fireballQuantityTextPanel;
    [SerializeField] private GameObject RightControllerCanvas;
    [SerializeField] private int coins;
    [SerializeField] private int fireballSpells;

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
}
