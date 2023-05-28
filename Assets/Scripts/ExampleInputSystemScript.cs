using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExampleInputSystemScript : MonoBehaviour
{
    
    [SerializeField] InputActionAsset myActions;
    
    [Header ("Actions")]
    [SerializeField] InputActionReference buildReference;
    [SerializeField] InputActionReference letBuildReference;
    [SerializeField] InputActionReference endBuildingReference;
    [SerializeField] InputActionReference menuSelectionReference;

    [Header ("Controllers")]
    [SerializeField] GameObject leftController;
    [SerializeField] GameObject rightController;

    [SerializeField] GameObject radialMenu;
    [SerializeField]private GameLogic gameInstance;
    private Vector2 menuSelectionDirection;
    private const int buildingTurretCost = 30;
    private const int buildingPlatformCost = 60;

    bool isBuilding = false;

    private void Awake()
    {
        radialMenu.SetActive(false);
        

        buildReference.action.performed += Build_performed;
        letBuildReference.action.performed += Let_Build;
        endBuildingReference.action.performed += BuildingTurnEnded;

        leftController.GetComponent<TowerBuilder>().BuildingTurretCost = buildingTurretCost;
        leftController.GetComponent<TowerBuilder>().BuildingPlatformCost = buildingPlatformCost;

    }

    private void OnDestroy()
    {
        buildReference.action.performed -= Build_performed;
        letBuildReference.action.performed -= Let_Build;
        endBuildingReference.action.performed -= BuildingTurnEnded;
    }

    private void Update()
    {
        if (isBuilding)
        {
            Vector2 JoystickInput = menuSelectionReference.action.ReadValue<Vector2>();
            JoystickInput = new Vector2(-JoystickInput.x, JoystickInput.y);
            radialMenu.GetComponent<RadialMenu>().TouchPosition = JoystickInput; 
        }
    }

    private void OnEnable()
    {
        myActions.Enable();
        menuSelectionReference.action.Enable();
    }

    private void OnDisable()
    {
        menuSelectionReference.action.Disable();
        myActions.Disable();
    }

    private void Build_performed(InputAction.CallbackContext obj)
    {
        if (isBuilding)
        {
            if (leftController.GetComponent<TowerBuilder>().CurrentTowerType == TowerBuilder.TowerType.Turret)
            {
                BuildTower(buildingTurretCost);
            }
            else if (leftController.GetComponent<TowerBuilder>().CurrentTowerType == TowerBuilder.TowerType.Platform)
            { 
                BuildTower(buildingPlatformCost);
            }
            else
            {
                Debug.Log("Tower Type could not be resolved");
            }
        }
    }

    private void BuildTower(int buildingCost)
    {
        if (gameInstance.Coins >= buildingCost)
        {
            if (leftController.GetComponent<TowerBuilder>().BuildTower(buildingCost))
            {
                isBuilding = false;
                leftController.GetComponent<TowerBuilder>().enabled = false;
                gameInstance.SpendCoins(buildingCost);
                rightController.GetComponentInChildren<RightControllerUIBehaviour>().EnableTowerCostText(false);
                radialMenu.SetActive(false);
            }
        }
    }

    private void Let_Build(InputAction.CallbackContext obj)
    {
        if (!isBuilding && gameInstance.Turn == GameLogic.TurnPhase.Building)
        {
            Debug.Log("Building");
            isBuilding = true;
            leftController.GetComponent<TowerBuilder>().enabled = true;
            radialMenu.SetActive(true);
            radialMenu.SetActive(true);
        }
        else
        {
            Debug.Log("Not building");
            isBuilding = false;
            rightController.GetComponentInChildren<RightControllerUIBehaviour>().EnableTowerCostText(false);
            radialMenu.SetActive(false);
            leftController.GetComponent<TowerBuilder>().StopPrevisualization();
            leftController.GetComponent<TowerBuilder>().enabled = false;
        }
    }
    private void BuildingTurnEnded(InputAction.CallbackContext obj)
    {
        if(gameInstance.Turn == GameLogic.TurnPhase.Building) gameInstance.PassTurn();
    }

    public GameLogic Game
    {
        get
        {
            return gameInstance;
        }
        set
        {
            gameInstance = value;
        }
    }
}
