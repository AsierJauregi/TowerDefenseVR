using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject coinQuantityTextPanel;
    [SerializeField] private GameObject RightControllerCanvas;
    [SerializeField] private int coins;

    private void Start()
    {
        UpdateCoins();
    }

    public void UpdateCoins()
    {   
        GetCoins();
        TextMeshProUGUI coinQuantityText = coinQuantityTextPanel.GetComponent<TextMeshProUGUI>();
        coinQuantityText.text = ""+coins;
    }

    private void GetCoins()
    {
        coins = GameLogic.GameInstance.Coins;
    }
}
