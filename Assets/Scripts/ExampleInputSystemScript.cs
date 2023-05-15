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
    [SerializeField] InputActionReference menuSelectionVerticalReference;
    [SerializeField] InputActionReference menuSelectionHorizontalReference;
    [SerializeField] InputActionReference menuSelectionReference;

    [Header ("Controllers")]
    [SerializeField] GameObject leftController;
    [SerializeField] GameObject rightController;

    [SerializeField] GameObject radialMenu;
    [SerializeField]private GameLogic gameInstance;
    private Vector2 menuSelectionDirection;
    [SerializeField] private const int buildingCost = 30;
    bool isBuilding = false;

    private void Awake()
    {
        radialMenu.SetActive(false);
        

        buildReference.action.performed += Build_performed;
        letBuildReference.action.performed += Let_Build;
        endBuildingReference.action.performed += BuildingTurnEnded;
    }

    private void Update()
    {
        if (isBuilding)
        {
            float verticalJoystickInput = menuSelectionVerticalReference.action.ReadValue<float>();
            float horizontalJoystickInput = menuSelectionHorizontalReference.action.ReadValue<float>();
            menuSelectionDirection.Set(horizontalJoystickInput, verticalJoystickInput);

            Vector2 JoystickInput = menuSelectionReference.action.ReadValue<Vector2>();
            radialMenu.GetComponent<RadialMenu>().TouchPosition = JoystickInput; 
        }
    }

    private void OnEnable()
    {
        myActions.Enable();
        menuSelectionVerticalReference.action.Enable();
        menuSelectionHorizontalReference.action.Enable();
        menuSelectionReference.action.Enable();
    }

    private void OnDisable()
    {
        menuSelectionVerticalReference.action.Disable();
        menuSelectionHorizontalReference.action.Disable();
        menuSelectionReference.action.Disable();
        myActions.Disable();
    }

    private void Build_performed(InputAction.CallbackContext obj)
    {
        if (isBuilding)
        {
            if (gameInstance.Coins >= buildingCost)
            {
                if (leftController.GetComponent<TowerBuilder>().BuildTower(buildingCost))
                {
                    isBuilding = false;
                    leftController.GetComponent<TowerBuilder>().enabled = false;
                    gameInstance.SpendCoins(buildingCost);
                }
            }
        }
    }

    private void Let_Build(InputAction.CallbackContext obj)
    {
        if (!isBuilding && gameInstance.Turn == GameLogic.TurnPhase.Building)
        {
            Debug.Log("Building");
            isBuilding = true;
            radialMenu.SetActive(true);
            leftController.GetComponent<TowerBuilder>().enabled = true;
        }
        else
        {
            Debug.Log("Not building");
            isBuilding = false;
            radialMenu.SetActive(false);
            leftController.GetComponent<TowerBuilder>().StopPrevisualization();
            leftController.GetComponent<TowerBuilder>().enabled = false;
        }
    }
    private void BuildingTurnEnded(InputAction.CallbackContext obj)
    {
        gameInstance.PassTurn();
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
