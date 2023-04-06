using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExampleInputSystemScript : MonoBehaviour
{
    [SerializeField] InputActionAsset misAcciones;
    [SerializeField] InputActionReference referenciaAccion;

    private void Awake()
    {
        misAcciones.Enable();

        referenciaAccion.action.performed += Action_performed;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("Mensaje de prueba");
    }
}
