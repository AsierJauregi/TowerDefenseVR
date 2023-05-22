using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class TowerUI : MonoBehaviour
{
    private const string turretTag = "Tower";
    private const string platformTag = "PlatformTower";
    [SerializeField] private GameObject towerNameTextPanel;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField]private Camera mainCamera;
    private GameObject towerCanvas;

    private int towerLevel;
    private int towerLevelUpCost;
    [SerializeField] private string towerTag;

    private void Awake()
    {
        towerNameTextPanel = transform.GetChild(0).gameObject;
        towerTag = transform.parent.parent.gameObject.tag;
        towerCanvas = transform.parent.gameObject;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        UpdateNameText();
        UpdateUpgradeButtonText();

        LookAtConstraint lookAtConstraint = towerCanvas.GetComponent<LookAtConstraint>();
        if (lookAtConstraint == null)
        {
            lookAtConstraint = towerCanvas.AddComponent<LookAtConstraint>();
        }

        ConstraintSource constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = mainCamera.transform;
        constraintSource.weight = 1f;
        lookAtConstraint.AddSource(constraintSource);
        lookAtConstraint.constraintActive = true;
    }
    public void UpdateNameText()
    {
        GetTowerLevel();
        TextMeshProUGUI towerNameText = towerNameTextPanel.GetComponent<TextMeshProUGUI>();
        if(towerTag == turretTag)
        {
            towerNameText.text = "Tower Lvl: " + towerLevel;
        }
        else if(towerTag == platformTag)
        {
            towerNameText.text = "Platform Lvl: " + towerLevel;
        }
        
    }
    public void UpdateUpgradeButtonText()
    {
        GetTowerLevelUpCost();
        TextMeshProUGUI upgradeButtonText = upgradeButton.GetComponentInChildren<TextMeshProUGUI>();
        upgradeButtonText.text = "Upgrade: " + towerLevelUpCost;
    }
    private void GetTowerLevel()
    {
        if(towerTag == turretTag)
        {
            towerLevel = GetComponentInParent<TurretDefender>().TowerLevel;
        }
        else if(towerTag == platformTag)
        {
            towerLevel = GetComponentInParent<PlatformTowerBehaviour>().TowerLevel;
        }
    }
    private void GetTowerLevelUpCost()
    {
        if (towerTag == turretTag)
        {
            towerLevelUpCost = GetComponentInParent<TurretDefender>().TowerLevelUpCost;
        }
        else if (towerTag == platformTag)
        {
            towerLevelUpCost = GetComponentInParent<PlatformTowerBehaviour>().TowerLevelUpCost;
        }
    }

    public Camera MainCamera
    {
        set
        {
            mainCamera = value;
        }
    }
}
