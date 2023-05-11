using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurretUI : MonoBehaviour
{
    [SerializeField] private GameObject towerNameTextPanel;
    [SerializeField] private GameObject TowerCanvas;
    [SerializeField] private int towerLevel;


    private void Awake()
    {
        towerNameTextPanel = transform.GetChild(0).gameObject;
        UpdateText();
        
    }
    public void UpdateText()
    {
        GetTowerLevel();
        TextMeshProUGUI towerNameText = towerNameTextPanel.GetComponent<TextMeshProUGUI>();
        towerNameText.text = "Tower Lvl: " + towerLevel;
    }
    private void GetTowerLevel()
    {
        towerLevel = GetComponentInParent<TurretDefender>().TowerLevel;
    }

    
}
