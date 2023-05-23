using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour, IPointerExitHandler
{

    [SerializeField] private GameObject towerNameTextPanel;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject priorizationButton;
    [SerializeField] private GameObject normalEnemyTagButton;
    [SerializeField] private GameObject fastEnemyTagButton;
    [SerializeField]private Camera mainCamera;
    private GameObject towerCanvas;
    [SerializeField] private GameObject priorizationCanvas;

    private int towerLevel;
    private int towerLevelUpCost;
    [SerializeField] private string towerTag;
    private const string turretTag = "Tower";
    private const string platformTag = "PlatformTower";
    private bool isFirstHover = false;
    private bool isPriorizationCanvasChanged = false;

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
        towerCanvas.GetComponent<Canvas>().enabled = false;
        if(towerTag == turretTag) priorizationCanvas.GetComponent<Canvas>().enabled = false;
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

    public void OnHoverEnter()
    {
        if (!isFirstHover)
        {
            FirstHoverEnter();
        }
        else
        {
            towerCanvas.GetComponent<Canvas>().enabled = true;
        }
    }

    public void FirstHoverEnter()
    {
        isFirstHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (towerTag == turretTag)
        {
            TurretOnPointerExit();
        }
        else if (towerTag == platformTag)
        {
            PlatformOnPointerExit();
        }
    }

    private void TurretOnPointerExit()
    {
        if (priorizationButton.GetComponent<Button>().IsInteractable())
        {
            towerCanvas.GetComponent<Canvas>().enabled = false;
        }
        else
        {
            if (!normalEnemyTagButton.GetComponent<Button>().IsInteractable() && isPriorizationCanvasChanged)
            {
                towerCanvas.GetComponent<Canvas>().enabled = false;
            }
        }
    }

    private void PlatformOnPointerExit()
    {
        towerCanvas.GetComponent<Canvas>().enabled = false;
    }

    public void PriorizationButton() 
    {
        Debug.Log("Priorization Button clicked");
        isPriorizationCanvasChanged = false;
    }

    public Camera MainCamera
    {
        set
        {
            mainCamera = value;
        }
    }
    public void IsPriorizationCanvasChanged()
    {
        isPriorizationCanvasChanged = true;
    }
}
