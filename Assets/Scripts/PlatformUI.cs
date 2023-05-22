using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformUI : MonoBehaviour
{
    [SerializeField] private GameObject towerNameTextPanel;
    [SerializeField] private GameObject TowerCanvas;
    private int towerLevel;


    private void Awake()
    {
        towerNameTextPanel = transform.GetChild(0).gameObject;
        UpdateText();

    }
    public void UpdateText()
    {
        GetTowerLevel();
        TextMeshProUGUI towerNameText = towerNameTextPanel.GetComponent<TextMeshProUGUI>();
        towerNameText.text = "Platform Lvl: " + towerLevel;
    }
    private void GetTowerLevel()
    {
        towerLevel = GetComponentInParent<PlatformTowerBehaviour>().TowerLevel;
    }


}
