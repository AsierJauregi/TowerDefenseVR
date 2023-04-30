using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExampleInputSystemScript : MonoBehaviour
{
    [SerializeField] InputActionAsset myActions;
    [SerializeField] InputActionReference actionReference;
    [SerializeField] InputActionReference buildReference;
    [SerializeField] InputActionReference letBuildReference;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject leftController;
    [SerializeField] GameObject rightController;
    bool isBuilding = false;
    bool on;

    private void Awake()
    {
        on = false;
        myActions.Enable();
        canvas.SetActive(false);
        canvas.SetActive(true);
        actionReference.action.performed += Action_performed;
        buildReference.action.performed += Build_performed;
        letBuildReference.action.performed += Let_Build;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        
        if (!on) { 
            Debug.Log("Canvas activated");
            canvas.SetActive(true);
            on = true;
        }
        if (on)
        {
            Debug.Log("Canvas desactivated");
            canvas.SetActive(false);
            on = false;
        }
    }
    private void Build_performed(InputAction.CallbackContext obj)
    {
        if (isBuilding)
        {
            if (leftController.GetComponent<TowerBuilder>().BuildTower())
            {
                isBuilding = false;
                leftController.GetComponent<TowerBuilder>().enabled = false;
            }
        }
    }

    private void Let_Build(InputAction.CallbackContext obj)
    {
        if (!isBuilding)
        {
            Debug.Log("Building");
            isBuilding = true;
            leftController.GetComponent<TowerBuilder>().enabled = true;
        }
        else
        {
            Debug.Log("Not building");
            isBuilding = false;
            leftController.GetComponent<TowerBuilder>().StopPrevisualization();
            leftController.GetComponent<TowerBuilder>().enabled = false;
        }
    }
}
